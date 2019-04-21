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
    public class WebRoomModule : IWebRoomModule
    {
        private HttpUtils mHttp;

        public WebRoomModule()
        {
            mHttp = new HttpUtils();
        }

        public void RequestCreate(string url, IDictionary<string, string> parameters, Action<string> response)
        {
            mHttp.SendPostAnsyc(url, parameters, response);
        }

        public void RequestJoin(string url, IDictionary<string, string> parameters, Action<string> response)
        {
            mHttp.SendPostAnsyc(url, parameters, response);
        }

        public void RequestRoomList(string url, IDictionary<string, string> parameters, Action<string> response)
        {
            mHttp.SendPostAnsyc(url, parameters, response);
        }

        public void RequestGetRoomInfo(string id)
        {
            ProtocolJson tempPj = new ProtocolJson();
            CS_GetRoomInfo tempData = new CS_GetRoomInfo();
            tempData.protocolName = ProtocolConst.GetRoomInfo;
            tempData.id = id;
            tempPj.Serialize(tempData);
            WebMgr.SrvConn.Send(tempPj);
        }

        public void RequestLeaveRoom(string id)
        {
            ProtocolJson tempPj = new ProtocolJson();
            CS_LeaveRoom tempData = new CS_LeaveRoom();
            tempData.protocolName = ProtocolConst.LeaveRoom;
            tempData.id = id;
            tempPj.Serialize(tempData);
            WebMgr.SrvConn.Send(tempPj);
        }

        public void RequestDissolveRoom(string id)
        {
            ProtocolJson tempPj = new ProtocolJson();
            CS_DissolveRoom tempData = new CS_DissolveRoom();
            tempData.protocolName = ProtocolConst.DissolveRoom;
            tempData.id = id;
            tempPj.Serialize(tempData);
            WebMgr.SrvConn.Send(tempPj);
        }

        public void RequestFight(string id)
        {
            ProtocolJson tempPj = new ProtocolJson();
            CS_StartFight tempData = new CS_StartFight();
            tempData.protocolName = ProtocolConst.StartFight;
            tempData.id = id;
            tempPj.Serialize(tempData);
            WebMgr.SrvConn.Send(tempPj);
        }
    }
}
