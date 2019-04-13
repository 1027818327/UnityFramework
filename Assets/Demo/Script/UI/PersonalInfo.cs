
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 * 
 *        创建者：  
 *      创建时间：  2019/4/10 22:40:31
 *  
 */
#endregion


using Framework.Unity.UI;
using System;

namespace Assets.Demo
{
    public class PersonalInfo : UIBase
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

        public void Close()
        {
            UIManager.GetInstance().CloseUI(UIFormPath);
        }

        public override void SetupConfig()
        {
            
        }
        #endregion

    }
}