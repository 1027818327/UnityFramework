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
    public class WebLoginModule : ILoginModule
    {
        #region Fields
        private HttpUtils mHttp;
        private string url;
        private string account;
        private string password;
        #endregion

        #region Properties
        public string Account
        {
            get
            {
                return account;
            }

            set
            {
                account = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods
        public WebLoginModule(string url, string account, string password)
        {
            mHttp = new HttpUtils();
            this.url = url;
            Account = account;
            Password = password;
        }

        public bool RequestLogin(Action<EventArgs> response)
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            tempDic.Add("account", account);
            tempDic.Add("password", password);

            if (response == null)
            {
                return mHttp.SendPostAnsyc(url, tempDic, null);
            }
            else
            {
                Action<string> tempA = delegate (string result)
                {
                    NetworkEventArgs<string> tempArgs = new NetworkEventArgs<string>(result);
                    response.Invoke(tempArgs);
                };

                return mHttp.SendPostAnsyc(url, tempDic, tempA);
            }
        }
        #endregion
    }
}
