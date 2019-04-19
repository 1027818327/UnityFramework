namespace Framework.Network
{
    /// <summary>
    /// 网络管理
    /// </summary>
    public class WebMgr
    {
        public static WebConnection SrvConn = new WebConnection();
        public static void Update()
        {
            SrvConn.Update();
        }

        /*
        /// <summary>
        /// 心跳
        /// </summary>
        /// <returns></returns>
        public static ProtocolBase GetHeatBeatProtocol()
        {
            //具体的发送内容根据服务端设定改动
            ProtocolBytes protocol = new ProtocolBytes();
            protocol.AddString("HeatBeat");
            return protocol;
        }
        */
    }
}