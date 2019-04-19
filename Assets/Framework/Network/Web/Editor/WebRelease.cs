
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 * 
 *        创建者：  陈伟超
 *      创建时间：  2018/10/13 11:20:39
 *  
 */
#endregion


using UnityEditor;
using UnityEngine;

namespace Framework.Network
{
    public class WebRelease
    {
        #region Fields
        [MenuItem("Tools/Web/Close Connection")]
        static void CloseConnection()
        {
            WebMgr.SrvConn.Close();
        }

        #endregion

        #region Properties

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods

        #endregion
    }
}