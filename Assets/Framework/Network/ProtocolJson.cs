using Framework.Debugger;
using LitJson;
using System;

namespace Framework.Network
{
    /// <summary>
    /// Json协议模型
    /// </summary>
    public class ProtocolJson : ProtocolBase
    {
        /// <summary>
        /// 传输的json字符串
        /// </summary>
        private string jsonStr;
        /// <summary>
        /// Json对象
        /// </summary>
        private JsonData jsonData;

        public string JsonStr
        {
            get
            {
                return jsonStr;
            }

            private set
            {
                jsonStr = value;
            }
        }

        public JsonData JsonData
        {
            get
            {
                return jsonData;
            }

            private set
            {
                jsonData = value;
            }
        }

        /// <summary>
        /// 解码器，解码readbuff中从start开始的length字节
        /// </summary>
        /// <param name="readbuff"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override ProtocolBase Decode(byte[] readbuff, int start, int length)
        {
            ProtocolJson protocol = new ProtocolJson();
            protocol.JsonStr = System.Text.Encoding.UTF8.GetString(readbuff, start, length);
            return (ProtocolBase)protocol;
        }

        /// <summary>
        /// 编码器
        /// </summary>
        /// <returns></returns>
        public override byte[] Encode()
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            return b;
        }

        /// <summary>
        /// 协议名称，用于消息分发
        /// </summary>
        /// <returns></returns>
        public override string GetName()
        {
            if (jsonData == null)
            {
                return "";
            }
            return jsonData["protocolName"].ToString();
        }

        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        public override string GetDesc()
        {
            return jsonStr;
        }

        public void Serialize(Object varO)
        {
            jsonStr = JsonMapper.ToJson(varO);
            jsonData = JsonMapper.ToObject(jsonStr);
        }

        public void Deserialize(string str)
        {
            try
            {
                jsonStr = str;
                jsonData = JsonMapper.ToObject(str);
            }
            catch (Exception e)
            {
                jsonData = null;
                Debuger.LogError(string.Format("json反序列化异常:{0}", e.Message));
            }
        }

        public T Deserialize<T>() where T : class
        {
            try
            {
                var tempObj = JsonMapper.ToObject<T>(jsonStr);
                return tempObj;
            }
            catch (Exception e)
            {
                Debuger.LogError(string.Format("json反序列化异常:{0}", e.Message));
                return null;
            }
        }
    }
}