#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/07/12 10:48:58
 *  
 */
#endregion


namespace Framework.Network.Web
{
    public interface IReconnectStrategy
    {
        #region Properties

        #endregion

        #region Protected & Public Methods
        void HandleDisconnected(string reason);
        #endregion
    }
}