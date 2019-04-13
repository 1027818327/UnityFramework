using UnityEngine;
using Framework.Event;
using Framework.Unity.Tools;
using Framework.Debugger;

namespace Vive
{
    /// <summary>
    ///  此脚本适用于没有手柄用键盘测试，当用手柄测试时，请不要使用此脚本
    /// </summary>
    public class SimulationHandle : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private GameObject mLeftObj;
        [HideInInspector]
        [SerializeField]
        private GameObject mRightObj;

        public GameObject LeftObj
        {
            get
            {
                return mLeftObj;
            }

            set
            {
                if (!enabled) return;
                mLeftObj = value;
                Global.m_leftHandObj = value;
                if (value != null)
                {
                    SetKinematic(value, true);
                }
            }
        }

        public GameObject RightObj
        {
            get
            {
                return mRightObj;
            }

            set
            {
                if (!enabled) return;
                mRightObj = value;
                Global.m_rightHandObj = value;
                if (value != null)
                {
                    SetKinematic(value, true);
                }
            }
        }

        private void SetKinematic(GameObject varObj, bool enable)
        {
            Rigidbody rigidbody = GameObjectUtils.FindComponent<Rigidbody>(varObj);
            if (rigidbody != null)
            {
                rigidbody.isKinematic = enable;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debuger.Log("左触发器单击");
                EventManager.Instance.DispatchEvent(LogicEventType.LeftTriggerClick);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                Debuger.Log("左触发器取消点击");
                EventManager.Instance.DispatchEvent(LogicEventType.LeftTriggerUnClick);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debuger.Log("右触发器单击");
                EventManager.Instance.DispatchEvent(LogicEventType.RightTriggerClick);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                Debuger.Log("右触发器取消点击");
                EventManager.Instance.DispatchEvent(LogicEventType.RightTriggerUnClick);
            }
        }
    }
}
