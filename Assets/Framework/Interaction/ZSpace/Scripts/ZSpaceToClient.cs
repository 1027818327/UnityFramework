using DG.Tweening;
using Framework.Event;
using Framework.Unity.Interaction;
using Framework.Unity.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using zSpace.Core;
using zSpace.SimsCommon;
using ZSpaceEx;

namespace Framework.Interaction
{
    public class ZSpaceToClient : MonoBehaviour, IPenEvent
    {
        public ZCore _zCore = null;

        public List<Material> endBallMaterials;

        public SampleStylusBeamRaycaster mStylusBeamRaycaster;

        public Camera m_mainCamera;
        private ZCore.Pose mCurPose;
        //碰撞信息
        private RaycastHit mHit;
        //点击进入的物体
        private GameObject mPointerEnterObj;
        private ZCore.Pose mPressPos; // 笔按下的当前世界坐标系位置
                                      //是否可以传送
        private bool _canBlink = true;

        //是否开始射线检测
        private bool _isAllowRayCash = true;
        //是否允许操作
        private bool m_isAllowOperate = true;
        //private TweenBase m_tweenPosition;
        //保持的对像
        private GameObject _hoverGameobject;

        //笔状态
        private StylusState _stylusState = StylusState.Idle;
        //笔的渲染队列
        private static readonly int StylusRenderQueue = 9000;
        // 以握笔方向0代表中键， 1代表右键，2代表左键
        public const int LeftKeyId = 2;
        public const int MiddleKeyId = 0;
        public const int RightKeyId = 1;
        //用与计算拖动
        private Vector3 _startPointPos;

        private Vector3 _hitPosition;

