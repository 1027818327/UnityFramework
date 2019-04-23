namespace Framework.Network.Web
{
    public class WebMessageHandle : IMessageHandle
    {
        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="protocol">协议数据</param>
        /// <param name="varDelegate">回调函数</param>
        public void SendPacket(ProtocolBase protocol, MsgDistribution.Delegate varDelegate)
        {
            WebMgr.SrvConn.Send(protocol, varDelegate);
        }

        public void AddListener(string name, MsgDistribution.Delegate varDelegate)
        {
            WebMgr.SrvConn.msgDist.AddListener(name, varDelegate);
        }

        public void AddOnceListener(string name, MsgDistribution.Delegate varDelegate)
        {
            WebMgr.SrvConn.msgDist.AddOnceListener(name, varDelegate);
        }

        public void DelListener(string name, MsgDistribution.Delegate varDelegate)
        {
            WebMgr.SrvConn.msgDist.DelListener(name, varDelegate);
        }

        public void DelOnceListener(string name, MsgDistribution.Delegate varDelegate)
        {
            WebMgr.SrvConn.msgDist.DelOnceListener(name, varDelegate);
        }
    }
}
