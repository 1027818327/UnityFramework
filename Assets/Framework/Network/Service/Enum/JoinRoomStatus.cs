#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/05/06 17:07:01
 *  
 */
#endregion


namespace Framework.Network
{
    public enum JoinRoomStatus
    {
        /// <summary>
        /// 房间不存在
        /// </summary>
        RoomNotExist = 1,
        /// <summary>
        /// 已经在房间
        /// </summary>
        InRoom,
        /// <summary>
        /// 加入成功
        /// </summary>
        Success,
        /// <summary>
        /// 房间已开始
        /// </summary>
        RoomBegin
    }
}