
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/10 22:18:29
 *  
 */
#endregion


using System;
using Framework.Unity.UI;

namespace Assets.Demo
{
    public class Login : UIBase
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
        public override void InitOpenArgs(EventArgs varArgs)
        {
            
        }

        public override void Freeze()
        {
            gameObject.SetActive(false);
        }

        public void OpenXueXun()
        {
            //UIManager.GetInstance().DestroyUI(UIFormPath);
            UIManager.GetInstance().ShowUI(UIPath.XueXun, null);
        }

        public void OpenJiaoXue()
        {
            //UIManager.GetInstance().DestroyUI(UIFormPath);
            UIManager.GetInstance().ShowUI(UIPath.JiaoXue, null);
        }

        public void OpenChangePassword()
        {
            UIManager.GetInstance().ShowUI(UIPath.ChangePassword, null);
        }
        #endregion

    }
}