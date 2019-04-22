#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/19 15:31:34
 *  
 */
#endregion

using Framework.Http;
using System;
using System.Collections.Generic;

namespace Framework.Network.Web
{
    public class WebLoginModule : ILoginModule
    {
        #region Fields
        private HttpUtils mHttp;
        private string url;
        private string account;
        private string password;
        #endregion

        #region Properties
        public string Account
        {
            get
            {
                return account;
            }

            set
            {
                account = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods
        public WebLoginModule(string url)
        {
            mHttp = new HttpUtils();
            this.url = url;
        }

        public void RequestLogin(Action<ResponseBase> onSuccess, Action<ResponseBase> onFail)
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            tempDic.Add("account", account);
            tempDic.Add("password", password);

            Action<string> tempA = delegate (string result)
            {
                SimpleJSON.JSONNode tempNode = SimpleJSON.JSON.Parse(result);
                string tempS = tempNode["success"];
                bool tempB = Convert.ToBoolean(tempS);

                if (tempB)
                {
                    string selfId = tempNode["user"]["id"];
                    string selfName = tempNode["user"]["name"];
                    string tempRoleId = tempNode["user"]["role_id"];
                    string sign = tempNode["user"]["sign"];

                    Player tempSelf = new Player(selfName, selfId);
                    tempSelf.RoleId = Convert.ToInt32(tempRoleId);

                    PlayerManager.GetInstance().Player = tempSelf;
                    PlayerManager.GetInstance().Sign = sign;

                    string roomId = tempNode["room"]["id"];
                    int tempRoomIdInt = Convert.ToInt32(roomId);
                    if (tempRoomIdInt == 0)
                    {
                        PlayerManager.GetInstance().Room = null;
                    }
                    else
                    {
                        string tempMasterId = tempNode["room"]["user_id"];

                        PlayerManager.GetInstance().Room = new Room(tempRoomIdInt);
                        SimpleJSON.JSONArray tempRoomArray = tempNode["room"]["room_info"] as SimpleJSON.JSONArray;

                        foreach (SimpleJSON.JSONNode element in tempRoomArray)
                        {
                            string userId = element["user_id"];
                            string userName = element["name"];
                            string roleId = element["role_id"];
                            Player tempP = new Player(userId, userName);
                            tempP.RoleId = Convert.ToInt32(roleId);
                            if (userId.Equals(tempMasterId))
                            {
                                tempP.RoomIdentity = RoomIdentity.RoomMaster;
                            }
                            PlayerManager.GetInstance().Room.Enter(tempP);
                        }
                    }
                }

                if (tempB)
                {
                    if (onSuccess != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.result = result;
                        onSuccess.Invoke(rb);
                    }
                }
                else
                {
                    if (onFail != null)
                    {
                        ResponseBase rb = new ResponseBase();
                        rb.result = tempNode["text"];
                        onFail.Invoke(rb);
                    }
                }
            };

            bool tempConnect = mHttp.SendPostAnsyc(url, tempDic, tempA);
            if (!tempConnect)
            {
                if (onFail != null)
                {
                    ResponseBase rb = new ResponseBase();
                    rb.result = "网络异常";
                    onFail.Invoke(rb);
                }
            }
        }
        #endregion
    }
}
