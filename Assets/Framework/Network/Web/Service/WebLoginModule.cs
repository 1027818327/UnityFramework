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

using Framework.Http;
using System;
using System.Collections.Generic;

namespace Framework.Network.Web
{
    public class WebLoginModule : IWebLoginModule
    {
        private HttpUtils mHttp;

        public WebLoginModule()
        {
            mHttp = new HttpUtils();
        }

        public void RequestLogin(string url, IDictionary<string, string> parameters, Action<string> response)
        {
            mHttp.SendPostAnsyc(url, parameters, response);
        }
    }
}
