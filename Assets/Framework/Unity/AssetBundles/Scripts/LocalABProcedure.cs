
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/05/17 13:47:44
 *  
 */
#endregion


using AssetBundles;
using Framework.Unity.Procedure;
using UnityEngine;

namespace Framework.Unity.AssetBundles
{
    /// <summary>
    /// 本地AB流程
    /// </summary>
    public class LocalABProcedure : AbstractLoadProcedure
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
        protected void LoadMainfestFinish(AssetBundleLoadAssetOperation operation)
        {
            Debug.Log("本地AssetBundle流程结束");
            ProcedureEnd();
        }

        public override void ProcedureBegin()
        {
            string tempUrl = System.IO.Path.Combine(System.Environment.CurrentDirectory, Utility.AssetBundlesOutputPath);
            AssetBundleManager.SetSourceAssetBundleURL(tempUrl);

            LoadTask tempLt = new LoadTask
            {
                mLoadFinish = LoadMainfestFinish,
                mAbLoadAssetOperation = AssetBundleManager.Initialize()
            };
            LoadQueue.GetInstance().AddTask(tempLt);
        }

        #endregion
    }
}