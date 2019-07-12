#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/07/12 11:00:37
 *  
 */
#endregion


using System;

namespace Framework.Network
{
    public interface IReconnectModule
    {
        #region Properties
        Action<object> RestoreAction { get; set; }
        #endregion

        #region Protected & Public Methods
        void RequestRestore();
        #endregion
    }
}