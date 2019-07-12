
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/07/12 10:53:45
 *  
 */
#endregion


using Framework.Unity.UI;

namespace Framework.Network.Web
{
    public class DialogStrategy : IReconnectStrategy
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods
        public void HandleDisconnected(string reason)
        {
            UIMsgBox.UIMsgBoxArgs tempData = new UIMsgBox.UIMsgBoxArgs();
            tempData.Title = "提示";
            tempData.Content = reason;
            tempData.Style = UIMsgBox.Style.OK;
            tempData.mBtnTexts = new string[1] { "重连" };
            tempData.CloseAction = delegate (UIMsgBox.Result result)
            {
                WebMgr.SrvConn.StartReconnect();
            };
            UIEventArgs<UIMsgBox.UIMsgBoxArgs> tempArgs2 = new UIEventArgs<UIMsgBox.UIMsgBoxArgs>(tempData);
            UIManager.GetInstance().ShowUI(UIPath.MsgBox, tempArgs2);
        }
        #endregion

    }
}