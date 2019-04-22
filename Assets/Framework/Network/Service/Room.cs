
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 * 
 *        创建者：  陈伟超
 *      创建时间：  2018/9/30 10:12:05
 *  
 */
#endregion


using System.Collections.Generic;

namespace Framework.Network
{
    public class Room
    {
        #region Fields
        private int mRoomId;
        private List<Player> mPlayerList = new List<Player>();

        /// <summary>
        /// 最多5个玩家，第一个玩家是房主，2到4是操作者，第5个是观看者
        /// </summary>
        public const int MaxPlayerCount = 5;
        #endregion

        #region Properties
        public List<Player> PlayerList
        {
            get
            {
                return mPlayerList;
            }

            private set
            {
                mPlayerList = value;
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId">房号</param>
        public Room(int roomId)
        {
            mRoomId = roomId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId">房号</param>
        /// <param name="varMasterPlayer">房主</param>
        public Room(int roomId, Player varMasterPlayer)
        {
            mRoomId = roomId;
            if (varMasterPlayer.PlayerIdentity != PlayerIdentity.RoomMaster)
            {
                varMasterPlayer.PlayerIdentity = PlayerIdentity.RoomMaster;
            }
            Enter(varMasterPlayer);
        }

        public Player GetPlayer(string varId)
        {
            for (int i = 0; i < mPlayerList.Count; i++)
            {
                if (mPlayerList[i].Id == varId)
                {
                    return mPlayerList[i];
                }
            }
            return null;
        }

        public bool Enter(Player varPlayer)
        {
            if (varPlayer == null)
            {
                return false;
            }

            foreach (var tempP in mPlayerList)
            {
                if (tempP.Id == varPlayer.Id)
                {
                    return false;
                }
            }
            mPlayerList.Add(varPlayer);
            return true;
        }

        public bool Leave(string varId)
        {
            bool tempFind = false;
            for (int i = 0; i < mPlayerList.Count; i++)
            {
                if (mPlayerList[i].Id == varId)
                {
                    mPlayerList.RemoveAt(i);
                    tempFind = true;
                    break;
                }
            }
            return tempFind;
        }

        public int GetPlayerCount()
        {
            if (mPlayerList == null || mPlayerList.Count == 0)
            {
                return 0;
            }
            return mPlayerList.Count;
        }

        public void UpdateRoomMember(List<Player> varPlayerList)
        {
            mPlayerList = varPlayerList;
        }
        #endregion
    }
}