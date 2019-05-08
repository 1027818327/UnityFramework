using Framework.Debugger;
using Framework.Event;
using UnityEngine;
using VRTK;

namespace Vive
{
    public class VRTK_InteractEvents_Listener : MonoBehaviour
    {
        [Header("左手")]
        public GameObject m_leftHand;
        [Header("右手")]
        public GameObject m_rightHand;

        [Header("左手渲染器")]
        private Renderer[] mLeftHandRenders;
        [Header("右手渲染器")]
        private Renderer[] mRightHandRenders;

        public Transform mLeftController;
        public Transform mRightController;

        public VRTK_HeightAdjustTeleport mTeleport;

        public static VRTK_InteractEvents_Listener m_instance;

        private bool mIsRotate;
        private float mRotateAngle;

        void Start()
        {
            m_instance = this;
            Global.RightController = mRightController.gameObject;
            Global.LeftController = mLeftController.gameObject;

            AddHandLeverEvent(mLeftController);
            AddHandLeverEvent(mRightController);

            HandAniAndRenderInit();

            EventManager.Instance.AddEventListener(LogicEventType.ForceTeleport, ForceTeleport);
        }

        private void AddHandLeverEvent(Transform varTrans)
        {
            if (varTrans == null)
            {
                return;
            }

            VRTK_InteractGrab tempGrab = varTrans.GetComponent<VRTK_InteractGrab>();
            if (tempGrab != null)
            {
                tempGrab.ControllerGrabInteractableObject += DoInteractGrab;
                tempGrab.ControllerUngrabInteractableObject += DoInteractUnGrab;
            }

            VRTK_InteractTouch tempTouch = varTrans.GetComponent<VRTK_InteractTouch>();
            if (tempTouch != null)
            {
                tempTouch.ControllerTouchInteractableObject += DoTouchInteract;
                tempTouch.ControllerUntouchInteractableObject += DoUnTouchInteract;
            }

            VRTK_ControllerEvents tempController = varTrans.GetComponent<VRTK_ControllerEvents>();
            if (tempController != null)
            {
                tempController.TriggerUnclicked += DoTriggerUnClicked;
                tempController.TriggerClicked += DoTriggerClicked;
                tempController.GripPressed += DoGripPressed;
                tempController.GripReleased += DoGripReleased;
                tempController.TouchpadPressed += OnTouchpadPress;
                tempController.TouchpadReleased += OnTouchpadReleased;
                tempController.TouchpadTouchStart += OnTouchpadTouchStart;
                tempController.TouchpadTouchEnd += OnTouchpadTouchEnd;
                tempController.TouchpadAxisChanged += OnTouchPadAxisChanged;
                tempController.ButtonTwoPressed += DoButtonTwoPressed;
            }
        }

        private void RemoveHandLeverEvent(Transform varTrans)
        {
            if (varTrans == null)
            {
                return;
            }

            VRTK_InteractGrab tempGrab = varTrans.GetComponent<VRTK_InteractGrab>();
            if (tempGrab != null)
            {
                tempGrab.ControllerGrabInteractableObject -= DoInteractGrab;
                tempGrab.ControllerUngrabInteractableObject -= DoInteractUnGrab;
            }

            VRTK_InteractTouch tempTouch = varTrans.GetComponent<VRTK_InteractTouch>();
            if (tempTouch != null)
            {
                tempTouch.ControllerTouchInteractableObject -= DoTouchInteract;
                tempTouch.ControllerUntouchInteractableObject -= DoUnTouchInteract;
            }

            VRTK_ControllerEvents tempController = varTrans.GetComponent<VRTK_ControllerEvents>();
            if (tempController != null)
            {
                tempController.TriggerUnclicked -= DoTriggerUnClicked;
                tempController.TriggerClicked -= DoTriggerClicked;
                tempController.GripPressed -= DoGripPressed;
                tempController.GripReleased -= DoGripReleased;
                tempController.TouchpadPressed -= OnTouchpadPress;
                tempController.TouchpadReleased -= OnTouchpadReleased;
                tempController.TouchpadTouchStart -= OnTouchpadTouchStart;
                tempController.TouchpadTouchEnd -= OnTouchpadTouchEnd;
                tempController.TouchpadAxisChanged -= OnTouchPadAxisChanged;
                tempController.ButtonTwoPressed -= DoButtonTwoPressed;
            }
        }

