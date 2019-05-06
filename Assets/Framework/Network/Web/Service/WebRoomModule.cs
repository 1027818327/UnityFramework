#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/19 15:31:34
 *  
 */
#endregion

using Framework.Debugger;
using Framework.Http;
using Framework.Unity.UI;
using System;
using System.Collections.Generic;

namespace Framework.Network.Web
{
    public class WebRoomModule : IRoomModule
    { 
        private HttpUtils mHttp;
        private RoomUrl mRoomUrl;

        public struct RoomUrl
        {
            public string createRoomUrl;
            public string joinRoomUrl;
            public string getRoomListUrl;
            public string getRoomInfoUrl;
            public string leaveRoomUrl;
            public string dissolveRoomUrl;
            public string startFightUrl;
        }

        public WebRoomModule(RoomUrl roomUrl)
        {
            mHttp = new HttpUtils();
            mRoomUrl = roomUrl;

            IMessageHandle tempMh = null;
            INetworkConnect tempNC = PlayerManager.GetInstance().NetworkConnect;
            if (tempNC == null || tempNC.MessageHandle as WebMessageHandle == null)
            {
                tempMh = new WebMessageHandle();
            }
            else
            {
                tempMh = tempNC.MessageHandle;
            }

            tempMh.AddListener(ProtocolConst.CreateSession, ResponseCreateSession);
            tempMh.AddListener(ProtocolConst.GetRoomInfo, ResponseGetRoomInfo);
            tempMh.AddListener(ProtocolConst.LeaveRoom, ResponseLeaveRoom);
            tempMh.AddListener(ProtocolConst.DissolveRoom, ResponseDissolveRoom);
        }

        public void RequestCreate(Action<ResponseBase> onSuccess, Action<ResponseBase> onFail)
        {
            if (IsLogin() == false) return;

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);

            Room tempRoom = PlayerManager.GetInstance().Room;
            if (tempRoom != null)
            {
                tempDic.Add("room_id", tempRoom.RoomId.ToString());
            }
            else
            {
                tempDic.Add("room_id", "0");
            }

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log("服务端返回创建房间结果");
                Debuger.Log(result);
                SimpleJSON.JSONNode tempNode = SimpleJSON.JSON.Parse(result);
                string tempS = tempNode["success"];
                bool tempB = Convert.ToBoolean(tempS);
                if (tempB)
                {
                    string tempRoomId = tempNode["text"]["id"];
                    int tempRoomIdInt = Convert.ToInt32(tempRoomId);
                    PlayerManager.GetInstance().Room = new Room(tempRoomIdInt, PlayerManager.GetInstance().Player);
                    SimpleJSON.JSONArray tempRoomArray = tempNode["text"]["room_info"] as SimpleJSON.JSONArray;

                    foreach (SimpleJSON.JSONNode element in tempRoomArray)
                    {
                        string userId = element["user_id"];
                        string userName = element["name"];
                        string roleId = element["role_id"];
                        Player tempP = new Player(userId, userName);
                        tempP.RoleId = Convert.ToInt32(roleId);

                        PlayerManager.GetInstance().Room.Enter(tempP);
                    }
                }

