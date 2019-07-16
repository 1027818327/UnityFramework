
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/07/05 16:59:22
 *  
 */
#endregion


using Framework.Debugger;
using Framework.Http;
using System;
using System.Collections.Generic;
using System.Net;

namespace Framework.Network.Web
{
    public class WebReconnectModule : IReconnectModule
    {
        #region Fields
        private HttpUtils mHttp;
        private string mReconnectUrl;
        private IReconnectStrategy mReconnectStrategy;

        #endregion

        #region Properties
        public Action<object> RestoreAction { get; set; }
        #endregion

        #region Private Methods
        private void Connected(bool needReconnect)
        {
            if (needReconnect)
            {
                /*
                INetworkConnect tempNC = PlayerManager.GetInstance().NetworkConnect;
                tempNC.MessageHandle.AddOnceListener(ProtocolConst.CreateSession, ResponseCreateSession);
                */

                Debuger.Log("Send ReconnectPacket");
                RequestRestore();
            }
        }

        private void Disconnected(string reason)
        {
            HandleDisconnected(reason);
        }

        private void ResponseCreateSession(ProtocolBase proto)
        {
            Debuger.Log("Send ReconnectPacket");

            RequestRestore();
        }

        #endregion

        #region Protected & Public Methods

        protected virtual void HandleDisconnected(string reason)
        {
            mReconnectStrategy.HandleDisconnected(reason);
        }

        public WebReconnectModule(string reconnectUrl, IReconnectStrategy reconnectStrategy)
        {
            mReconnectUrl = reconnectUrl;
            mReconnectStrategy = reconnectStrategy;

            WebMgr.SrvConn.mConnectAction += Connected;
            WebMgr.SrvConn.mDisconnectAction += Disconnected;
        }

        ~WebReconnectModule()
        {
            WebMgr.SrvConn.mConnectAction -= Connected;
            WebMgr.SrvConn.mDisconnectAction -= Disconnected;
        }

        public void RequestRestore()
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>();

            string id = PlayerManager.GetInstance().GetPlayerId();
            string roomId = PlayerManager.GetInstance().Room.RoomId.ToString();
            string sign = PlayerManager.GetInstance().Sign;

            tempDic.Add("user_id", id);
            tempDic.Add("room_id", roomId);
            tempDic.Add("sign", sign);

            Debuger.LogFormat("签名={0}", sign);

            Action<string> tempA = delegate (string result)
            {
                Debuger.Log("服务端返回重连结果");
                Debuger.Log(result);

                if (RestoreAction != null)
                {
                    RestoreAction(result);
                }
            };

            if (mHttp == null)
            {
                mHttp = new HttpUtils();
            }
            HttpWebRequest tempRequest = mHttp.CreateHttpRequest(mReconnectUrl);
            mHttp.SetParams(tempRequest, tempDic);
            mHttp.SendRequestAnsyc(tempRequest, tempA);
        }
        #endregion
    }
}