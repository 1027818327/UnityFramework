
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/19 17:53:52
 *  
 */
#endregion


using Framework.Network;
using Framework.Network.Web;
using Framework.Unity.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Demo2
{
    public class LoginLogic : MonoBehaviour
    {
        #region Fields
        private ILoginModule loginModule = new WebLoginModule("http://truck.kmax-arvr.com/truck.php/port/Index/login", "123", "123");
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
            bool tempConnect = loginModule.RequestLogin(ResponseLogin);
            if (!tempConnect)
            {
                UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
                tempData.Title = "提示";
                tempData.Content = "网络异常,请检查你的网络";
                UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
                UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs);
            }
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
        private void ResponseLogin(EventArgs args)
        {
            NetworkEventArgs<string> tempArgs = args as NetworkEventArgs<string>;
            string result = tempArgs.Data;
            Debug.Log(result);
            SimpleJSON.JSONNode tempNode = SimpleJSON.JSON.Parse(result);
            string tempTips = tempNode["text"];
            if (tempNode["success"] == "false")
            {
                UIEventArgs<string> tempU2 = new UIEventArgs<string>(tempTips);
                UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempU2);
            }
            else
            {
                UIEventArgs<string> tempU2 = new UIEventArgs<string>(tempTips);
                UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempU2);
            }
        }
        #endregion

        #region Protected & Public Methods

        #endregion
    }
}