                if (tempB)
                {
                    if (onSuccess != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.tips = "创建房间成功";
                        rb.result = result;
                        onSuccess.Invoke(rb);
                    }
                }
                else
                {
                    if (onFail != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.tips = tempNode["text"];
                        rb.result = result;
                        onFail.Invoke(rb);
                    }
                }
            };

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.createRoomUrl, tempDic, tempA);
            if (!tempConnect)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "网络异常";
                    onFail.Invoke(rb);
                }
            }
        }

        public void RequestJoin(string roomId, Action<ResponseBase> onSuccess, Action<ResponseBase> onFail)
        {
            if (IsLogin() == false) return;

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);
            tempDic.Add("room_id", roomId);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log("服务端返回加入房间结果");
                Debuger.Log(result);
                SimpleJSON.JSONNode tempNode = SimpleJSON.JSON.Parse(result);

                var tempRoomNode = tempNode["info"];
                if (tempRoomNode != null)
                {
                    // 加入房间成功
                    string tempRoomId = tempRoomNode["id"];
                    int tempRoomIdInt = Convert.ToInt32(tempRoomId);
                    PlayerManager.GetInstance().Room = new Room(tempRoomIdInt);
                    SimpleJSON.JSONArray tempRoomArray = tempRoomNode["room_info"] as SimpleJSON.JSONArray;

                    string tempMasterId = tempRoomNode["user_id"];

                    foreach (SimpleJSON.JSONNode element in tempRoomArray)
                    {
                        string userId = element["user_id"];
                        string userName = element["name"];
                        string roleId = element["role_id"];
                        Player tempP = new Player(userId, userName);
                        tempP.RoleId = Convert.ToInt32(roleId);
                        if (userId.Equals(tempMasterId))
                        {
                            tempP.RoomIdentity = RoomIdentity.RoomMaster;
                        }
                        PlayerManager.GetInstance().Room.Enter(tempP);
                    }
                }

                if (tempRoomNode != null)
                {
                    if (onSuccess != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.tips = tempNode["text"];
                        rb.result = result;
                        onSuccess.Invoke(rb);
                    }
                }
                else
                {
                    if (onFail != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.tips = tempNode["text"];
                        rb.result = result;
                        onFail.Invoke(rb);
                    }
                }
            };

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.joinRoomUrl, tempDic, tempA);
            if (!tempConnect)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "网络异常";
                    onFail.Invoke(rb);
                }
            }
        }

        public void RequestRoomList(Action<ResponseBase> onSuccess, Action<ResponseBase> onFail)
        {
            if (IsLogin() == false) return;

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log("服务端返回房间列表结果");
                Debuger.Log(result);
                SimpleJSON.JSONNode tempNode = SimpleJSON.JSON.Parse(result);
                string tempS = tempNode["success"];
                bool tempB = Convert.ToBoolean(tempS);
                if (tempB)
                {
                    if (onSuccess != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.tips = "请求房间列表成功";
                        rb.result = result;
                        onSuccess.Invoke(rb);
                    }
                }
                else
                {
                    if (onFail != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.tips = "请求房间列表失败";
                        rb.result = result;
                        onFail.Invoke(rb);
                    }
                }
            };

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.getRoomListUrl, tempDic, tempA);
            if (!tempConnect)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "网络异常";
                    onFail.Invoke(rb);
                }
            }
        }

        public void RequestGetRoomInfo()
        {
            if (IsLogin() == false) return;
            if (IsInRoom() == false) return;

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string protocolName = ProtocolConst.GetRoomInfo;
            string id = PlayerManager.GetInstance().GetPlayerId();
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();
            string client_id = PlayerManager.GetInstance().Room.Client_id;
            string sign = PlayerManager.GetInstance().Sign;

            tempDic.Add("protocolName", protocolName);
            tempDic.Add("user_id", id);
            tempDic.Add("room_id", roomId);
            tempDic.Add("client_id", client_id);
            tempDic.Add("sign", sign);

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.getRoomInfoUrl, tempDic, null);
            if (!tempConnect)
            {
                NetworkException();
            }
        }

        public void RequestLeaveRoom()
        {
            if (IsLogin() == false) return;
            if (IsInRoom() == false) return;

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string protocolName = ProtocolConst.LeaveRoom;
            string id = PlayerManager.GetInstance().GetPlayerId();
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();
            string client_id = PlayerManager.GetInstance().Room.Client_id;
            string sign = PlayerManager.GetInstance().Sign;

            tempDic.Add("protocolName", protocolName);
            tempDic.Add("user_id", id);
            tempDic.Add("room_id", roomId);
            tempDic.Add("client_id", client_id);
            tempDic.Add("sign", sign);

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.leaveRoomUrl, tempDic, null);
            if (!tempConnect)
            {
                NetworkException();
            }
        }

        public void RequestDissolveRoom()
        {
            if (IsLogin() == false) return;
            if (IsInRoom() == false) return;
            if (IsMaster() == false) return;

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string protocolName = ProtocolConst.DissolveRoom;
            string id = PlayerManager.GetInstance().GetPlayerId();
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();
            string client_id = PlayerManager.GetInstance().Room.Client_id;
            string sign = PlayerManager.GetInstance().Sign;

            tempDic.Add("protocolName", protocolName);
            tempDic.Add("user_id", id);
            tempDic.Add("room_id", roomId);
            tempDic.Add("client_id", client_id);
            tempDic.Add("sign", sign);

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.dissolveRoomUrl, tempDic, null);
            if (!tempConnect)
            {
                NetworkException();
            }
        }

        public void RequestFight()
        {
            if (IsLogin() == false) return;
            if (IsInRoom() == false) return;
            if (IsMaster() == false) return;

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string protocolName = ProtocolConst.StartFight;
            string id = PlayerManager.GetInstance().GetPlayerId();
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();
            string client_id = PlayerManager.GetInstance().Room.Client_id;
            string sign = PlayerManager.GetInstance().Sign;

            tempDic.Add("protocolName", protocolName);
            tempDic.Add("user_id", id);
            tempDic.Add("room_id", roomId);
            tempDic.Add("client_id", client_id);
            tempDic.Add("sign", sign);

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.startFightUrl, tempDic, null);
            if (!tempConnect)
            {
                NetworkException();
            }
        }

        #region 效验
        private bool IsLogin()
        {
            Player tempP = PlayerManager.GetInstance().Player;
            if (tempP == null || string.IsNullOrEmpty(tempP.Id))
            {
                UIEventArgs<string> tempArgs2 = new UIEventArgs<string>("请先登录");
                UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempArgs2);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否在房间
        /// </summary>
        /// <returns></returns>
        private bool IsInRoom()
        {
            if (PlayerManager.GetInstance().Room == null)
            {
                UIEventArgs<string> tempArgs2 = new UIEventArgs<string>("请先创建或加入房间");
                UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempArgs2);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否是房主
        /// </summary>
        /// <returns></returns>
        private bool IsMaster()
        {
            Player tempP = PlayerManager.GetInstance().Player;
            if (tempP != null && !string.IsNullOrEmpty(tempP.Id) && tempP.RoomIdentity == RoomIdentity.RoomMaster)
            {
                return true;
            }
            UIEventArgs<string> tempArgs2 = new UIEventArgs<string>("你不是房主，无权限操作");
            UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempArgs2);
            return false;
        }

        private void NetworkException()
        {
            UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
            tempData.Title = "提示";
            tempData.Content = "网络异常";
            tempData.Style = UIMsgBox.Style.OKAndCancel;
            UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs2 = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
            UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs2);
        }

        #endregion

        #region Response
        private void ResponseGetRoomInfo(ProtocolBase proto)
        {
            ProtocolJson tempJson = proto as ProtocolJson;
            SC_GetRoomInfo tempInfo = tempJson.Deserialize<SC_GetRoomInfo>();
            if (tempInfo != null)
            {
                SC_Player[] tempPlayers = tempInfo.room.room_info;

                Room tempRoom = PlayerManager.GetInstance().Room;
                List<string> tempList = new List<string>();

                for (int j = 0; j < tempPlayers.Length; j++)
                {
                    string tempId = tempPlayers[j].id.ToString();
                    tempList.Add(tempId);
                }

                for (int i = 0; i < tempRoom.PlayerList.Count;)
                {
                    string tempId = tempRoom.PlayerList[i].Id;
                    if (tempList.Contains(tempId))
                    {
                        i++;
                        continue;
                    }
                    tempRoom.PlayerList.RemoveAt(i);
                }

                foreach (SC_Player tempP in tempPlayers)
                {
                    Player tempPlayer = tempRoom.GetPlayer(tempP.id.ToString());
                    if (tempPlayer == null)
                    {
                        tempPlayer = new Player(tempP.name, tempP.id.ToString());
                        tempPlayer.RoleId = tempP.role_id;
                        PlayerManager.GetInstance().Room.Enter(tempPlayer);
                    }
                    else
                    {
                        tempPlayer.Name = tempP.name;
                        tempPlayer.RoleId = tempP.role_id;
                    }
                }
            }
        }

        private void ResponseLeaveRoom(ProtocolBase protocol)
        {
            ProtocolJson tempJson = protocol as ProtocolJson;
            string id = tempJson.JsonData["user_id"].ToString();
            if (PlayerManager.GetInstance().IsSelf(id))
            {
                /// 如果是自己离开则清空房间数据
                PlayerManager.GetInstance().Room = null;
                PlayerManager.GetInstance().NetworkConnect.DisconnectServer();
            }
        }

        private void ResponseDissolveRoom(ProtocolBase protocol)
        {
            ProtocolJson tempJson = protocol as ProtocolJson;
            
            Room tempRoom = PlayerManager.GetInstance().Room;
            if (tempRoom == null)
            {
                Debuger.LogError("房间不存在");
                return;
            }

            string room_id = tempJson.JsonData["room_id"].ToString();
            int tempRoomId = int.Parse(room_id);
            if (tempRoom.RoomId == tempRoomId)
            {
                PlayerManager.GetInstance().Room = null;
                PlayerManager.GetInstance().NetworkConnect.DisconnectServer();
            }
        }

        private void ResponseCreateSession(ProtocolBase protocol)
        {
            ProtocolJson tempJson = protocol as ProtocolJson;
            string client_id = tempJson.JsonData["client_id"].ToString();
            if (PlayerManager.GetInstance().Room == null)
            {
                Debuger.LogError("房间未初始化，程序异常");
            }
            else
            {
                PlayerManager.GetInstance().Room.Client_id = client_id;
            }
            
        }
        #endregion
    }
}
