
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


using Framework.Http;
using Framework.Network;
using Framework.Network.Web;
using Framework.Unity.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Demo2
{
    public class JsonLogin
    {
        public bool success;
        public string text;
        public string id;
        public string sign;
        public string role_id;
    }

    public class LoginLogic : MonoBehaviour
    {
        #region Fields
        public string id;
        public string sign;

        /// <summary>
        /// 工号
        /// </summary>
        public string account;
        /// <summary>
        /// 名字
        /// </summary>
        public string mName;

        public string phone;

        /// <summary>
        /// 房间Id
        /// </summary>
        public string roomId;

        private ILoginModule loginModule = new WebLoginModule("http://truck.kmax-arvr.com/truck.php/port/Index/login", "truck", "123456");

        private IRoomModule roomModule;
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
        [ContextMenu("登录")]
        private void Login()
        {
            ((WebLoginModule)loginModule).Account = account;

            bool tempConnect = loginModule.RequestLogin(ResponseLogin);
            if (!tempConnect)
            {
                UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
                tempData.Title = "提示";
                tempData.Content = "网络异常,请检查你的网络";
                tempData.Style = UIMsgBox.Style.OK;
                UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
                UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs);
            }
        }

        private void ResponseLogin(EventArgs args)
        {
            NetworkEventArgs<string> tempArgs = args as NetworkEventArgs<string>;
            string result = tempArgs.Data;
            Debug.Log(result);

            SimpleJSON.JSONNode tempNode = SimpleJSON.JSON.Parse(result);
            string tempS = tempNode["success"];
            bool tempB = Convert.ToBoolean(tempS);

            string tempTips = tempNode["text"];
            if (!tempB)
            {
                UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
                tempData.Title = "提示";
                tempData.Content = tempTips;
                tempData.Style = UIMsgBox.Style.OKAndCancel;
                UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs2 = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
                UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs2);
            }
            else
            {
                UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
                tempData.Title = "提示";
                tempData.Content = tempTips;
                tempData.Style = UIMsgBox.Style.OKAndCancel;
                UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs2 = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
                UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs2);

                id = tempNode["id"];
                sign = tempNode["sign"];

                roomModule = new WebRoomModule("http://truck.kmax-arvr.com/truck.php/port/Room/create_room",
            "http://truck.kmax-arvr.com/truck.php/port/Room/join_room",
            "http://truck.kmax-arvr.com/truck.php/port/Room/index");
            }
        }

        [ContextMenu("注册")]
        private void RequestReigst()
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            tempDic.Add("user_id", id);
            tempDic.Add("sign", sign);
            tempDic.Add("account", account);
            tempDic.Add("name", mName);
            tempDic.Add("age", "20");
            tempDic.Add("sex", "0");
            tempDic.Add("class_name", "1");
            tempDic.Add("phone", phone);

            HttpUtils tempUtils = new HttpUtils();
            tempUtils.SendPostAnsyc("http://truck.kmax-arvr.com/truck.php/port/User/add_student", tempDic, ResponseRegist);
        }

        private void ResponseRegist(string result)
        {
            Debug.Log(result);
        }

        [ContextMenu("创建房间")]
        private void RequestCreate()
        {
            roomModule.RequestCreate(ResponseCreate);
        }

        [ContextMenu("加入房间")]
        private void RequestJoin()
        {
            roomModule.RequestJoin(roomId, ResponseJoin);
        }

        [ContextMenu("房间列表")]
        private void RequestRoomList()
        {
            roomModule.RequestRoomList(ResponseRoomList);
        }

        private void ResponseCreate(EventArgs args)
        {
            NetworkEventArgs<string> tempArgs = args as NetworkEventArgs<string>;
            string result = tempArgs.Data;
            Debug.Log(result);
        }

        private void ResponseJoin(EventArgs args)
        {
            NetworkEventArgs<string> tempArgs = args as NetworkEventArgs<string>;
            string result = tempArgs.Data;
            Debug.Log(result);
        }

        private void ResponseRoomList(EventArgs args)
        {
            NetworkEventArgs<string> tempArgs = args as NetworkEventArgs<string>;
            string result = tempArgs.Data;
            Debug.Log(result);
        }

        [ContextMenu("TestConnect")]
        private void TestConnect()
        {
            WebMgr.SrvConn.Connect(WebConfig.ConnectAddress);
        }

        #endregion

        #region Protected & Public Methods

        #endregion
    }
}