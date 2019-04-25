
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


using Framework.Event;
using Framework.Http;
using Framework.Network;
using Framework.Network.Web;
using Framework.Unity.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Demo2
{
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

        private ILoginModule loginModule = new WebLoginModule("http://truck.kmax-arvr.com/truck.php/port/Index/login");

        private IRoomModule roomModule;
        private INetworkConnect networkConnect = new WebConnect();

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
            PlayerManager.GetInstance().NetworkConnect = networkConnect;


            WebRoomModule.RoomUrl tempRoomUrl = new WebRoomModule.RoomUrl();
            tempRoomUrl.createRoomUrl = "http://truck.kmax-arvr.com/truck.php/port/Room/create_room";
            tempRoomUrl.joinRoomUrl = "http://truck.kmax-arvr.com/truck.php/port/Room/join_room";
            tempRoomUrl.getRoomListUrl = "http://truck.kmax-arvr.com/truck.php/port/Room/index";
            //tempRoomUrl.leaveRoomUrl = "http://truck.kmax-arvr.com/truck.php/port/Room/out_room";
            //tempRoomUrl.dissolveRoomUrl = "http://truck.kmax-arvr.com/truck.php/port/Room/dissolve_room";
            //tempRoomUrl.startFightUrl = "http://truck.kmax-arvr.com/truck.php/port/Room/start_fire";
            roomModule = new WebRoomModule(tempRoomUrl);

            PlayerManager.GetInstance().LoginModule = loginModule;
            PlayerManager.GetInstance().RoomModule = roomModule;
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
        private void OnLoginSuccess(ResponseBase varData)
        {
            UIEventArgs<string> tempArgs2 = new UIEventArgs<string>(varData.tips);
            UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempArgs2);

            id = PlayerManager.GetInstance().Player.Id;
            sign = PlayerManager.GetInstance().Sign;
        }

        private void OnLoginFail(ResponseBase varData)
        {
            UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
            tempData.Title = "提示";
            tempData.Content = varData.result;
            tempData.Style = UIMsgBox.Style.OKAndCancel;
            UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs2 = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
            UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs2);
        }

        private void ResponseRegist(string result)
        {
            Debug.Log(result);
        }

        private void OnCreateRoomSuccess(ResponseBase varData)
        {
            UIEventArgs<string> tempArgs2 = new UIEventArgs<string>(varData.tips);
            UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempArgs2);

            if (networkConnect.IsConnect())
            {
                RequestGetRoomInfo();
            }
            else
            {
                networkConnect.RemoveConnectListener(OnConnected);
                networkConnect.AddConnectListener(OnConnected);
                networkConnect.ConnectServer();
            }
        }

        private void OnCreateRoomFail(ResponseBase varData)
        {
            UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
            tempData.Title = "提示";
            tempData.Content = varData.tips;
            tempData.Style = UIMsgBox.Style.OKAndCancel;
            UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs2 = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
            UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs2);
        }

        private void OnJoinRoomSuccess(ResponseBase varData)
        {
            UIEventArgs<string> tempArgs2 = new UIEventArgs<string>(varData.tips);
            UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempArgs2);

            if (networkConnect.IsConnect())
            {
                RequestGetRoomInfo();
            }
            else
            {
                networkConnect.RemoveConnectListener(OnConnected);
                networkConnect.AddConnectListener(OnConnected);
                networkConnect.ConnectServer();
            }
        }

        private void OnJoinRoomFail(ResponseBase varData)
        {
            UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
            tempData.Title = "提示";
            tempData.Content = varData.tips;
            tempData.Style = UIMsgBox.Style.OKAndCancel;
            UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs2 = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
            UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs2);
        }

        private void OnRoomListSuccess(ResponseBase varData)
        {
            UIEventArgs<string> tempArgs2 = new UIEventArgs<string>(varData.tips);
            UIManager.GetInstance().ShowUI(UIPath.MsgTips, tempArgs2);
        }

        private void OnRoomListFail(ResponseBase varData)
        {
            
        }

        #region WebSocket服务器

        /// <summary>
        /// 当连上服务器时
        /// </summary>
        /// <param name="varData"></param>
        private void OnConnected(EventData varData)
        {
            networkConnect.RemoveConnectListener(OnConnected);
            RequestGetRoomInfo();
        }

        private void ResponseGetRoomInfo(ProtocolBase proto)
        {
            ProtocolJson tempJson = proto as ProtocolJson;
            SC_GetRoomInfo tempInfo = tempJson.Deserialize<SC_GetRoomInfo>();
            if (tempInfo != null)
            {
                Room tempRoom = PlayerManager.GetInstance().Room;

                foreach (SC_Player tempP in tempInfo.text)
                {
                    Player tempPlayer = tempRoom.GetPlayer(tempP.id.ToString());
                    if(tempPlayer == null)
                    {
                        tempPlayer = new Player(tempP.name, tempP.id.ToString());
                        tempPlayer.RoleId = tempP.role_id;
                        PlayerManager.GetInstance().Room.Enter(tempPlayer);
                        continue;
                    }
                    tempPlayer.Name = tempP.name;
                    tempPlayer.RoleId = tempP.role_id;
                }
            }
        }

        #endregion

        #endregion

        #region Protected & Public Methods
        [ContextMenu("登录")]
        public void Login()
        {
            loginModule.Account = account;
            loginModule.Password = "123456";

            loginModule.RequestLogin(OnLoginSuccess, OnLoginFail);
        }

        [ContextMenu("注册")]
        public void RequestRegist()
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

        [ContextMenu("创建房间")]
        public void RequestCreate()
        {
            roomModule.RequestCreate(OnCreateRoomSuccess, OnCreateRoomFail);
        }

        [ContextMenu("加入房间")]
        public void RequestJoin()
        {
            roomModule.RequestJoin(roomId, OnJoinRoomSuccess, OnJoinRoomFail);
        }

        [ContextMenu("房间列表")]
        public void RequestRoomList()
        {
            roomModule.RequestRoomList(OnRoomListSuccess, OnRoomListFail);
        }

        [ContextMenu("获取房间信息")]
        public void RequestGetRoomInfo()
        {
            networkConnect.MessageHandle.AddOnceListener(ProtocolConst.GetRoomInfo, ResponseGetRoomInfo);
            roomModule.RequestGetRoomInfo();
        }

        [ContextMenu("离开房间")]
        public void RequestLeaveRoom()
        {
            roomModule.RequestLeaveRoom();
        }

        [ContextMenu("解散房间")]
        public void RequestDissolveRoom()
        {
            roomModule.RequestDissolveRoom();
        }

        [ContextMenu("战斗")]
        public void RequestFight()
        {
            roomModule.RequestFight();
        }

        public void ChangeAccount()
        {
            account = mAccountInput.text;
        }

        public void ChangeRoomId()
        {
            roomId = mRoomIdInput.text;
        }

        public InputField mAccountInput;
        public InputField mRoomIdInput;
        #endregion
    }
}