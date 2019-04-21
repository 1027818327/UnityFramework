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



using System;

namespace Framework.Network
{
    public interface IRoomModule
    {
        #region Properties

        #endregion

        #region Protected & Public Methods

        /// <summary>
        /// 请求创建房间
        /// </summary>
        /// <param name="response">响应</param>
        /// <returns>false表示网络异常</returns>
        bool RequestCreate(Action<EventArgs> response);

        /// <summary>
        /// 请求加入房间
        /// </summary>
        /// <param name="response">响应</param>
        /// <returns>false表示网络异常</returns>
        bool RequestJoin(Action<EventArgs> response);

        /// <summary>
        /// 请求查看房间列表
        /// </summary>
        /// <param name="response">响应</param>
        /// <returns>false表示网络异常</returns>
        bool RequestRoomList(Action<EventArgs> response);

        /// <summary>
        /// 请求获取房间信息
        /// </summary>
        void RequestGetRoomInfo();

        /// <summary>
        /// 请求离开房间
        /// </summary>
        void RequestLeaveRoom();

        /// <summary>
        /// 请求解散房间
        /// </summary>
        void RequestDissolveRoom();

        /// <summary>
        /// 请求战斗
        /// </summary>
        void RequestFight();
        #endregion
    }
}