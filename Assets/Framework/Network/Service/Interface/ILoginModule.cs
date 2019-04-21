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
    public interface ILoginModule
    {
        #region Properties

        #endregion

        #region Protected & Public Methods
        /// <summary>
        /// 请求登录
        /// </summary>
        /// <param name="response">响应</param>
        /// <returns>false表示网络异常</returns>
        bool RequestLogin(Action<EventArgs> response);

        #endregion
    }
}