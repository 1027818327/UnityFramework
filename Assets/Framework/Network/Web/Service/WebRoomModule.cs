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

using Framework.Http;
using System;
using System.Collections.Generic;

namespace Framework.Network.Web
{
    public class WebRoomModule : IRoomModule
    { 
        private HttpUtils mHttp;
        private string createRoomUrl;
        private string joinRoomUrl;
        private string getRoomListUrl;
        private INetworkConnect webConnect;

        public WebRoomModule(string createRoomUrl, string joinRoomUrl, string getRoomListUrl)
        {
            mHttp = new HttpUtils();
            webConnect = new WebConnect();

            this.createRoomUrl = createRoomUrl;
            this.joinRoomUrl = joinRoomUrl;
            this.getRoomListUrl = getRoomListUrl;
        }

        public void RequestCreate(Action<ResponseBase> onSuccess, Action<ResponseBase> onFail)
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);

            Action<string> tempA = delegate (string result)
            {
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

                    webConnect.ConnectServer();
                }

                if (tempB)
                {
                    if (onSuccess != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.result = result;
                        onSuccess.Invoke(rb);
                    }
                }
                else
                {
                    if (onFail != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.result = tempNode["text"];
                        onFail.Invoke(rb);
                    }
                }
            };

            bool tempConnect = mHttp.SendPostAnsyc(createRoomUrl, tempDic, tempA);
            if (!tempConnect)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.result = "网络异常";
                    onFail.Invoke(rb);
                }
            }
        }

        public void RequestJoin(string roomId, Action<ResponseBase> onSuccess, Action<ResponseBase> onFail)
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);
            tempDic.Add("room_id", roomId);

            Action<string> tempA = delegate (string result)
            {
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

                    webConnect.ConnectServer();
                }

                if (tempRoomNode != null)
                {
                    if (onSuccess != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.result = result;
                        onSuccess.Invoke(rb);
                    }
                }
                else
                {
                    if (onFail != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.result = tempNode["text"];
                        onSuccess.Invoke(rb);
                    }
                }
            };

            bool tempConnect = mHttp.SendPostAnsyc(joinRoomUrl, tempDic, tempA);
            if (!tempConnect)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.result = "网络异常";
                    onSuccess.Invoke(rb);
                }
            }
        }

        public void RequestRoomList(Action<ResponseBase> onSuccess, Action<ResponseBase> onFail)
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            string id = PlayerManager.GetInstance().GetPlayerId();
            string sign = PlayerManager.GetInstance().Sign;
            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);

            Action<string> tempA = delegate (string result)
            {
                SimpleJSON.JSONNode tempNode = SimpleJSON.JSON.Parse(result);
                
            };

            bool tempConnect = mHttp.SendPostAnsyc(getRoomListUrl, tempDic, tempA);
            if (!tempConnect)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.result = "网络异常";
                    onFail.Invoke(rb);
                }
            }
        }

        public void RequestGetRoomInfo()
        {
            ProtocolJson tempPj = new ProtocolJson();
            CS_GetRoomInfo tempData = new CS_GetRoomInfo();
            tempData.protocolName = ProtocolConst.GetRoomInfo;
            string id = PlayerManager.GetInstance().GetPlayerId();
            tempData.id = id;
            tempPj.Serialize(tempData);
            WebMgr.SrvConn.Send(tempPj);
        }

        public void RequestLeaveRoom()
        {
            ProtocolJson tempPj = new ProtocolJson();
            CS_LeaveRoom tempData = new CS_LeaveRoom();
            tempData.protocolName = ProtocolConst.LeaveRoom;
            string id = PlayerManager.GetInstance().GetPlayerId();
            tempData.id = id;
            tempPj.Serialize(tempData);
            WebMgr.SrvConn.Send(tempPj);
        }

        public void RequestDissolveRoom()
        {
            ProtocolJson tempPj = new ProtocolJson();
            CS_DissolveRoom tempData = new CS_DissolveRoom();
            tempData.protocolName = ProtocolConst.DissolveRoom;
            string id = PlayerManager.GetInstance().GetPlayerId();
            tempData.id = id;
            tempPj.Serialize(tempData);
            WebMgr.SrvConn.Send(tempPj);
        }

        public void RequestFight()
        {
            ProtocolJson tempPj = new ProtocolJson();
            CS_StartFight tempData = new CS_StartFight();
            tempData.protocolName = ProtocolConst.StartFight;
            string id = PlayerManager.GetInstance().GetPlayerId();
            tempData.id = id;
            tempPj.Serialize(tempData);
            WebMgr.SrvConn.Send(tempPj);
        }
    }
}