        private GrabObj mGrabObj;  // 当前被拖拽物体信息
        private Vector3 _initialGrabOffset = Vector3.zero;
        private Quaternion _initialGrabRotation = Quaternion.identity;
        private float _initialGrabDistance = 0.0f;
        // Use this for initialization
        void Start()
        {
            var tempEventEngine = GlobalEventManager.GetInstance();
            if (tempEventEngine != null)
            {
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_HERO_MOVE, OnHeroMove);
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_STOP_HERO_MOVE, OnHeroStopMove);
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_HERO_OPERATE_STATE, OnHeroOperateState);
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_CAMERA_SHAKE, OnCameraShake);
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_RAYCASH_ISOPEN, OnRaycashIsOpen);
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_RAYCASH_CHANGE, OnRaycashChange);

            }

            if (mStylusBeamRaycaster == null)
            {
                mStylusBeamRaycaster = GetComponentInChildren<SampleStylusBeamRaycaster>();
            }
            //线段初始化
            LineInit();

            Global.mLeftKey.id = LeftKeyId;
            Global.mMiddleKey.id = MiddleKeyId;
            Global.mRightKey.id = RightKeyId;

            _zCore.TargetButtonPress += HandleButtonPress;
            _zCore.TargetButtonRelease += HandleButtonRelease;

            //设置mainCamera
            GlobalQualitySetting.GetInstance().SetMainCamera(m_mainCamera);
            CameraUtils.MainCam = m_mainCamera;
            //发送相机姿态事件
            SEventCameraPose pose = new SEventCameraPose();
            pose.HeadCamera = CameraUtils.MainCam.transform;
            if (tempEventEngine != null)
            {
                tempEventEngine.UintDispatcher.DispatchEvent(DGlobalEvent.EVENT_CAMERA_POSE, new EventDataEx<SEventCameraPose>(pose));
            }
        }

        private void OnDestroy()
        {
            var tempEventEngine = GlobalEventManager.GetInstance();
            if (tempEventEngine != null)
            {
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_HERO_MOVE, OnHeroMove);
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_STOP_HERO_MOVE, OnHeroStopMove);
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_HERO_OPERATE_STATE, OnHeroOperateState);
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_CAMERA_SHAKE, OnCameraShake);
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_RAYCASH_ISOPEN, OnRaycashIsOpen);
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_RAYCASH_CHANGE, OnRaycashChange);
            }
        }

        /// <summary>
        /// 线初始化
        /// </summary>
        private void LineInit()
        {
            foreach (var material in endBallMaterials)
            {
                material.renderQueue = StylusRenderQueue;
            }
        }

        // Update is called once per frame
        void Update()
        {
            Global.mLeftKey.isPressed = _zCore.IsTargetButtonPressed(ZCore.TargetType.Primary, Global.mLeftKey.id);
            Global.mMiddleKey.isPressed = _zCore.IsTargetButtonPressed(ZCore.TargetType.Primary, Global.mMiddleKey.id);
            Global.mRightKey.isPressed = _zCore.IsTargetButtonPressed(ZCore.TargetType.Primary, Global.mRightKey.id);

            ZCore.Pose pose = _zCore.GetTargetPose(ZCore.TargetType.Primary, ZCore.CoordinateSpace.World);

            mCurPose = pose;

            // 当状态是idel时，检查有没有进入到物体
            if (_stylusState == StylusState.Idle)
            {
                RaycastHit hit;
                var raycastDistance = (mStylusBeamRaycaster.BeamLength + mStylusBeamRaycaster.HelperLength) * _zCore.ViewerScale;
                if (Physics.Raycast(pose.Position, pose.Direction, out hit, raycastDistance))
                {
                    if (mPointerEnterObj != hit.collider.gameObject)
                    {
                        OnPenExit(mPointerEnterObj);
                        mPointerEnterObj = hit.collider.gameObject;
                        _hitPosition = hit.point;
                        OnPenEnter(mPointerEnterObj);
                    }
                }
                else
                {
                    OnPenExit(mPointerEnterObj);
                }
                mHit = hit;
            }
            if (mPointerEnterObj != null)
            {
                if (_stylusState == StylusState.Press)
                {
                    var tempPos = pose.Position - mPressPos.Position;
                    if (Mathf.Approximately(tempPos.x, 0) && Mathf.Approximately(tempPos.y, 0) && Mathf.Approximately(tempPos.z, 0))
                    {

                    }
                    else
                    {
                        if (mPointerEnterObj != null)
                        {
                            BeginGrab(mPointerEnterObj, mHit.distance, pose.Position, pose.Rotation);
                            mGrabObj.mObj = mPointerEnterObj;
                            OnPenGrabObjBegin();
                        }
                    }
                }
                else if (_stylusState == StylusState.Grab)
                {
                    UpdateGrab(pose.Position, pose.Rotation);
                }
            }
        }

        private void BeginGrab(GameObject hitObject, float hitDistance, Vector3 inputPosition, Quaternion inputRotation)
        {
            Vector3 inputEndPosition = inputPosition + (inputRotation * (Vector3.forward * hitDistance));

            // Cache the initial grab state.
            _initialGrabOffset = Quaternion.Inverse(hitObject.transform.rotation) * (hitObject.transform.position - inputEndPosition);
            _initialGrabRotation = Quaternion.Inverse(inputRotation) * hitObject.transform.rotation;
            _initialGrabDistance = hitDistance;
        }

        private void UpdateGrab(Vector3 inputPosition, Quaternion inputRotation)
        {
            Vector3 inputEndPosition = inputPosition + (inputRotation * (Vector3.forward * _initialGrabDistance));

            // Update the grab object's rotation.
            Quaternion objectRotation = inputRotation * _initialGrabRotation;
            // Update the grab object's position.
            Vector3 objectPosition = inputEndPosition + (objectRotation * _initialGrabOffset);

            //mGrabObj.mObj.transform.rotation = objectRotation;
            //mGrabObj.mObj.transform.position = objectPosition;

            mGrabObj.mPos = objectPosition;
            mGrabObj.mQuaternion = objectRotation;
            OnPenGrabObj();
        }

        /// <summary>
        /// 笔进入物体
        /// </summary>
        public void OnPenEnter(GameObject varObj)
        {
            if (!_isAllowRayCash) return;
            if (!m_isAllowOperate) return;

            IEventCallback callback = varObj.GetComponent<IEventCallback>();
            if (callback != null)
            {
                RayEventCallBackParameter param = new RayEventCallBackParameter();
                //param.CurrentPoint = e.raycastHit.point;
                param.Detaly = Vector3.zero;
                callback.OnRayEventMessage(RayEventType.EventEnter, param);
                _hoverGameobject = varObj;
            }
        }
        /// <summary>
        /// 笔离开物体
        /// </summary>
        public void OnPenExit(GameObject varObj)
        {
            if (!_isAllowRayCash) return;
            if (!m_isAllowOperate) return;
            if (varObj == null) return;
            IEventCallback callback = varObj.GetComponent<IEventCallback>();
            if (callback != null)
            {
                RayEventCallBackParameter param = new RayEventCallBackParameter();
                //param.CurrentPoint = e.raycastHit.point;
                param.Detaly = Vector3.zero;
                callback.OnRayEventMessage(RayEventType.EventExit, param);
                _hoverGameobject = null;
            }
        }
        /// <summary>
        /// 笔按钮按下
        /// </summary>
        public void OnPenButtonPress(ZCore.TrackerButtonEventInfo info)
        {
            if (info.ButtonId == Global.mLeftKey.id)
            {
                Global.mLeftKey.isPressed = true;
            }
            else if (info.ButtonId == Global.mMiddleKey.id)
            {
                Global.mMiddleKey.isPressed = true;

            }
            else if (info.ButtonId == Global.mRightKey.id)
            {
                Global.mRightKey.isPressed = true;
            }
        }
        /// <summary>
        /// 笔按钮释放
        /// </summary>
        public void OnPenButtonRelease(ZCore.TrackerButtonEventInfo info)
        {
            if (info.ButtonId == Global.mLeftKey.id)
            {
                Global.mLeftKey.isPressed = false;
            }
            else if (info.ButtonId == Global.mMiddleKey.id)
            {
                Global.mMiddleKey.isPressed = false;
                if (!_isAllowRayCash) return;
                _startPointPos = Vector3.zero;
                //_triggerPressDown = false;
                if (!m_isAllowOperate) return;
                if (_hoverGameobject != null)
                {
                    IEventCallback callback = _hoverGameobject.GetComponent<IEventCallback>();
                    if (callback != null)
                    {
                        RayEventCallBackParameter param = new RayEventCallBackParameter(); 
                        param.CurrentPoint = Vector3.zero;
                        param.Detaly = Vector3.zero;
                        callback.OnRayEventMessage(RayEventType.EventRelease, param);
                    }
                }
            }
            else if (info.ButtonId == Global.mRightKey.id)
            {
                Global.mRightKey.isPressed = false;
            }
        }
        /// <summary>
        /// 笔按钮点击
        /// </summary>
        public void OnPenButtonClick(ZCore.TrackerButtonEventInfo info)
        {
            if (!m_isAllowOperate) return;
            if (info.ButtonId == Global.mLeftKey.id)
            {
                return;
            }
            else if (info.ButtonId == Global.mRightKey.id)
            {
                return;
            }
            if (_hoverGameobject == null || !_hoverGameobject.gameObject.activeSelf)
            {
                SEventOPPress opPress = new SEventOPPress();
                opPress.nPressObj = null;
                GlobalEventManager.GetInstance().UintDispatcher.DispatchEvent(DGlobalEvent.EVENT_OP_PRESS, new EventDataEx<SEventOPPress>(opPress));
            }
            else
            {
                IEventCallback callback = _hoverGameobject.GetComponent<IEventCallback>();
                if (callback != null)
                {
                    RayEventCallBackParameter param = new RayEventCallBackParameter(); 
                    param.CurrentPoint = _hitPosition;
                    param.Detaly = Vector3.zero;
                    param.isLeftHand = false;
                    callback.OnRayEventMessage(RayEventType.EventPress, param);
                }
                else
                {
                    SEventOPPress opPress = new SEventOPPress();
                    opPress.nPressObj = null;
                    GlobalEventManager.GetInstance().UintDispatcher.DispatchEvent(DGlobalEvent.EVENT_OP_PRESS, new EventDataEx<SEventOPPress>(opPress));
                }
            }
        }
        /// <summary>
        /// 笔抓物体开始
        /// </summary>
        public void OnPenGrabObjBegin()
        {
            _stylusState = StylusState.Grab;
        }
        /// <summary>
        /// 笔抓物体结束
        /// </summary>
        public void OnPenGrabObjEnd()
        {

        }
        /// <summary>
        /// 笔正在抓物体
        /// </summary>
        public void OnPenGrabObj()
        {
            if (!_isAllowRayCash) return;
            if (!m_isAllowOperate) return;
            Debug.LogError("OnPenGrabObj:" + mGrabObj.mPos);
            IEventCallback callback = mGrabObj.mObj.GetComponent<IEventCallback>();
            if (callback != null)
            {
                RayEventCallBackParameter param = new RayEventCallBackParameter();
                //首次按下初始化在拖动时赋值，因按下处和此处取到的坐标不同/
                if (_startPointPos == Vector3.zero)
                    _startPointPos = mGrabObj.mPos;
                param.CurrentPoint = mGrabObj.mPos;
                _hitPosition = mGrabObj.mPos;
                param.Detaly = mGrabObj.mPos - _startPointPos;
                float x = (float)(Math.Round(param.Detaly.x * 10000)) / 10000;
                float y = (float)(Math.Round(param.Detaly.y * 10000)) / 10000;
                float z = (float)(Math.Round(param.Detaly.z * 10000)) / 10000;
                param.Detaly = new Vector3(x, y, z);
                param.isLeftHand = false;
                callback.OnRayEventMessage(RayEventType.EventHover, param);
                _startPointPos = mGrabObj.mPos;
            }
        }

        /// <summary>
        /// 处理button press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="info"></param>
        private void HandleButtonPress(ZCore sender, ZCore.TrackerButtonEventInfo info)
        {
            if (!m_isAllowOperate) return;
            _stylusState = StylusState.Press;
            mPressPos = info.WorldPose;
            OnPenButtonPress(info);
        }
        /// <summary>
        /// 处理button Release
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="info"></param>
        private void HandleButtonRelease(ZCore sender, ZCore.TrackerButtonEventInfo info)
        {
            if (!m_isAllowOperate) return;
            OnPenButtonRelease(info);
            if (_stylusState == StylusState.Press && info.WorldPose.Position.ToString() == mPressPos.Position.ToString())
            {
                OnPenButtonClick(info);
            }

            if (_stylusState == StylusState.Grab)
            {
                OnPenGrabObjEnd();
            }

            _stylusState = StylusState.Idle;
        }

        private void OnHeroMove(EventData varData)
        {
            var pContext = varData as EventDataEx<SEventHeroMove>;
            if (pContext == null)
            {
                return;
            }
            SEventHeroMove heroMove = pContext.GetData();
            //zSpace默认位置没有高度
            heroMove.vTragetPos.y += 1.7f;

            if (heroMove.bAnimation)
            {
                var tempTween = transform.parent.DOMove(heroMove.vTragetPos, heroMove.fAnimationDuration);
                tempTween.onComplete += delegate ()
                {
                    if (heroMove.CallBack != null)
                    {
                        heroMove.CallBack();
                    }
                }; 
            }
            else
            {
                transform.parent.position = heroMove.vTragetPos;
                transform.parent.eulerAngles = heroMove.vRotation;
                if (heroMove.CallBack != null)
                {
                    heroMove.CallBack();
                }
            }
        }

        private void OnHeroStopMove(EventData varData)
        {
            transform.parent.DOKill();
        }

        private void OnHeroOperateState(EventData varData)
        {
            var pContext = varData as EventDataEx<SEventHeroOperate>;
            if (pContext == null)
            {
                return;
            }
            SEventHeroOperate op = pContext.GetData();

            m_isAllowOperate = op.isAllowOperate;
        }


        private void OnCameraShake(EventData varData)
        {
            var pContext = varData as EventDataEx<SEventCameraShake>;
            if (pContext == null)
            {
                return;
            }
            transform.parent.gameObject.AddComponent<CameraShake>();        //相机抖动
        }

        private void OnRaycashIsOpen(EventData varData)
        {
            var pContext = varData as EventDataEx<SEventCameraShake>;
            if (pContext == null)
            {
                return;
            }
            transform.parent.gameObject.AddComponent<CameraShake>();        //相机抖动
        }

        private void OnRaycashChange(EventData varData)
        {
            var pContext = varData as EventDataEx<SEventRayInfo>;
            if (pContext == null)
            {
                return;
            }
            SEventRayInfo tempInfo = pContext.GetData();
            ControlRayLength(tempInfo);
        }

        private void ControlRayLength(SEventRayInfo info)
        {
            mStylusBeamRaycaster.HelperLength = info.length;
        }
    }
}
