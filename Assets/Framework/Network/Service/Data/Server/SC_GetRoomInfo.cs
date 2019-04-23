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

    public class SC_GetRoomInfo : MessageBase
    {
        #region Fields
        public SC_Player[] text;
        #endregion

        #region Properties

        #endregion
    }
}