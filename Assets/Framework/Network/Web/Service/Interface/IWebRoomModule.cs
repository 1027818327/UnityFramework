
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
using System.Collections.Generic;

namespace Framework.Network.Web
{
    public interface IWebRoomModule
    {
        #region Properties

        #endregion

        #region Protected & Public Methods
        /// <summary>
        /// 请求创建房间
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="response">回调函数</param>
        void RequestCreate(string url, IDictionary<string, string> parameters, Action<string> response);

        /// <summary>
        /// 请求加入房间
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="response">回调函数</param>
        void RequestJoin(string url, IDictionary<string, string> parameters, Action<string> response);

        /// <summary>
        /// 请求查看房间列表
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="response">回调函数</param>
        void RequestRoomList(string url, IDictionary<string, string> parameters, Action<string> response);

        /// <summary>
        /// 连接广播服务器
        /// </summary>
        void ConnectBoardcastServer();

        /// <summary>
        /// 请求获取房间信息
        /// </summary>
        /// <param name="id">用户id</param>
        void RequestGetRoomInfo(string id);

        /// <summary>
        /// 请求离开房间
        /// </summary>
        /// <param name="id">用户id</param>
        void RequestLeaveRoom(string id);

        /// <summary>
        /// 请求解散房间
        /// </summary>
        /// <param name="id">用户id</param>
        void RequestDissolveRoom(string id);

        /// <summary>
        /// 请求战斗
        /// </summary>
        /// <param name="id">用户id</param>
        void RequestFight(string id);
        #endregion
    }
}