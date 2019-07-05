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

using System;

namespace Framework.Network
{
    public interface INetworkConnect
    {
        #region Properties
        IMessageHandle MessageHandle { get; set; }
        #endregion

        #region Protected & Public Methods
        /// <summary>
        /// 连接服务器
        /// </summary>
        void ConnectServer();

        /// <summary>
        /// 断开服务器
        /// </summary>
        void DisconnectServer();

        /// <summary>
        /// 是否连上服务器
        /// </summary>
        /// <returns></returns>
        bool IsConnect();

        void AddConnectListener(Action<bool> varAction);

        void RemoveConnectListener(Action<bool> varAction);

        #endregion
    }
}
