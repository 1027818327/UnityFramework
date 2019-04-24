#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/22 09:04:54
 *  
 */
#endregion



namespace Framework.Network
{
    /// <summary>
    /// 消息处理接口
    /// </summary>
    public interface IMessageHandle
    {
        #region Properties

        #endregion

        #region Protected & Public Methods
        /// <summary>
        /// 发送数据包无回调
        /// </summary>
        /// <param name="protocol">协议数据</param>
        void SendPacket(ProtocolBase protocol);

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="protocol">协议数据</param>
        /// <param name="varDelegate">回调函数</param>
        void SendPacket(ProtocolBase protocol, MsgDistribution.Delegate varDelegate);

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="name"></param>
        /// <param name="varDelegate"></param>
        void AddListener(string name, MsgDistribution.Delegate varDelegate);

        /// <summary>
        /// 添加单次事件监听
        /// </summary>
        /// <param name="name"></param>
        /// <param name="varDelegate"></param>
        void AddOnceListener(string name, MsgDistribution.Delegate varDelegate);

        /// <summary>
        /// 删除监听事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="varDelegate"></param>
        void DelListener(string name, MsgDistribution.Delegate varDelegate);

        /// <summary>
        /// 删除单次监听事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="varDelegate"></param>
        void DelOnceListener(string name, MsgDistribution.Delegate varDelegate);
        #endregion
    }
}