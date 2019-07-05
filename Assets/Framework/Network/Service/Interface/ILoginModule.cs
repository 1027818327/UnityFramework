﻿#region 版权信息
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
    public interface ILoginModule
    {
        #region Properties
        string Account { get; set; }

        string Password { get; set; }

        #endregion

        #region Protected & Public Methods
        /// <summary>
        /// 请求登录
        /// </summary>
        /// <param name="onSuccess">成功回调</param>
        /// <param name="onFail">失败回调</param>
        void RequestLogin(Action<ResponseBase> onSuccess, Action<ResponseBase> onFail);

        #endregion
    }
}