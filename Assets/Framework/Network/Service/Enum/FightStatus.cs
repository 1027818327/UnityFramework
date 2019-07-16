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
    public enum FightStatus
    {
        /// <summary>
        /// 开始战斗失败
        /// </summary>
        Fail = 1,
        /// <summary>
        /// 房间已经开始战斗
        /// </summary>
        HasBegin,
        /// <summary>
        /// 房间不存在
        /// </summary>
        RoomNotExist,
    }
}