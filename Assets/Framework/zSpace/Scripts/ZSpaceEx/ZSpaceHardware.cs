#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/03/29 13:44:15
 *  
 */
#endregion


using UnityEngine;

namespace zSpace.Core
{
    public partial class ZCore : MonoBehaviour
    {
        /// <summary>
        /// 获取设备序列号
        /// </summary>
        /// <returns></returns>
        public string GetSerialNumber()
        {
            return GetDisplayAttributeString(DisplayAttribute.SerialNumber);
        }
    }
}
