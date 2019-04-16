
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/10 23:28:06
 *  
 */
#endregion


using Framework.Unity.UI;
using UnityEngine;

namespace Assets.Demo
{
    public class Test2 : MonoBehaviour
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
        void Start()
        {
            //Invoke("Load", 1f);
            Load();
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

        [ContextMenu("Load")]
        private void Load()
        {
            UIManager.GetInstance().ShowUI(UIPath.Login, null);
            UIMsgBox.UIMsgBoxArgs tempArgs = new UIMsgBox.UIMsgBoxArgs();
            tempArgs.Style = UIMsgBox.Style.OKAndCancel;
            tempArgs.Title = "提示";
            tempArgs.Content = "你好啊";
            UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempU = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempArgs);
            UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempU);

            UIEventArgs<string> tempU2 = new UIEventArgs<string>("欢迎光临");
            UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempU2);
        }

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods

        #endregion
    }
}