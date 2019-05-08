namespace Framework.Event
{
    public partial class LogicEventType
    {
        #region 手柄事件开始
        /// <summary>
        /// 按下手柄Two Button
        /// </summary>
        public const uint PressHandTwoBtn = 1;
        /// <summary>
        /// 按下左菜单键
        /// </summary>
        public const uint PressLeftMenuBtn = 2;
        /// <summary>
        /// 按下右菜单键
        /// </summary>
        public const uint PressRightMenuBtn = 3;
        /// <summary>
        /// 左手柄触发器点击
        /// </summary>
        public const uint LeftTriggerClick = 4;
        /// <summary>
        /// 右手柄触发器点击
        /// </summary>
        public const uint RightTriggerClick = 5;
        /// <summary>
        /// 左手柄触发器取消点击
        /// </summary>
        public const uint LeftTriggerUnClick = 6;
        /// <summary>
        /// 右手柄触发器取消点击
        /// </summary>
        public const uint RightTriggerUnClick = 7;
        /// <summary>
        /// 左手柄触摸板键触摸开始
        /// </summary>
        public const uint StartTouchLeftTouchpad = 8;
        /// <summary>
        /// 右手柄触摸板键触摸开始
        /// </summary>
        public const uint StartTouchRightTouchpad = 9;
        /// <summary>
        /// 左手柄触摸板键触摸结束
        /// </summary>
        public const uint EndTouchLeftTouchpad = 10;
        /// <summary>
        /// 右手柄触摸板键触摸结束
        /// </summary>
        public const uint EndTouchRightTouchpad = 11;
        /// <summary>
        /// 左手柄触摸板键按下
        /// </summary>
        public const uint PressLeftTouchpad = 12;
        /// <summary>
        /// 右手柄触摸板键按下
        /// </summary>
        public const uint PressRightTouchpad = 13;
        /// <summary>
        /// 左手柄触摸板键抬起
        /// </summary>
        public const uint ReleaseLeftTouchpad = 14;
        /// <summary>
        /// 右手柄触摸板键抬起
        /// </summary>
        public const uint ReleaseRightTouchpad = 15;


        /// <summary>
        /// 从Vive系统菜单返回事件
        /// </summary>
        public const uint ControllerEnable = 20;
        /// <summary>
        /// 将物体放在左手上
        /// </summary>
        public const uint PutObjectToLeftHand = 21;
        /// <summary>
        /// 将物体放在右手上
        /// </summary>
        public const uint PutObjectToRightHand = 22;

        #endregion 手柄事件结束

        /// <summary>
        /// 正要抓动画
        /// </summary>
        public const uint GripHandAnim = 100;
        /// <summary>
        /// 抓到了物体动画
        /// </summary>
        public const uint InteractGrabAnim = 101;
        /// <summary>
        /// 释放抓动画
        /// </summary>
        public const uint ReleaseHandAnim = 102;
        /// <summary>
        /// 位置传送(强制传送)
        /// </summary>
        public const uint ForceTeleport = 103;
        /// <summary>
        /// 指引指向物体发生改变
        /// </summary>
        public const uint GuideChange = 104;
    }
}