#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 * 
 *        创建者：  陈伟超
 *      创建时间：  2018/9/28 10:21:24
 *  
 */
#endregion



using Framework.Debugger;
using System;
using System.IO;
using UnityEngine;

namespace Framework.Network.Web
{
    public class WebConfig
    {
        #region Fields
        #region 地址
        public const string ServerAddress = "ws://47.105.101.118:3389";
        public const string LocalAddress = "ws://172.16.12.244:8181";
        public static string ConnectAddress = ServerAddress;
        #endregion

        #region 网络事件常量
        public const string WebSocketOpen = "WebSocket Open";
        #endregion

        #endregion

        #region Properties

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods
        public static void ReadConfig()
        {
            string tempPath = string.Format("{0}/{1}", Application.streamingAssetsPath, "WebConfig.txt");
            try
            {
                string tempAddress = File.ReadAllText(tempPath);
                if (string.IsNullOrEmpty(tempAddress) == false && tempAddress.StartsWith("ws://"))
                {
                    ConnectAddress = tempAddress;
                    Debuger.Log(string.Format("连接的地址是{0}", ConnectAddress));
                }
            }
            catch (Exception e)
            {
                Debuger.LogError(e.Message);
            }
        }
        #endregion
    }
}