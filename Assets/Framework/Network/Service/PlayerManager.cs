
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/22 09:12:31
 *  
 */
#endregion


using Framework.Pattern;

namespace Framework.Network
{
    /// <summary>
    /// 玩家管理
    /// </summary>
    public partial class PlayerManager : Singleton<PlayerManager>
    {
        #region Fields
        /// <summary>
        /// 自己
        /// </summary>
        private Player mPlayer;
        /// <summary>
        /// 自己所在的房间
        /// </summary>
        private Room mRoom;

        private ILoginModule loginModule;
        private IRoomModule roomModule;
        private INetworkConnect networkConnect;


        #endregion

        #region Properties
        public Player Player
        {
            get
            {
                return mPlayer;
            }

            set
            {
                mPlayer = value;
            }
        }

        public Room Room
        {
            get
            {
                return mRoom;
            }

            set
            {
                mRoom = value;
            }
        }

        public ILoginModule LoginModule
        {
            get
            {
                return loginModule;
            }

            set
            {
                loginModule = value;
            }
        }

        public IRoomModule RoomModule
        {
            get
            {
                return roomModule;
            }

            set
            {
                roomModule = value;
            }
        }

        public INetworkConnect NetworkConnect
        {
            get
            {
                return networkConnect;
            }

            set
            {
                networkConnect = value;
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods

        /// <summary>
        /// 获取自己的id
        /// </summary>
        /// <returns></returns>
        public string GetPlayerId()
        {
            if (mPlayer == null)
            {
                return "-1";
            }
            return mPlayer.Id;
        }

        /// <summary>
        /// 是否是自己
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsSelf(string id)
        {
            if (mPlayer == null || mPlayer.Id != id)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取自己的身份
        /// </summary>
        /// <returns></returns>
        public RoomIdentity GetPlayerIdentity()
        {
            if (mPlayer == null)
            {
                return RoomIdentity.None;
            }
            return mPlayer.RoomIdentity;
        }

        /// <summary>
        /// 重置自己的身份
        /// </summary>
        public void ResetIdentity()
        {
            if (mPlayer != null)
            {
                mPlayer.RoomIdentity = RoomIdentity.None;
            }
        }
        #endregion
    }
}