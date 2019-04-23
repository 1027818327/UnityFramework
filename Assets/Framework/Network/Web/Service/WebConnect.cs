#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/21 15:31:34
 *  
 */
#endregion


using Framework.Event;

namespace Framework.Network.Web
{
    public class WebConnect : INetworkConnect
    {
        #region Properties
        public IMessageHandle MessageHandle
        {
            get;
            set;
        }
        #endregion

        #region Protected & Public Methods
        public WebConnect()
        {
            MessageHandle = new WebMessageHandle();
        }

        public void ConnectServer()
        {
            WebMgr.SrvConn.Connect(WebConfig.ConnectAddress);
        }

        public void DisconnectServer()
        {
            WebMgr.SrvConn.Close();
        }

        public bool IsConnect()
        {
            WebConnection.Status tempStatus = WebMgr.SrvConn.GetStatus();
            if (tempStatus == WebConnection.Status.None)
            {
                return false;
            }
            return true;
        }

        public void AddConnectListener(OnNotificationDelegate varDelegate)
        {
            EventManager.GetInstance().AddEventListener(WebConfig.WebSocketOpen, varDelegate);
        }

        public void RemoveConnectListener(OnNotificationDelegate varDelegate)
        {
            EventManager.GetInstance().RemoveEventListener(WebConfig.WebSocketOpen, varDelegate);
        }
        #endregion
    }
}
