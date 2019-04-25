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
                    onSuccess.Invoke(rb);
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

            IMessageHandle tempMh = null;
            INetworkConnect tempConnect = PlayerManager.GetInstance().NetworkConnect;
            if (tempConnect == null || (tempConnect.MessageHandle as WebMessageHandle) == null)
            {
                tempMh = new WebMessageHandle();
            }
            else
            {
                tempMh = tempConnect.MessageHandle;
            }

            ProtocolJson tempPj = new ProtocolJson();
            CS_GetRoomInfo tempData = new CS_GetRoomInfo();
            tempData.protocolName = ProtocolConst.GetRoomInfo;
            string id = PlayerManager.GetInstance().GetPlayerId();
            tempData.id = id;
            tempData.room_id = PlayerManager.GetInstance().Room.RoomId.ToString();

            tempPj.Serialize(tempData);
            tempMh.SendPacket(tempPj);
        }

        public void RequestLeaveRoom()
        {
            if (IsLogin() == false) return;
            if (IsInRoom() == false) return;

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
            ProtocolJson tempJson = new ProtocolJson();
            CS_LeaveRoom tempData = new CS_LeaveRoom();
            tempData.protocolName = ProtocolConst.LeaveRoom;
            tempData.id = PlayerManager.GetInstance().GetPlayerId();
            tempData.room_id = PlayerManager.GetInstance().Room.RoomId.ToString();
            tempJson.Serialize(tempData);
            tempMh.SendPacket(tempJson);

            /*
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();

            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);
            tempDic.Add("room_id", roomId);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log(result);
            };
            mHttp.SendPostAnsyc(mRoomUrl.leaveRoomUrl, tempDic, tempA);
            */
        }

        public void RequestDissolveRoom()
        {
            if (IsLogin() == false) return;
            if (IsInRoom() == false) return;
            if (IsMaster() == false) return;

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
            ProtocolJson tempJson = new ProtocolJson();
            CS_DissolveRoom tempData = new CS_DissolveRoom();
            tempData.protocolName = ProtocolConst.DissolveRoom;
            tempData.id = PlayerManager.GetInstance().GetPlayerId();
            tempData.room_id = PlayerManager.GetInstance().Room.RoomId.ToString();
            tempJson.Serialize(tempData);
            tempMh.SendPacket(tempJson);

            /*
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();

            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);
            tempDic.Add("room_id", roomId);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log(result);
            };
            mHttp.SendPostAnsyc(mRoomUrl.dissolveRoomUrl, tempDic, tempA);
            */
        }

        public void RequestFight()
        {
            if (IsLogin() == false) return;
            if (IsInRoom() == false) return;
            if (IsMaster() == false) return;

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
            ProtocolJson tempJson = new ProtocolJson();
            CS_StartFight tempData = new CS_StartFight();
            tempData.protocolName = ProtocolConst.StartFight;
            tempData.id = PlayerManager.GetInstance().GetPlayerId();
            tempData.room_id = PlayerManager.GetInstance().Room.RoomId.ToString();
            tempJson.Serialize(tempData);
            tempMh.SendPacket(tempJson);

            /*
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();

            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);
            tempDic.Add("room_id", roomId);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log(result);
            };
            mHttp.SendPostAnsyc(mRoomUrl.startFightUrl, tempDic, tempA);
            */
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

        #endregion

        #region Response
        private void ResponseLeaveRoom(ProtocolBase protocol)
        {
            ProtocolJson tempJson = protocol as ProtocolJson;
            string id = tempJson.JsonData["id"].ToString();
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
            string id = tempJson.JsonData["id"].ToString();
            Room tempRoom = PlayerManager.GetInstance().Room;
            if (tempRoom == null)
            {
                Debuger.LogError("房间不存在");
                return;
            }
            Player tempP = tempRoom.GetPlayer(id);
            if (tempP != null && tempP.RoomIdentity == RoomIdentity.RoomMaster)
            {
                /// 如果是房主解散房间则清空房间数据
                PlayerManager.GetInstance().Room = null;
                PlayerManager.GetInstance().NetworkConnect.DisconnectServer();
            }
        }
        #endregion
    }
}
