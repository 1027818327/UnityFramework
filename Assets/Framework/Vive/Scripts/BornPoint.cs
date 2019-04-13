using UnityEngine;
using Framework.Event;

namespace Vive
{
    /// <summary>
    /// 出生点类
    /// </summary>
    public class BornPoint : MonoBehaviour
    {
        /// <summary>
        /// 延迟传送
        /// </summary>
        [Header("延迟传送")]
        public float mDelayTime;
        /// <summary>
        /// 是否Enable就传送
        /// </summary>
        public bool mEnableCall = true;

        private void Start()
        {
            if (mEnableCall)
            {
                return;
            }

            if (mDelayTime <= 0)
            {
                BackToBornPoint();
            }
            else
            {
                Invoke("BackToBornPoint", mDelayTime);
            }
        }

        private void OnEnable()
        {
            if (!mEnableCall)
            {
                return;
            }

            if (mDelayTime <= 0)
            {
                BackToBornPoint();
            }
            else
            {
                Invoke("BackToBornPoint", mDelayTime);
            }
        }

        /// <summary>
        /// 回到传送点
        /// </summary>
        [ContextMenu("BackToBornPoint")]
        public void BackToBornPoint()
        {
            EventManager.Instance.DispatchEvent(LogicEventType.ForceTeleport, new EventDataEx<Transform>(this.transform));
        }
    }
}
