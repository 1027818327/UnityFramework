
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/06/21 09:05:55
 *  
 */
#endregion


using AssetBundles;
using Framework.Unity.Procedure;

namespace Framework.Unity.AssetBundles
{
    public class LoadAbSceneProcedure : AbstractLoadProcedure
    {
        #region Fields
        public string sceneAssetBundle;
        public string sceneName;
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
        private void LoadFinish(AssetBundleLoadOperation varO)
        {
            ProcedureEnd();
        }
        #endregion

        #region Protected & Public Methods
        public override void ProcedureBegin()
        {
            LoadQueue.GetInstance().AddSceneTask(sceneAssetBundle, sceneName, false, LoadFinish);
        }

        public void LoadScene()
        {

        }

        #endregion
    }
}