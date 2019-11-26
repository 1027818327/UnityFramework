
/// <summary>
/// InputToClient.cs
/// </summary>
/// <remarks>
/// 2019/11/26: 创建. 陈伟超 <br/>
/// </remarks>

using Framework.Event;
using Framework.Unity.Render;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Unity.Interaction
{
    public class InputToClient : MonoBehaviour
    {
        private GameObject _hoverGameobject;
        private GameObject _dragGameObject;

        private bool _triggerPressDown = false;

        //是否允许操作
        private bool m_isAllowOperate = true;
        //是否开始射线检测
        private bool _isAllowRayCash = true;

        private bool mMouseDown = false;
        private IEventCallback mHoverObjCallback;
        private Vector3 mDragPos;
        private Vector3 lastMousePos;
        //射线长度
        private float mRayLength = 20;


        private void Start()
        {
            var tempEventEngine = GlobalEventManager.GetInstance();
            if (tempEventEngine != null)
            {
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_HERO_MOVE, OnHeroMove);
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_HERO_OPERATE_STATE, OnHeroOperateState);
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_CAMERA_SHAKE, OnCameraShake);
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_RAYCASH_ISOPEN, OnRaycashIsOpen);
                tempEventEngine.UintDispatcher.AddEventListener(DGlobalEvent.EVENT_RAYCASH_CHANGE, OnRaycashChange);
            }

            CameraUtils.MainCam = Camera.main;

            //设置mainCamera
            //GlobalQualitySetting.Instance.SetMainCamera(CameraUtils.MainCam);
            //发送相机姿态事件
            SEventCameraPose pose = new SEventCameraPose();
            pose.HeadCamera = CameraUtils.MainCam.transform;
            tempEventEngine.UintDispatcher.DispatchEvent(DGlobalEvent.EVENT_CAMERA_POSE, new EventDataEx<SEventCameraPose>(pose));
        }

        private void OnDestroy()
        {
            var tempEventEngine = GlobalEventManager.GetInstance();
            if (tempEventEngine != null)
            {
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_HERO_MOVE, OnHeroMove);
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_HERO_OPERATE_STATE, OnHeroOperateState);
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_CAMERA_SHAKE, OnCameraShake);
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_RAYCASH_ISOPEN, OnRaycashIsOpen);
                tempEventEngine.UintDispatcher.RemoveEventListener(DGlobalEvent.EVENT_RAYCASH_CHANGE, OnRaycashChange);
            }
        }

        private void Update()
        {
            if (!m_isAllowOperate) return;
            if (!_isAllowRayCash) return;
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
#if UNITY_ANDROID || UNITY_IPHONE
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
                if (EventSystem.current.IsPointerOverGameObject())
#endif
                {
                    //Debug.Log("当前触摸在UI上");
                    if (mHoverObjCallback != null)
                    {
                        RayEventCallBackParameter param = new RayEventCallBackParameter();

                        param.Detaly = Vector3.zero;
                        mHoverObjCallback.OnRayEventMessage(RayEventType.EventRelease, param);
                        //Debug.Log(string.Format("EventRelease{0}", _hoverGameobject.name));
                    }
                    else
                    {
                        SEventOPPress opPress = new SEventOPPress();
                        opPress.nPressObj = null;

                        GlobalEventManager.GetInstance().UintDispatcher.DispatchEvent(DGlobalEvent.EVENT_OP_PRESS, new EventDataEx<SEventOPPress>(opPress));
                    }

                    _hoverGameobject = null;
                    _dragGameObject = null;
                    mHoverObjCallback = null;
                }
                else
                {
                    //Debug.Log("当前没有触摸在UI上");
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                //TRACE.TraceLn("MouseDown");

                mMouseDown = true;

                _triggerPressDown = true;
                if (_hoverGameobject == null)
                {
                    SEventOPPress opPress = new SEventOPPress();
                    opPress.nPressObj = null;
                    GlobalEventManager.GetInstance().UintDispatcher.DispatchEvent(DGlobalEvent.EVENT_OP_PRESS, new EventDataEx<SEventOPPress>(opPress));
                }
                else
                {
                    _dragGameObject = _hoverGameobject;
                    IEventCallback callback = _hoverGameobject.GetComponent<IEventCallback>();
                    mHoverObjCallback = callback;
                    if (callback != null)
                    {
                        RayEventCallBackParameter param = new RayEventCallBackParameter();

                        param.CurrentPoint = _hoverGameobject.transform.position;
                        param.Detaly = Vector3.zero;
                        callback.OnRayEventMessage(RayEventType.EventPress, param);
                        //Debug.Log(string.Format("EventPress{0}", _hoverGameobject.name));

                        lastMousePos = Input.mousePosition;

                        mDragPos = CameraUtils.MainCam.ScreenToViewportPoint(lastMousePos) - CameraUtils.MainCam.WorldToViewportPoint(param.CurrentPoint);
                    }
                    else
                    {
                        SEventOPPress opPress = new SEventOPPress();
                        opPress.nPressObj = null;
                        GlobalEventManager.GetInstance().UintDispatcher.DispatchEvent(DGlobalEvent.EVENT_OP_PRESS, new EventDataEx<SEventOPPress>(opPress));
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                //TRACE.TraceLn("MouseUp");

                mMouseDown = false;

                _triggerPressDown = false;

                if (mHoverObjCallback != null)
                {
                    RayEventCallBackParameter param = new RayEventCallBackParameter();

                    param.Detaly = Vector3.zero;
                    mHoverObjCallback.OnRayEventMessage(RayEventType.EventRelease, param);
                    //Debug.Log(string.Format("EventRelease{0}", _hoverGameobject.name));
                }
                else
                {
                    SEventOPPress opPress = new SEventOPPress();
                    opPress.nPressObj = null;
                    GlobalEventManager.GetInstance().UintDispatcher.DispatchEvent(DGlobalEvent.EVENT_OP_PRESS, new EventDataEx<SEventOPPress>(opPress));
                }

                _hoverGameobject = null;
                _dragGameObject = null;
                mHoverObjCallback = null;
            }

            if (mMouseDown && _dragGameObject != null && mHoverObjCallback != null)
            {
                if (lastMousePos.Equals(Input.mousePosition) == false)
                {
                    RayEventCallBackParameter param = new RayEventCallBackParameter();

                    param.Detaly = Input.mousePosition - lastMousePos;
                    param.CurrentPoint = CameraUtils.MainCam.ViewportToWorldPoint(CameraUtils.MainCam.ScreenToViewportPoint(Input.mousePosition) - mDragPos);
                    mHoverObjCallback.OnRayEventMessage(RayEventType.EventDrag, param);

                    lastMousePos = Input.mousePosition;
                    //Debug.Log(string.Format("EventDrag{0}", _hoverGameobject.name));
                }
            }

            Ray ray = CameraUtils.MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, mRayLength))
            {
                if (Application.isEditor)
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                }

                GameObject tempHitObj = hit.transform.gameObject;
                if (_hoverGameobject != null && _hoverGameobject != tempHitObj)
                {

                    if (_dragGameObject != _hoverGameobject)
                    {
                        SendExitEvent(_hoverGameobject);
                    }


                    SendEnterEvent(tempHitObj);
                }
                else
                {
                    if (_hoverGameobject != tempHitObj)
                    {
                        SendEnterEvent(tempHitObj);
                    }
                }

                // 发送进入事件
                _hoverGameobject = tempHitObj;

            }
            else
            {
                if (_dragGameObject == null)
                {
                    SendExitEvent(_hoverGameobject);
                }

                _hoverGameobject = null;
            }
        }

        private void SendEnterEvent(GameObject varObj)
        {
            if (varObj == null)
            {
                return;
            }

            // 发送进入事件
            IEventCallback callback = varObj.GetComponent<IEventCallback>();
            if (callback != null)
            {
                RayEventCallBackParameter param = new RayEventCallBackParameter();
                param.CurrentPoint = varObj.transform.position;
                param.Detaly = Vector3.zero;
                callback.OnRayEventMessage(RayEventType.EventEnter, param);
            }
        }

        private void SendExitEvent(GameObject varObj)
        {
            if (varObj == null)
            {
                return;
            }

            // 发送离开事件
            IEventCallback callback = varObj.GetComponent<IEventCallback>();
            if (callback != null)
            {
                RayEventCallBackParameter param = new RayEventCallBackParameter();
                param.CurrentPoint = varObj.transform.position;
                param.Detaly = Vector3.zero;
                callback.OnRayEventMessage(RayEventType.EventExit, param);
                //Debug.Log(string.Format("SendExitEvent{0}", varObj.name));
            }
        }

        private void OnHeroMove(EventData varData)
        {
            var pContext = varData as EventDataEx<SEventHeroMove>;
            if (pContext == null)
            {
                return;
            }
            SEventHeroMove heroMove = pContext.GetData();

            if (heroMove.bAnimation)
            {
                //TweenPosition.Tween<TweenPosition>(transform.parent.gameObject, transform.parent.position, heroMove.vTragetPos, heroMove.fAnimationDuration);
            }
            else
            {
                transform.parent.position = heroMove.vTragetPos;
                transform.parent.eulerAngles = heroMove.vRotation;
            }
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
            _triggerPressDown = false;
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
            mRayLength = info.length;
        }
    }
}