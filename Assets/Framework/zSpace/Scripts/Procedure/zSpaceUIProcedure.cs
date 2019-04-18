
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/18 17:56:06
 *  
 */
#endregion


using Framework.Unity.Procedure;
using Framework.Unity.UI;
using UnityEngine;

namespace Framework.zSpace.Procedure
{
    public class zSpaceUIProcedure : AbstractLoadProcedure
    {
        #region Fields

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
        //    void Start() 
        //    {
        //    
        //    }
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
        public override void ProcedureBegin()
        {
            UIConfig tempUIConfig = UIConfig.GetInstance();
            tempUIConfig.DestroyOnLoad = true;
            tempUIConfig.RootHasCanvas = false;
            tempUIConfig.ShowMask = true;
            ProcedureEnd();
        }
        #endregion
    }
}