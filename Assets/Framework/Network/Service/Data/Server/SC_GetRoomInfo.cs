#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/23 15:46:43
 *  
 */
#endregion



namespace Framework.Network
{
    public class SC_Player
    {
        /// <summary>
        /// 玩家id
        /// </summary>
        public int id;
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string name;
        /// <summary>
        /// 玩家身份
        /// </summary>
        public int role_id;
    }

    public class SC_Room
    {
        /// <summary>
        /// 房号
        /// </summary>
        public int id;

        /// <summary>
        /// 房主id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 是否已开始战斗
        /// </summary>
        public bool is_start_text;
        /// <summary>
        /// 玩家列表
        /// </summary>
        public SC_Player[] room_info;

        public int course;
    }

    public class SC_GetRoomInfo : MessageBase
    {
        #region Fields
        public SC_Room room;
        #endregion

        #region Properties

        #endregion
    }
}