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
        /// <param name="onSuccess">成功回调</param>
        /// <param name="onFail">失败回调</param>
        void RequestCreate(Action<ResponseBase> onSuccess, Action<ResponseBase> onFail);

        /// <summary>
        /// 请求加入房间
        /// </summary>
        /// <param name="onSuccess">成功回调</param>
        /// <param name="onFail">失败回调</param>
        void RequestJoin(string roomId, Action<ResponseBase> onSuccess, Action<ResponseBase> onFail);

        /// <summary>
        /// 请求查看房间列表
        /// </summary>
        /// <param name="onSuccess">成功回调</param>
        /// <param name="onFail">失败回调</param>
        void RequestRoomList(Action<ResponseBase> onSuccess, Action<ResponseBase> onFail);

        /// <summary>
        /// 请求获取房间信息
        /// </summary>
        /// <param name="onFail">失败回调</param>
        void RequestGetRoomInfo(Action<ResponseBase> onFail);

        /// <summary>
        /// 请求离开房间
        /// </summary>
        /// <param name="onFail">失败回调</param>
        void RequestLeaveRoom(Action<ResponseBase> onFail);

        /// <summary>
        /// 请求解散房间
        /// </summary>
        /// <param name="onFail">失败回调</param>
        void RequestDissolveRoom(Action<ResponseBase> onFail);

        /// <summary>
        /// 请求战斗
        /// </summary>
        /// <param name="onFail">失败回调</param>
        void RequestFight(Action<ResponseBase> onFail);
        #endregion
    }
}