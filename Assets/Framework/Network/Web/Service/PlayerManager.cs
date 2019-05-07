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

namespace Framework.Network
{
    public partial class PlayerManager
    {
        /// <summary>
        /// 签名
        /// </summary>
        private string sign;
        /// <summary>
        /// 会话id
        /// </summary>
        private string client_id;

        public string Sign
        {
            get
            {
                return sign;
            }

            set
            {
                sign = value;
            }
        }

        public string Client_id
        {
            get
            {
                return client_id;
            }

            set
            {
                client_id = value;
            }
        }
    }
}
