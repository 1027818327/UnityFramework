
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


using System.Collections.Generic;

namespace Framework.Network
{
    public interface IRoom
    {
        #region Properties
        /// <summary>
        /// 房间玩家列表
        /// </summary>
        List<Player> PlayerList { get; set; }
        #endregion

        #region Protected & Public Methods
        /// <summary>
        /// 获取玩家
        /// </summary>
        /// <param name="varId"></param>
        /// <returns></returns>
        Player GetPlayer(string varId);

        /// <summary>
        /// 进入玩家
        /// </summary>
        /// <param name="varPlayer"></param>
        /// <returns></returns>
        bool Enter(Player varPlayer);

        /// <summary>
        /// 离开玩家
        /// </summary>
        /// <param name="varId"></param>
        /// <returns></returns>
        bool Leave(string varId);

        /// <summary>
        /// 获取房间当前玩家数量
        /// </summary>
        /// <returns></returns>
        int GetPlayerCount();

        /// <summary>
        /// 更新房间成员
        /// </summary>
        /// <param name="varPlayerList"></param>
        void UpdateRoomMember(List<Player> varPlayerList);
        #endregion
    }
}