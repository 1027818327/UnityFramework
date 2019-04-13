using Framework.Event;
using UnityEngine;
using VRTK;

namespace Vive
{
    public class HandAniCtrl : MonoBehaviour
    {
        public SDK_BaseController.ControllerHand m_handRL;

        private Animator mAnimator;

        private EnHandAni m_AniType = EnHandAni.Type0;

        private void Start()
        {
            mAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            EventManager.Instance.AddEventListener(LogicEventType.GripHandAnim, GripHandAnim);
            EventManager.Instance.AddEventListener(LogicEventType.InteractGrabAnim, InteractGrabAnim);
            EventManager.Instance.AddEventListener(LogicEventType.ReleaseHandAnim, ReleaseHandAnim);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveEventListener(LogicEventType.GripHandAnim, GripHandAnim);
            EventManager.Instance.RemoveEventListener(LogicEventType.InteractGrabAnim, InteractGrabAnim);
            EventManager.Instance.RemoveEventListener(LogicEventType.ReleaseHandAnim, ReleaseHandAnim);
        }

        /// <summary>
        /// 正要抓动画
        /// </summary>
        /// <param name="varData"></param>
        private void GripHandAnim(EventData varData)
        {
            var tempData = varData as EventDataEx<SDK_BaseController.ControllerHand>;
            if (tempData == null)
            {
                return;
            }
            if (m_handRL == tempData.GetData())
            {
                PlayHandAnim();
            }
        }

        /// <summary>
        /// 抓到了物体动画
        /// </summary>
        /// <param name="varData"></param>
        private void InteractGrabAnim(EventData varData)
        {
            var tempData = varData as EventDataEx<ObjectInteractEventArgs>;
            if (tempData == null)
            {
                return;
            }
            ObjectInteractEventArgs e = tempData.GetData();

            if (m_handRL == e.controllerReference.hand)
            {
                HandAniIdx tempHai = e.target.GetComponent<HandAniIdx>();
                if (tempHai != null)
                {
                    m_AniType = tempHai.m_handAni;
                }

                PlayHandAnim();
            }
        }

        /// <summary>
        /// 释放抓动画
        /// </summary>
        /// <param name="varData"></param>
        private void ReleaseHandAnim(EventData varData)
        {
            var tempData = varData as EventDataEx<SDK_BaseController.ControllerHand>;
            if (tempData == null)
            {
                return;
            }
            if (m_handRL == tempData.GetData())
            {
                if (mAnimator != null)
                {
                    mAnimator.SetBool(m_AniType.ToString(), false);
                }
                m_AniType = EnHandAni.Type0;
            }
        }

        private void PlayHandAnim()
        {
            if (mAnimator != null)
            {
                for (var i = EnHandAni.Type0; i < EnHandAni.Max; i++)
                {
                    mAnimator.SetBool(i.ToString(), i == m_AniType);
                }
            }
        }
    }
}
