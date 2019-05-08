namespace Vive
{
    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum OperatType
    {
        /// <summary>
        /// 手柄停留道具几秒
        /// </summary>
        Touch_Stay = 1,
        /// <summary>
        /// 手柄触摸到道具按下侧边键抓起，再按下扳机键
        /// </summary>
        Touch_Grip_Trigger = 2,
        /// <summary>
        /// 手柄触摸到道具按下扳机键
        /// </summary>
        Touch_Trigger = 3,
        /// <summary>
        /// 手柄触摸到道具按下扳机键触发动画，动画结束
        /// </summary>
        Touch_Trigger_PlayAnim = 4,
        /// <summary>
        /// 手柄触摸到道具按下侧边键跟随手只沿一个轴摆动
        /// </summary>
        Touch_Grip_SwingJoint = 5,
        /// <summary>
        /// 同上，但没有铰链效果
        /// </summary>
        Touch_Grip_Swing = 6,
    }

    /// <summary>
    /// 旋转类型枚举
    /// </summary>
    public enum SwingType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 绕x轴旋转
        /// </summary>
        X = 1,
        /// <summary>
        /// 绕y轴旋转
        /// </summary>
        Y = 2,
        /// <summary>
        /// 绕z轴旋转
        /// </summary>
        Z = 3
    }
}
