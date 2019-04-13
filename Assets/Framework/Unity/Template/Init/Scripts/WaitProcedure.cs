
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/10 14:35:55
 *  
 */
#endregion


using Framework.Procedure;
using Framework.Unity.Tools;

namespace Framework.Unity
{
    public class WaitProcedure : AbstractLoadProcedure
    {
        #region Fields
        public float mWaitTime = 1f;
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
        /// <summary>
        /// 流程开始
        /// </summary>
        public override void ProcedureBegin()
        {
            TimeManager.Instance.AddTask(mWaitTime, false, ProcedureEnd);
        }
        #endregion
    }
}