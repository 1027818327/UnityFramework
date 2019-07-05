
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


using System;
using Framework.Unity.UI;
using UnityEngine;

namespace Framework.Network.Web
{
    public class WebReconnectBase : MonoBehaviour
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Unity Messages
        //    void Awake()
        //    {
        //
        //    }
        //    void OnEnable()
        //    {
        //
        //    }
        //
        void Start()
        {
            WebMgr.SrvConn.mConnectAction += Connected;
            WebMgr.SrvConn.mDisconnectAction += Disconnected;
        }
        //    
        //    void Update() 
        //    {
        //    
        //    }
        //
        //    void OnDisable()
        //    {
        //
        //    }
        //
        //    void OnDestroy()
        //    {
        //
        //    }

        #endregion

        #region Private Methods
        private void Connected(bool needReconnect)
        {
            if (needReconnect)
            {
                ProtocolBase tempPb = GetReconnectProtocol();
                // 发送重连消息
                WebMgr.SrvConn.Send(tempPb, OnReceiveReconnectPacket);
            }
        }

   
        private void Disconnected(string reason)
        {
            HandleDisconnected(reason);
        }

        #endregion

        #region Protected & Public Methods

        protected virtual ProtocolBase GetReconnectProtocol()
        {
            return null;
        }

        protected void OnReceiveReconnectPacket(ProtocolBase proto)
        {
            
        }

        protected virtual void HandleDisconnected(string reason)
        {
            UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
            tempData.Title = "提示";
            tempData.Content = reason;
            tempData.Style = UIMsgBox.Style.OK;
            tempData.mBtnTexts = new string[1] { "重连" };
            tempData.CloseAction = delegate (UIMsgBox.Result result)
            {
                WebMgr.SrvConn.StartReconnect();
            };
            UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs2 = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
            UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs2);
        }
        #endregion
    }
}