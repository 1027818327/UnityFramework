
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2018/11/16 09:52:40
 *  
 */
#endregion

using AssetBundles;
using Framework.Pattern;
using Framework.Unity.Tools;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Unity.AssetBundles
{
    public struct LoadTask
    {
        public string mAssetBundleName;
        public string mAssetName;
        public System.Type mType;
        public AssetBundleLoadOperation mAbLoadOperation;
        public System.Action<AssetBundleLoadOperation> mLoadFinish;
    }

    public class LoadQueue : Singleton<LoadQueue>
    {
        #region Fields
        List<LoadTask> mLoadTasks = new List<LoadTask>();
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
        protected IEnumerator Update()
        {
            while (true)
            {
                while (mLoadTasks.Count > 0)
                {
                    var tempLoadTask = mLoadTasks[0];

                    if (tempLoadTask.mAbLoadOperation == null && tempLoadTask.mAssetBundleName != null)
                    {
                        tempLoadTask.mAbLoadOperation = AssetBundleManager.LoadAssetAsync(tempLoadTask.mAssetBundleName, tempLoadTask.mAssetName, tempLoadTask.mType);
                    }
                    yield return tempLoadTask.mAbLoadOperation;
                    if (tempLoadTask.mLoadFinish != null)
                    {
                        tempLoadTask.mLoadFinish.Invoke(tempLoadTask.mAbLoadOperation);
                    }
                    lock (mLoadTasks)
                    {
                        mLoadTasks.RemoveAt(0);
                    }
                }
                yield return null;
            }
        }

        protected void AddManifestTask(System.Action<AssetBundleLoadOperation> loadFinish = null)
        {
            LoadTask tempLt = new LoadTask
            {
                mLoadFinish = loadFinish,
                mAbLoadOperation = AssetBundleManager.Initialize()
            };
            AddTask(tempLt);
        }

        public override void Init()
        {
            MonoHelper.StartCoroutines(Update());
            //AddManifestTask(null);
        }

        public void AddTask(string assetBundleName, string assetName, System.Type type, System.Action<AssetBundleLoadOperation> loadFinish = null)
        {
            LoadTask tempLt = new LoadTask
            {
                mAssetBundleName = assetBundleName,
                mAssetName = assetName,
                mType = type,
                mLoadFinish = loadFinish,
            };
            AddTask(tempLt);
        }

        public void AddTask(LoadTask varTask)
        {
            lock (mLoadTasks)
            {
                mLoadTasks.Add(varTask);
            }
        }

        public void AddSceneTask(string assetBundleName, string assetName, bool isAdditive, System.Action<AssetBundleLoadOperation> loadFinish = null)
        {
            LoadTask tempLt = new LoadTask
            {
                mAssetBundleName = assetBundleName,
                mAssetName = assetName,
                mLoadFinish = loadFinish,
            };
            tempLt.mAbLoadOperation = AssetBundleManager.LoadLevelAsync(assetBundleName, assetName, isAdditive);
            AddTask(tempLt);
        }

        public void RemoveTask(LoadTask varTask)
        {
            lock (mLoadTasks)
            {
                mLoadTasks.Remove(varTask);
            }
        }

        /// <summary>
        /// 移除所有任务
        /// </summary>

        public void ClearAllTask()
        {
            lock (mLoadTasks)
            {
                mLoadTasks.Clear();
            }
        }
        #endregion
    }
}