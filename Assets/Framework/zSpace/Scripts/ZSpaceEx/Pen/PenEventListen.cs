using Framework.Event;
using Framework.Unity.Event;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using zSpace.Core;

namespace ZSpaceEx
{
    public class PenEventListen : MonoBehaviour
    {
        /*
        public Highlighter mHighlighter;
        */

        [Header("是否打开抓取")]
        public bool mOpenGrab = true;

        [Header("是否能被单独被抓")]
        public bool canGrab = true;
        [Header("连接的物体，整体被抓取拖动")]
        [SerializeField]
        private List<GameObject> connectObjList;
        private bool wholeCanGrab;
        private GameObject mWholeGrabParentObj;

        private bool mIsEnter;
        private int chooseKey = PenMgr.MiddleKeyId;

        [Serializable]
        public sealed class InteractEvent : UnityEvent<GameObject> { }

        public InteractEvent OnPointerEnter = new InteractEvent();
        public InteractEvent OnPointerExit = new InteractEvent();
        public InteractEvent OnPressDown = new InteractEvent();
        public InteractEvent OnPressUp = new InteractEvent();

        /// <summary>
        /// 整体是否能抓取
        /// </summary>
        /// <returns></returns>
        public bool WholeCanGrab()
        {
            if (connectObjList == null)
            {
                return false;
            }
            foreach (var obj in connectObjList)
            {
                if (obj != null && obj.activeSelf == true)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置碰撞体是否开启
        /// </summary>
        /// <param name="show"></param>
        public void SetCollider(bool show)
        {
            var tempCollider = GetComponent<Collider>();
            if (tempCollider != null)
            {
                tempCollider.enabled = show;
            }
        }

        /*
        private void Start()
        {  
            if (mHighlighter == null)
            {
                mHighlighter = GetComponent<Highlighter>();
            }
            
            if (mHighlighter == null)
            {
                mHighlighter = gameObject.AddComponent<Highlighter>();
            }
        }
        */

        private void OnEnable()
        {
            PenMgr.mPointerEnterEvent += PointerEnter;
            PenMgr.mPointerExitEvent += PointerExit;
            PenMgr.mPressedEvent += PressDown;
            PenMgr.mReleasedEvent += PressUp;
            PenMgr.mGrabBeginEvent += GrabBegin;
            PenMgr.mGrabEvent += GrabAction;
            PenMgr.mGrabEndEvent += GrabEnd;
        }

        private void OnDisable()
        {
            if (Application.isPlaying)
            {
                PenMgr.mPointerEnterEvent -= PointerEnter;
                PenMgr.mPointerExitEvent -= PointerExit;
                PenMgr.mPressedEvent -= PressDown;
                PenMgr.mReleasedEvent -= PressUp;
                PenMgr.mGrabBeginEvent -= GrabBegin;
                PenMgr.mGrabEvent -= GrabAction;
                PenMgr.mGrabEndEvent -= GrabEnd;
            }
        }

        private void PointerEnter(GameObject varObj)
        {
            if (varObj.GetInstanceID() == gameObject.GetInstanceID())
            {
                mIsEnter = true;
                
                /*
                if (mHighlighter != null)
                {
                    mHighlighter.ConstantOn();
                }
                */
               
                if(OnPointerEnter != null)
                {
                    OnPointerEnter.Invoke(gameObject);
                }
            }
        }

        private void PointerExit(GameObject varObj)
        {
            if (varObj.GetInstanceID() == gameObject.GetInstanceID())
            {
                mIsEnter = false;
                
                /*
                if (mHighlighter != null)
                {
                    mHighlighter.ConstantOff();
                }
                */
                
                if (OnPointerExit != null)
                {
                    OnPointerExit.Invoke(gameObject);
                }
            }
        }

        private void PressDown(ZCore.TrackerButtonEventInfo info)
        {
            if (mIsEnter)
            {
                if (chooseKey != PenMgr.LeftKeyId && chooseKey != PenMgr.RightKeyId && chooseKey != PenMgr.MiddleKeyId)
                {
                    if (OnPressDown != null)
                    {
                        OnPressDown.Invoke(gameObject);
                    }
                    return;
                }

                if (chooseKey == info.ButtonId)
                {
                    if (OnPressDown != null)
                    {
                        OnPressDown.Invoke(gameObject);
                    }
                    return;
                }
            }
        }

        private void PressUp(ZCore.TrackerButtonEventInfo info)
        {
            if (mIsEnter && chooseKey == info.ButtonId)
            {
                if (chooseKey != PenMgr.LeftKeyId && chooseKey != PenMgr.RightKeyId && chooseKey != PenMgr.MiddleKeyId)
                {
                    if (OnPressUp != null)
                    {
                        OnPressUp.Invoke(gameObject);
                    }
                    return;
                }

                if (chooseKey == info.ButtonId)
                {
                    if (OnPressUp != null)
                    {
                        OnPressUp.Invoke(gameObject);
                    }
                    return;
                }
            }
        }

        private void GrabBegin(GrabObj varGrabObj)
        {
            if (!mOpenGrab)
            {
                return;
            }
            if (varGrabObj.mObj.GetInstanceID() == gameObject.GetInstanceID())
            {
                if (PenMgr._zCore.IsTargetButtonPressed(ZCore.TargetType.Primary, chooseKey) == false)
                {
                    return;
                }

                wholeCanGrab = WholeCanGrab();
                if (wholeCanGrab)
                {
                    if (mWholeGrabParentObj == null)
                    {
                        mWholeGrabParentObj = new GameObject("Temp");
                    }

                    foreach (var obj in connectObjList)
                    {
                        if (obj != null)
                        {
                            obj.transform.SetParent(mWholeGrabParentObj.transform, true);
                        }
                    }
                    transform.SetParent(mWholeGrabParentObj.transform, true);
                    LocalEventManager.GetInstance().UintDispatcher.DispatchEvent(LogicEventType.PenWholeGrab, new EventDataEx<GameObject>(mWholeGrabParentObj));
                }
            }
        }

        private void GrabAction(GrabObj varGrabObj)
        {
            if (!mOpenGrab)
            {
                return;
            }

            if (varGrabObj.mObj.GetInstanceID() != gameObject.GetInstanceID())
            {
                return;
            }

            if (PenMgr._zCore.IsTargetButtonPressed(ZCore.TargetType.Primary, chooseKey) == false)
            {
                return;
            }

            if (canGrab)
            {
                transform.position = varGrabObj.mPos;
                transform.rotation = varGrabObj.mQuaternion;
            }
            else
            {
                // 不能被单独拖动的情况下判断能不能整体拖动
                if (wholeCanGrab)
                {
                    Transform tempTrans = mWholeGrabParentObj.transform;
                    tempTrans.position = varGrabObj.mPos;
                    tempTrans.rotation = varGrabObj.mQuaternion;
                }
            }
        }

        private void GrabEnd(GrabObj varGrabObj)
        {
            if (!mOpenGrab)
            {
                return;
            }

            if (varGrabObj.mObj.GetInstanceID() != gameObject.GetInstanceID())
            {
                return;
            }

            if (mWholeGrabParentObj != null)
            {
                if (connectObjList != null)
                {
                    foreach (var obj in connectObjList)
                    {
                        if (obj != null)
                        {
                            obj.transform.SetParent(null, true);
                        }
                    }
                }
                transform.SetParent(null, true);

                Destroy(mWholeGrabParentObj);
                mWholeGrabParentObj = null;
            }
        }

        /// <summary>
        /// 添加连接物
        /// </summary>
        /// <param name="varObj"></param>
        public void AddConnectObj(GameObject varObj)
        {
            if (connectObjList == null)
            {
                connectObjList = new List<GameObject>();
            }
            if (!connectObjList.Contains(varObj))
            {
                connectObjList.Add(varObj);
            }
        }

        /// <summary>
        /// 移除连接物
        /// </summary>
        /// <param name="varObj"></param>
        public void RemoveConnectObj(GameObject varObj)
        {
            if (connectObjList != null)
            {
                connectObjList.Remove(varObj);
            }
        }

        public void RemoveAllConnectObj()
        {
            if (connectObjList != null)
            {
                connectObjList.Clear();
            }
        }

        public void ChangeChooseKey(int id)
        {
            chooseKey = id;
        }

        public static PenEventListen SetCanDrag(GameObject varObj, bool canGrab)
        {
            PenEventListen tempP = varObj.GetComponent<PenEventListen>();
            if (tempP != null)
            {
                tempP.canGrab = canGrab;
            }

            return tempP;
        }

        public static PenEventListen Get(GameObject varObj)
        {
            if (varObj == null)
            {
                return null;
            }
            var tempP = varObj.GetComponent<PenEventListen>();
            if (tempP == null)
            {
                tempP = varObj.AddComponent<PenEventListen>();
            }
            return tempP;
        }
    }
}