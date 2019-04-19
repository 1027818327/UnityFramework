
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/01/09 11:35:50
 *  
 */
#endregion


using UnityEngine;
using zSpace.Core;
using zSpace.SimsCommon;

namespace ZSpaceEx
{
    /// <summary>
    /// 将此脚本挂在zCore物体上
    /// </summary>
    public class GraphicAdapter : MonoBehaviour
    {
        #region Fields
        public GameObject[] mCtrlObjs;
        #endregion

        #region Properties

        #endregion

        #region Unity Messages
        //    void Awake()
        //    {
        //
        //    }
        //    void OnEnable()
        //    {
        //
        //    }
        //
        void Start()
        {
            WorldAutosizer tempWa = GetComponent<WorldAutosizer>();
            if (tempWa == null)
            {
                tempWa = gameObject.AddComponent<WorldAutosizer>();
            }
            ZCore tempZCore = GetComponent<ZCore>();
            if (tempZCore != null)
            {
                tempWa.DesignTimeZSpaceHardware = tempZCore.GetDisplayTypeByScale();
            }
            OpenObjs();
        }
        //    
        //    void Update() 
        //    {
        //    
        //    }
        //
        //    void OnDisable()
        //    {
        //
        //    }
        //
        //    void OnDestroy()
        //    {
        //
        //    }

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods
        [ContextMenu("Close Objs")]
        protected void CloseObjs()
        {
            if (mCtrlObjs != null)
            {
                foreach (GameObject tempObj in mCtrlObjs)
                {
                    if (tempObj != null)
                        tempObj.SetActive(false);
                }
            }
        }

        [ContextMenu("Open Objs")]
        protected void OpenObjs()
        {
            if (mCtrlObjs != null)
            {
                foreach (GameObject tempObj in mCtrlObjs)
                {
                    if (tempObj != null)
                        tempObj.SetActive(true);
                }
            }
        }
        #endregion
    }
}