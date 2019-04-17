
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/01/03 18:04:03
 *  
 */
#endregion


using Framework.Registry;
using Microsoft.Win32;

namespace ZSpaceEx
{
    public class Computer
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods
        public static bool IsZspace()
        {
            var tempRk = RegistryUtils.GetRegistryKey(Registry.LocalMachine, "SOFTWARE/zSpace/Runtime");
            return tempRk != null;
        }
        #endregion
    }
}