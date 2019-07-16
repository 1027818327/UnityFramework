﻿#region 版权信息
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
using System;

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
            WebMgr.SrvConn.CloseDirect();
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

        public void AddConnectListener(Action<bool> varAction)
        {
            WebMgr.SrvConn.mConnectAction += varAction;
        }

        public void RemoveConnectListener(Action<bool> varAction)
        {
            WebMgr.SrvConn.mConnectAction -= varAction;
        }
        #endregion
    }
}
