
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/01/04 13:44:15
 *  
 */
#endregion


using UnityEngine;
using zSpace.SimsCommon;

namespace zSpace.Core
{
    public partial class ZCore : MonoBehaviour
    {
        #region Properties
        public Vector2 ViewportDisplayCenter
        {

            get { return _viewportDisplayCenter; }
        }

        public Vector2 ViewportSizeInMeters
        {
            get { return _viewportSizeInMeters; }
        }

        #endregion

        #region Protected & Public Methods
        public DisplayScaleUtility.DisplayType GetDisplayTypeByScale()
        {
            Vector2 tempV = GetDisplaySize();

            float tempVolatility = 0.09f; // 波动值
            Vector2 tempAllInOne = new Vector2(0.521f, 0.293f);  // zSpace一体机物理尺寸参数
            Vector2 tempLapTop = new Vector2(0.344f, 0.193f);  // zSpace笔记本物理尺寸参数

            if (tempAllInOne.x - tempV.x <= tempVolatility && tempV.y - tempAllInOne.y <= tempVolatility)
            {
                return DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9;
            }
            else if (tempLapTop.x - tempV.x <= tempVolatility && tempV.y - tempAllInOne.y <= tempVolatility)
            {
                return DisplayScaleUtility.DisplayType.Size15Inch_Aspect16x9;
            }
            return DisplayScaleUtility.DisplayType.Unknown;
        }
        #endregion
    }
}