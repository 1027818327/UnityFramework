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


namespace Framework.Network.Web
{
    public class WebConnect : IWebConnect
    {
        public void ConnectServer()
        {
            WebMgr.SrvConn.Connect(WebConfig.ConnectAddress);
        }

        public void DisconnectServer()
        {
            WebMgr.SrvConn.Close();
        }
    }
}
