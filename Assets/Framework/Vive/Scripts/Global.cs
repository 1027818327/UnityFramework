using UnityEngine;

namespace Vive
{
    /// <summary>
    /// Vive全局变量类
    /// </summary>
	public partial class Global 
	{
        /// <summary>
        /// 右手物品
        /// </summary>
        public static GameObject m_rightHandObj;
        /// <summary>
        /// 左手物品
        /// </summary>
        public static GameObject m_leftHandObj;
        /// <summary>
        /// 右手柄抓取键是否按下
        /// </summary>
        public static bool IsRightGripPressed;
        /// <summary>
        /// 左手柄抓取键否按下
        /// </summary>
        public static bool IsLeftGripPressed;
        /// <summary>
        /// 右手柄扳机键是否按下
        /// </summary>
        public static bool IsRightTriggerPressed;
        /// <summary>
        /// 左手柄抓取键否按下
        /// </summary>
        public static bool IsLeftTriggerPressed;
        /// <summary>
        /// 右手柄控制器
        /// </summary>
        public static GameObject RightController;
        /// <summary>
        /// 左手柄控制器
        /// </summary>
        public static GameObject LeftController;
        /// <summary>
        /// 左手柄控制器Tag
        /// </summary>
        public const string LeftControllerTag = "LeftController";
        /// <summary>
        /// 右手柄控制器Tag
        /// </summary>
        public const string RightControllerTag = "RightController";
    }
}