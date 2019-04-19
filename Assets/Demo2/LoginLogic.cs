
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


using Framework.Network.Web;
using Framework.Unity.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Demo2
{
    public class LoginLogic : MonoBehaviour
    {
        #region Fields
        private IWebLoginModule loginModule = new WebLoginModule();
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
            string tempUrl = "http://truck.kmax-arvr.com/truck.php/port/Index/login";
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            tempDic.Add("account", "123");
            tempDic.Add("password", "123");
            loginModule.RequestLogin(tempUrl, tempDic, ResponseLogin);
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
        private void ResponseLogin(string result)
        {
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