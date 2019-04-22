
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 * 
 *        创建者：陈伟超  
 *      创建时间：  2018/9/29 15:07:00
 *  
 */
#endregion

using UnityEngine;

namespace Framework.Network
{
    public class Player
    {
        #region Fields
        [SerializeField]
        private string mName;
        [SerializeField]
        private string mId;
        /// <summary>
        /// 所在房间身份
        /// </summary>
        [SerializeField]
        private RoomIdentity mRoomIdentity;
        /// <summary>
        /// 业务逻辑身份，应用里面的身份
        /// </summary>
        private int roleId;

        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return mName;
            }

            set
            {
                mName = value;
            }
        }

        public string Id
        {
            get
            {
                return mId;
            }

            set
            {
                mId = value;
            }
        }

        public RoomIdentity RoomIdentity
        {
            get
            {
                return mRoomIdentity;
            }

            set
            {
                mRoomIdentity = value;
            }
        }

        public int RoleId
        {
            get
            {
                return roleId;
            }

            set
            {
                roleId = value;
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods
        public Player() { }

        public Player(string varName, string varId)
        {
            mName = varName;
            mId = varId;
        }
        #endregion
    }

    public enum RoomIdentity
    {
        /// <summary>
        /// 没有身份
        /// </summary>
        None,
        /// <summary>
        /// 房主
        /// </summary>
        RoomMaster,
        /// <summary>
        /// 操作者
        /// </summary>
        Operator,
        /// <summary>
        /// 观察者
        /// </summary>
        Watcher,
    }
}