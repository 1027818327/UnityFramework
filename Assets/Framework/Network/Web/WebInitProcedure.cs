
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/18 11:29:19
 *  
 */
#endregion


using Framework.Unity.Procedure;
using Framework.Unity.Tools;
using UnityEngine;

namespace Framework.Network.Web
{
    /// <summary>
    /// 网络初始化流程
    /// </summary>
    public class WebInitProcedure : AbstractLoadProcedure
    {
        #region Fields
        private static WebInitProcedure mInstance;
        #endregion

        #region Properties

        #endregion

        #region Unity Messages
        //void Awake()
        //{

        //}
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

        private void OnApplicationQuit()
        {
            WebMgr.SrvConn.StopReconnect();
        }

        #endregion

        #region Private Methods
        private void NetworkUpdate()
        {
            WebMgr.Update();
        }
        #endregion

        #region Protected & Public Methods
        public override void ProcedureBegin()
        {
            if (mInstance == null)
            {
                mInstance = this;
            }
            else
            {
                if (mInstance.gameObject != gameObject)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            DontDestroyOnLoad(gameObject);

            Application.runInBackground = true;
            WebConfig.ReadConfig();
            MonoHelper.AddUpdateListener(NetworkUpdate);

            TimeManager.Instance.AddOneFrameTask(false, ProcedureEnd);
        }
        #endregion
    }
}