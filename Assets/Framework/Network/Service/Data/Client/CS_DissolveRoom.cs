#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 * 
 *        创建者：  陈伟超
 *      创建时间：  2019/4/19 17:16:33
 *  
 */
#endregion



namespace Framework.Network
{
    public class CS_DissolveRoom : MessageBase
    {
        #region Fields
        public string user_id;
        public string room_id;
        public string client_id;
        #endregion
    }
}