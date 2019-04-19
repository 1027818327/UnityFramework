
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
    public interface IWebLoginModule
    {
        #region Properties

        #endregion

        #region Protected & Public Methods
        /// <summary>
        /// 请求登录
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="response"></param>
        void RequestLogin(string url, IDictionary<string, string> parameters, Action<string> response);

        #endregion
    }
}