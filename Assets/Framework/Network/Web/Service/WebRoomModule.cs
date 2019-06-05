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
            if (IsLogin() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先登陆";
                    onFail.Invoke(rb);
                }
                return;
            }
            

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

        public void RequestCreate(int course, Action<ResponseBase> onSuccess, Action<ResponseBase> onFail)
        {
            if (IsLogin() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先登陆";
                    onFail.Invoke(rb);
                }
                return;
            }


            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);
            tempDic.Add("course", course.ToString());

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
            if (IsLogin() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先登陆";
                    onFail.Invoke(rb);
                }
                return;
            }

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

                bool tempSuccess = tempNode["success"].AsBool;
                int tempStatus = tempNode["status"].AsInt;

                if (tempSuccess || tempStatus == (int)JoinRoomStatus.InRoom || tempStatus == (int)JoinRoomStatus.Success)
                {
                    int tempRoomId = 0;
                    int.TryParse(roomId, out tempRoomId);
                    if (tempRoomId == 0)
                    {
                        Debuger.LogError("加入的房号错误");
                    }
                    PlayerManager.GetInstance().Room = new Room(tempRoomId);

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
            if (IsLogin() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先登陆";
                    onFail.Invoke(rb);
                }
                return;
            }

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

        public void RequestGetRoomInfo(Action<ResponseBase> onFail)
        {
            if (IsLogin() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先登陆";
                    onFail.Invoke(rb);
                }
                return;
            }
            if (IsInRoom() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先创建或加入房间";
                    onFail.Invoke(rb);
                }
                return;
            }

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string protocolName = ProtocolConst.GetRoomInfo;
            string id = PlayerManager.GetInstance().GetPlayerId();
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();
            string client_id = PlayerManager.GetInstance().Client_id;
            string sign = PlayerManager.GetInstance().Sign;

            tempDic.Add("protocolName", protocolName);
            tempDic.Add("user_id", id);
            tempDic.Add("room_id", roomId);
            tempDic.Add("client_id", client_id);
            tempDic.Add("sign", sign);

            //Debuger.Log(client_id);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log("服务端返回房间信息结果");
                Debuger.Log(result);
            };

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.getRoomInfoUrl, tempDic, tempA);
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

        public void RequestLeaveRoom(Action<ResponseBase> onFail)
        {
            if (IsLogin() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先登陆";
                    onFail.Invoke(rb);
                }
                return;
            }
            if (IsInRoom() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先创建或加入房间";
                    onFail.Invoke(rb);
                }
                return;
            }

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string protocolName = ProtocolConst.LeaveRoom;
            string id = PlayerManager.GetInstance().GetPlayerId();
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();
            string client_id = PlayerManager.GetInstance().Client_id;
            string sign = PlayerManager.GetInstance().Sign;

            tempDic.Add("protocolName", protocolName);
            tempDic.Add("user_id", id);
            tempDic.Add("room_id", roomId);
            tempDic.Add("client_id", client_id);
            tempDic.Add("sign", sign);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log("服务端返回离开房间结果");
                Debuger.Log(result);
            };

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.leaveRoomUrl, tempDic, tempA);
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

        public void RequestDissolveRoom(Action<ResponseBase> onFail)
        {
            if (IsLogin() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先登陆";
                    onFail.Invoke(rb);
                }
                return;
            }
            if (IsInRoom() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先创建或加入房间";
                    onFail.Invoke(rb);
                }
                return;
            }
            if (IsMaster() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "你不是房主，无权限操作";
                    onFail.Invoke(rb);
                }
                return;
            }

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string protocolName = ProtocolConst.DissolveRoom;
            string id = PlayerManager.GetInstance().GetPlayerId();
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();
            string client_id = PlayerManager.GetInstance().Client_id;
            string sign = PlayerManager.GetInstance().Sign;

            tempDic.Add("protocolName", protocolName);
            tempDic.Add("user_id", id);
            tempDic.Add("room_id", roomId);
            tempDic.Add("client_id", client_id);
            tempDic.Add("sign", sign);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log("服务端返回解散房间结果");
                Debuger.Log(result);
            };

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.dissolveRoomUrl, tempDic, tempA);
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

        public void RequestFight(Action<ResponseBase> onFail)
        {
            if (IsLogin() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先登陆";
                    onFail.Invoke(rb);
                }
                return;
            }
            if (IsInRoom() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "请先创建或加入房间";
                    onFail.Invoke(rb);
                }
                return;
            }
            if (IsMaster() == false)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.tips = "你不是房主，无权限操作";
                    onFail.Invoke(rb);
                }
                return;
            }

            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string protocolName = ProtocolConst.StartFight;
            string id = PlayerManager.GetInstance().GetPlayerId();
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();
            string client_id = PlayerManager.GetInstance().Client_id;
            string sign = PlayerManager.GetInstance().Sign;

            tempDic.Add("protocolName", protocolName);
            tempDic.Add("user_id", id);
            tempDic.Add("room_id", roomId);
            tempDic.Add("client_id", client_id);
            tempDic.Add("sign", sign);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log("服务端返回开始战斗结果");
                Debuger.Log(result);
            };

            bool tempConnect = mHttp.SendPostAnsyc(mRoomUrl.startFightUrl, tempDic, tempA);
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

        #region 效验
        private bool IsLogin()
        {
            Player tempP = PlayerManager.GetInstance().Player;
            if (tempP == null || string.IsNullOrEmpty(tempP.Id))
            {
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
            return false;
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
                if (tempRoom == null)
                {
                    tempRoom = new Room(tempInfo.room.id);
                }

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

                string tempRoomMasterId = tempInfo.room.user_id.ToString();
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

                    if (tempPlayer.Id.Equals(tempRoomMasterId))
                    {
                        tempPlayer.RoomIdentity = RoomIdentity.RoomMaster;
                    }
                    else
                    {
                        tempPlayer.RoomIdentity = RoomIdentity.Operator;
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
                PlayerManager.GetInstance().Client_id = null;
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
                PlayerManager.GetInstance().Client_id = null;
            }
        }

        private void ResponseCreateSession(ProtocolBase protocol)
        {
            ProtocolJson tempJson = protocol as ProtocolJson;
            string client_id = tempJson.JsonData["client_id"].ToString();
            PlayerManager.GetInstance().Client_id = client_id;

            if (PlayerManager.GetInstance().Room == null)
            {
                Debuger.LogError("房间未初始化，等待初始化");
            }
        }
        #endregion
    }
}
