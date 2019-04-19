
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


using Framework.Unity.Tools;
using Framework.Unity.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        #endregion

        #region Private Methods

        [ContextMenu("Load")]
        private void Load()
        {
            UIManager.GetInstance().ShowUI(UIPath.Login, null);
            
            UIMsgBox.UIMsgBoxArgs tempArgs = new UIMsgBox.UIMsgBoxArgs();
            tempArgs.Style = UIMsgBox.Style.OKAndCancel;
            tempArgs.Title = "提示";
            tempArgs.Content = "当前场景是"+SceneManager.GetActiveScene().name;
            tempArgs.CloseAction = CloseMsgBox;
            UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempU = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempArgs);
            UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempU);
            
        }
        private void CloseMsgBox(UIMsgBox.Result varResult)
        {
            if (varResult == UIMsgBox.Result.OK)
            {
                string tempTips = "你点击了OK按钮";
                Debug.Log(tempTips);

                UIEventArgs<string> tempU2 = new UIEventArgs<string>(tempTips);
                UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempU2);

                TimeManager.Instance.AddTask(1.5f, false, JumpScene1);
            }
            else if(varResult == UIMsgBox.Result.Cancel)
            {
                string tempTips = "你点击了Cancle按钮";
                Debug.Log(tempTips);

                UIEventArgs<string> tempU2 = new UIEventArgs<string>(tempTips);
                UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempU2);

                TimeManager.Instance.AddTask(1.5f, false, JumpScene2);
            }
        }
        #endregion

        #region Protected & Public Methods
        public void JumpScene1()
        {
            UIManager.GetInstance().LoadScene("Scene1");
        }

        public void JumpScene2()
        {
            UIManager.GetInstance().LoadScene("Scene2");
        }
        #endregion
    }
}