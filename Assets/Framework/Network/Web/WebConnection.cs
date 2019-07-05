using BestHTTP;
using BestHTTP.WebSocket;
using Framework.Debugger;
using Framework.Unity.UI;
using System;
using System.Timers;

namespace Framework.Network.Web
{
    //网络链接
    public class WebConnection
    {
        /// <summary>
        /// Saved WebSocket instance
        /// </summary>
        WebSocket webSocket;

        private string mLastAddress;

        #region 重连变量
        private Timer mReconnectTimer;
        /// <summary>
        /// 是否需要重连
        /// </summary>
        private bool mNeedReconnect;

        /// <summary>
        /// 重连间隔时间，单位是毫秒
        /// </summary>
        private const double ReconnectInterval = 1000;

        /// <summary>
        /// 参数表示是否之前异常断开，然后连接上
        /// </summary>
        public Action<bool> mConnectAction;

        /// <summary>
        /// 参数表示网络异常断开的原因
        /// </summary>
        public Action<string> mDisconnectAction;

        #endregion

        //消息分发
        public MsgDistribution msgDist = new MsgDistribution();

        ///状态
        public enum Status
        {
            /// <summary>
            /// 未连接
            /// </summary>
            None,
            /// <summary>
            /// 已连接
            /// </summary>
            Connected,
        };

        public bool Connect(string address)
        {
            mLastAddress = address;

            if (webSocket != null && webSocket.IsOpen)
            {
                AutoClose();
            }

            // Create the WebSocket instance
            webSocket = new WebSocket(new Uri(address));

#if !BESTHTTP_DISABLE_PROXY && !UNITY_WEBGL
            if (HTTPManager.Proxy != null)
                webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
#endif

            // Subscribe to the WS events
            webSocket.OnOpen += OnOpen;
            webSocket.OnMessage += OnMessageReceived;
            webSocket.OnClosed += OnClosed;
            webSocket.OnError += OnError;

            // Start connecting to the server
            webSocket.Open();

            Debuger.Log("Opening Web Socket...");

            StopReconnect();

            return true;
        }

        public void Close()
        {
            mNeedReconnect = false;
        }

        protected void AutoClose()
        {
            if (webSocket != null)
            {
                webSocket.Close();
            }
        }

        public Status GetStatus()
        {
            if (webSocket == null || !webSocket.IsOpen)
            {
                return Status.None;
            }

            return Status.Connected;
        }

        public void Update()
        {
            //消息对应业务逻辑处理
            msgDist.Update();
        }

        public void Send(string str)
        {
            if (GetStatus() == Status.Connected)
            {
                webSocket.Send(str);
            }
        }

        public bool Send(ProtocolBase protocol)
        {
            if (GetStatus() != Status.Connected)
            {
                Debuger.LogError("[Connection]还没连接就发送数据是不好的");
                return true;
            }

            Send(protocol.GetDesc());
            //Debuger.Log(string.Format("发送消息 {0}", protocol.GetDesc()));
            return true;
        }

        public bool Send(ProtocolBase protocol, string cbName, MsgDistribution.Delegate cb)
        {
            if (GetStatus() != Status.Connected)
                return false;
            if (cb != null)
            {
                msgDist.AddOnceListener(cbName, cb);
            }
            
            return Send(protocol);
        }

        public bool Send(ProtocolBase protocol, MsgDistribution.Delegate cb)
        {
            string cbName = protocol.GetName();
            return Send(protocol, cbName, cb);
        }


        #region WebSocket Event Handlers

        /// <summary>
        /// Called when the web socket is open, and we are ready to send and receive data
        /// </summary>
        void OnOpen(WebSocket ws)
        {
            Debuger.Log("WebSocket Open");
            if (mConnectAction != null)
            {
                mConnectAction(mNeedReconnect);
            }
            mNeedReconnect = false;
        }

        /// <summary>
        /// Called when we received a text message from the server
        /// </summary>
        void OnMessageReceived(WebSocket ws, string message)
        {
            Debuger.Log(string.Format("Message received: {0}", message));
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            lock (msgDist.msgList)
            {
                ProtocolJson protocol = new ProtocolJson();
                protocol.Deserialize(message);
                if (protocol.JsonData != null)
                {
                    msgDist.msgList.Add(protocol);
                }
            }
        }

        /// <summary>
        /// Called when the web socket closed
        /// </summary>
        void OnClosed(WebSocket ws, UInt16 code, string message)
        {
            Debuger.Log(string.Format("WebSocket closed! Code: {0} Message: {1}", code, message));
            webSocket = null;
        }

        /// <summary>
        /// Called when an error occured on client side
        /// </summary>
        void OnError(WebSocket ws, Exception ex)
        {
            string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
            if (ws.InternalRequest.Response != null)
                errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
#endif

            Debuger.LogError(string.Format("An error occured: {0}", (ex != null ? ex.Message : "Unknown Error " + errorMsg)));
            AutoClose();
            webSocket = null;
            if (ex != null)
            {
                mNeedReconnect = true;

                if (ex.Message.StartsWith("向一个无法连接的网络尝试了一个套接字操作"))
                {
                    // 直接将网络适配器关掉后发生
                    Debuger.LogError("网络关闭");
                    if (mDisconnectAction != null)
                    {
                        mDisconnectAction("网络关闭");
                    }
                }
                else if (ex.Message.StartsWith("Connection timed out"))
                {
                    Debuger.LogError("连接超时");
                    if (mDisconnectAction != null)
                    {
                        mDisconnectAction("连接超时");
                    }
                }
                else
                {
                    Debuger.LogError("网络异常");
                    if (mDisconnectAction != null)
                    {
                        mDisconnectAction("网络异常");
                    }
                }
            }
        }

        /// <summary>
        /// 开始重连
        /// </summary>
        public void StartReconnect()
        {
            if (mReconnectTimer == null)
            {
                mReconnectTimer = new Timer();
                mReconnectTimer.Elapsed += new ElapsedEventHandler(OnReconnect);
                /// 每隔1秒重连1次
                mReconnectTimer.Interval = ReconnectInterval;
            }
            mReconnectTimer.Enabled = true;
        }

        /// <summary>
        /// 结束重连
        /// </summary>
        public void StopReconnect()
        {
            if (mReconnectTimer != null)
            {
                mReconnectTimer.Enabled = false;
                Debuger.Log("关闭重连接口");
            }
        }

        void OnReconnect(object source, ElapsedEventArgs e)
        {
            Connect(mLastAddress);
        }

        #endregion
    }
}