        void HandAniAndRenderInit()
        {
            if (m_leftHand != null)
            {
                mLeftHandRenders = m_leftHand.GetComponentsInChildren<Renderer>();
            }
            if (m_rightHand != null)
            {
                mRightHandRenders = m_rightHand.GetComponentsInChildren<Renderer>();
            }
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveEventListener(LogicEventType.ForceTeleport, ForceTeleport);

            RemoveHandLeverEvent(mLeftController);
            RemoveHandLeverEvent(mRightController);
        }

        public void DoGripPressed(object sender, ControllerInteractionEventArgs e)
        {
            SDK_BaseController.ControllerHand tempHand = GetHandType(e);
            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                Global.IsLeftGripPressed = true;
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                Global.IsRightGripPressed = true;
            }
            EventManager.Instance.DispatchEvent(LogicEventType.GripHandAnim, new EventDataEx<SDK_BaseController.ControllerHand>(tempHand));
        }

        public void DoGripReleased(object sender, ControllerInteractionEventArgs e)
        {
            SDK_BaseController.ControllerHand tempHand = GetHandType(e);

            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                Global.IsLeftGripPressed = false;
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                Global.IsRightGripPressed = false;
            }

            if (tempHand == SDK_BaseController.ControllerHand.Left && Global.m_leftHandObj == null)
            {
                EventManager.Instance.DispatchEvent(LogicEventType.ReleaseHandAnim, new EventDataEx<SDK_BaseController.ControllerHand>(tempHand));
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right && Global.m_rightHandObj == null)
            {
                EventManager.Instance.DispatchEvent(LogicEventType.ReleaseHandAnim, new EventDataEx<SDK_BaseController.ControllerHand>(tempHand));
            }
        }

        public void DoTriggerUnClicked(object sender, ControllerInteractionEventArgs e)
        {
            SDK_BaseController.ControllerHand tempHand = GetHandType(e);
            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                Debuger.Log("左触发器取消点击");
                Global.IsLeftTriggerPressed = false;
                EventManager.Instance.DispatchEvent(LogicEventType.LeftTriggerUnClick);
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                Debuger.Log("右触发器取消点击");
                Global.IsRightTriggerPressed = false;
                EventManager.Instance.DispatchEvent(LogicEventType.RightTriggerUnClick);
            }
        }

        public void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
        {
            SDK_BaseController.ControllerHand tempHand = GetHandType(e);
            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                Debuger.Log("左触发器单击");
                Global.IsLeftTriggerPressed = true;
                EventManager.Instance.DispatchEvent(LogicEventType.LeftTriggerClick);
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                Debuger.Log("右触发器单击");
                Global.IsRightTriggerPressed = true;
                EventManager.Instance.DispatchEvent(LogicEventType.RightTriggerClick);
            }
        }

        public void DoTouchInteract(object sender, ObjectInteractEventArgs e)
        {
            //HandHapticPulse(e.controllerReference.hand);
            //Debuger.Log("触摸到了"+e.target.name);
        }

        public void DoUnTouchInteract(object sender, ObjectInteractEventArgs e)
        {
            //Debuger.Log("没有触摸到了" + e.target.name);
        }

        public void DoInteractGrab(object sender, ObjectInteractEventArgs e)
        {
            SDK_BaseController.ControllerHand tempHand = e.controllerReference.hand;
            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                //Debuger.Log("左手抓到了" + e.target.name);
                EventManager.Instance.DispatchEvent(LogicEventType.InteractGrabAnim, new EventDataEx<ObjectInteractEventArgs>(e));

                VRTK_InteractControllerAppearance tempScript = e.target.GetComponent<VRTK_InteractControllerAppearance>();
                if (tempScript != null && tempScript.hideControllerOnGrab)
                {
                    ShowHandRender(mLeftHandRenders, false);
                }

                Global.m_leftHandObj = e.target;
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                //Debuger.Log("右手抓到了" + e.target.name);
                EventManager.Instance.DispatchEvent(LogicEventType.InteractGrabAnim, new EventDataEx<ObjectInteractEventArgs>(e));

                VRTK_InteractControllerAppearance tempScript = e.target.GetComponent<VRTK_InteractControllerAppearance>();
                if (tempScript != null && tempScript.hideControllerOnGrab)
                {
                    ShowHandRender(mRightHandRenders, false);
                }

                Global.m_rightHandObj = e.target;
            }
        }

        public void DoInteractUnGrab(object sender, ObjectInteractEventArgs e)
        {
            SDK_BaseController.ControllerHand tempHand = e.controllerReference.hand;
            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                //Debuger.Log("左手取消抓到了" + e.target.name);

                VRTK_InteractControllerAppearance tempScript = e.target.GetComponent<VRTK_InteractControllerAppearance>();
                if (tempScript != null)
                {
                    ShowHandRender(mLeftHandRenders, true);
                }
                EventManager.Instance.DispatchEvent(LogicEventType.ReleaseHandAnim, new EventDataEx<SDK_BaseController.ControllerHand>(tempHand));

                Global.m_leftHandObj = null;
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                //Debuger.Log("右手取消抓到了" + e.target.name);
                VRTK_InteractControllerAppearance tempScript = e.target.GetComponent<VRTK_InteractControllerAppearance>();
                if (tempScript != null)
                {
                    ShowHandRender(mRightHandRenders, true);
                }

                EventManager.Instance.DispatchEvent(LogicEventType.ReleaseHandAnim, new EventDataEx<SDK_BaseController.ControllerHand>(tempHand));
                Global.m_rightHandObj = null;
            }
        }

        public void OnControllerEnabled(object sender, ControllerInteractionEventArgs e)
        {
            SDK_BaseController.ControllerHand tempHand = GetHandType(e);
            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                EventManager.Instance.DispatchEvent(LogicEventType.ControllerEnable);
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                EventManager.Instance.DispatchEvent(LogicEventType.ControllerEnable);
            }
        }

        private void ShowHandRender(Renderer[] array, bool enable)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i].enabled = enable;
            }
        }

        public void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
        {
            SDK_BaseController.ControllerHand tempHand = GetHandType(e);
            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                Debuger.Log("左菜单键按下");
                EventManager.Instance.DispatchEvent(LogicEventType.PressLeftMenuBtn);
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                Debuger.Log("右菜单键按下");
                EventManager.Instance.DispatchEvent(LogicEventType.PressRightMenuBtn);
            }
        }

        public SDK_BaseController.ControllerHand GetHandType(ControllerInteractionEventArgs e)
        {
            return e.controllerReference.hand;
        }

        public void RestoreHandRender()
        {
            ShowHandRender(mLeftHandRenders, true);
            ShowHandRender(mRightHandRenders, true);
            // 将手模型设为可见
            mLeftController.GetChild(0).gameObject.SetActive(true);
            mRightController.GetChild(0).gameObject.SetActive(true);
        }

        /// <summary>
        /// 删除手上的物体
        /// </summary>
        public void DestroyHandObj()
        {
            if (Global.m_leftHandObj != null)
            {
                Destroy(Global.m_leftHandObj);
            }
            if (Global.m_rightHandObj != null)
            {
                Destroy(Global.m_rightHandObj);
            }
        }

        public void OnTouchpadPress(object sender, ControllerInteractionEventArgs e)
        {
            // 按下后禁止旋转
            mIsRotate = false;
            GamepadDir tempDir = GetGamepadDir(e.touchpadAxis);
            SDK_BaseController.ControllerHand tempHand = GetHandType(e);
            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                EventManager.Instance.DispatchEvent(LogicEventType.PressLeftTouchpad, new EventDataEx<GamepadDir>(tempDir));
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                EventManager.Instance.DispatchEvent(LogicEventType.PressRightTouchpad, new EventDataEx<GamepadDir>(tempDir));
            }
        }

        public void OnTouchpadReleased(object sender, ControllerInteractionEventArgs e)
        {

        }

        public void OnTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
        {
            mIsRotate = true;
            mRotateAngle = e.touchpadAngle;

            GamepadDir tempDir = GetGamepadDir(e.touchpadAxis);
            SDK_BaseController.ControllerHand tempHand = GetHandType(e);
            if (tempHand == SDK_BaseController.ControllerHand.Left)
            {
                EventManager.Instance.DispatchEvent(LogicEventType.StartTouchLeftTouchpad, new EventDataEx<GamepadDir>(tempDir));
            }
            else if (tempHand == SDK_BaseController.ControllerHand.Right)
            {
                EventManager.Instance.DispatchEvent(LogicEventType.StartTouchRightTouchpad, new EventDataEx<GamepadDir>(tempDir));
            }
        }

        public void OnTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
        {

        }

        public void OnTouchPadAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
            if (mIsRotate)
            {
                //Debuger.Log("OnTouchPadAxisChanged" + e.touchpadAngle);

                RotateCamerRig(e.touchpadAngle - mRotateAngle);
                mRotateAngle = e.touchpadAngle;

            }
        }

        // 手柄震动
        public void HandHapticPulse(SDK_BaseController.ControllerHand varHand)
        {
            if (varHand == SDK_BaseController.ControllerHand.Left)
            {
                SteamVR_TrackedObject tempTrack = mLeftController.GetComponentInParent<SteamVR_TrackedObject>();
                SteamVR_Controller.Device device = SteamVR_Controller.Input((int)tempTrack.index);
                device.TriggerHapticPulse(65535);
            }
            else if (varHand == SDK_BaseController.ControllerHand.Right)
            {
                SteamVR_TrackedObject tempTrack = mRightController.GetComponentInParent<SteamVR_TrackedObject>();
                SteamVR_Controller.Device device = SteamVR_Controller.Input((int)tempTrack.index);
                device.TriggerHapticPulse(65535);
            }
        }



        private void RotateCamerRig(float varAngle)
        {
            Transform tempCameraParent = mLeftController.parent.parent;
            if (tempCameraParent != null)
            {
                Transform tempCameraTrans = tempCameraParent.GetChild(2);
                if (tempCameraTrans != null)
                {
                    // 绕摄像机旋转
                    tempCameraParent.RotateAround(tempCameraTrans.position, Vector3.up, varAngle);
                }

                //tempCameraParent.Rotate(varAngle * Vector3.up, Space.Self);
            }
        }

        private void ForceTeleport(EventData varData)
        {
            var tempData = varData as EventDataEx<Transform>;
            Transform tempTrans = tempData.GetData();

            Quaternion tempQ = Quaternion.Euler(tempTrans.eulerAngles);
            mTeleport.ForceTeleport(tempTrans.position, tempQ);
        }

        // 可用角度划分圆盘按键数量
        // 这个函数输入两个二维向量会返回一个夹角 180 到 -180  
        private float VectorAngle(Vector2 from, Vector2 to)
        {
            float angle;
            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            return cross.z > 0 ? -angle : angle;
        }

        private GamepadDir GetGamepadDir(Vector2 target)
        {
            // 例子：圆盘分成上下左右  
            float jiaodu = VectorAngle(Vector2.right, target);
            //Debuger.Log(jiaodu.ToString());
            //下  
            if (jiaodu > 45 && jiaodu < 135)
            {
                Debuger.Log("下");
                return GamepadDir.Down;
            }
            //上  
            if (jiaodu < -45 && jiaodu > -135)
            {
                Debuger.Log("上");
                return GamepadDir.Up;
            }
            //左  
            if ((jiaodu < 180 && jiaodu > 135) || (jiaodu < -135 && jiaodu > -180))
            {
                Debuger.Log("左");
                return GamepadDir.Left;
            }
            //右  
            if ((jiaodu > 0 && jiaodu < 45) || (jiaodu > -45 && jiaodu < 0))
            {
                Debuger.Log("右");
                return GamepadDir.Right;
            }
            Debuger.Log("其他方向");
            return GamepadDir.Other;
        }
    }
}