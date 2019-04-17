//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2016 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////
//#define develop


using Framework.Debugger;
using Framework.Event;
using Framework.Unity.Event;
using System;
using System.Collections.Generic;
using UnityEngine;
using zSpace.Core;

namespace ZSpaceEx
{
    public class PenMgr : MonoBehaviour, IPenEvent
    {
        #region 公共成员
        public List<Material> endBallMaterials;

        public zSpace.SimsCommon.SampleStylusBeamRaycaster mStylusBeamRaycaster;

        // 以握笔方向0代表中键， 1代表右键，2代表左键
        public const int LeftKeyId = 2;
        public const int MiddleKeyId = 0;
        public const int RightKeyId = 1;
        public const int AnyKeyId = -1;

        public static ZCore _zCore = null;
        public static readonly int StylusRenderQueue = 9000;

        public static Action<GameObject> mPointerEnterEvent;
        public static Action<GameObject> mPointerExitEvent;
        public static Action<ZCore.TrackerButtonEventInfo> mPressedEvent;
        public static Action<ZCore.TrackerButtonEventInfo> mReleasedEvent;
        public static Action<ZCore.TrackerButtonEventInfo> mClickEvent;
        public static Action<GrabObj> mGrabBeginEvent;
        public static Action<GrabObj> mGrabEndEvent;
        public static Action<GrabObj> mGrabEvent;
        #endregion


        #region 私有成员

        private StylusState _stylusState = StylusState.Idle;
        private Vector3 _initialGrabOffset = Vector3.zero;
        private Quaternion _initialGrabRotation = Quaternion.identity;
        private float _initialGrabDistance = 0.0f;

        private ZCore.Pose mCurPose;
        private RaycastHit mHit;

        private GameObject mPointerEnterObj;
        private GrabObj mGrabObj;  // 当前被拖拽物体信息
        private ZCore.Pose mPressPos; // 笔按下的当前世界坐标系位置

        #endregion

        #region 公共属性
        public GrabObj GrabObj
        {
            get
            {
                return mGrabObj;
            }
        }
        #endregion

        private void Start()
        {
            _zCore = GameObject.FindObjectOfType<ZCore>();
            if (_zCore == null)
            {
                Debuger.LogError("Unable to find reference to zSpace.Core.Core Monobehaviour.");
                this.enabled = false;
                return;
            }

            if (mStylusBeamRaycaster == null)
            {
                mStylusBeamRaycaster = GetComponentInChildren<zSpace.SimsCommon.SampleStylusBeamRaycaster>();
            }

            LineInit();

            Global.mLeftKey.id = LeftKeyId;
            Global.mMiddleKey.id = MiddleKeyId;
            Global.mRightKey.id = RightKeyId;

            _zCore.TargetButtonPress += HandleButtonPress;
            _zCore.TargetButtonRelease += HandleButtonRelease;

            LocalEventManager.GetInstance().UintDispatcher.AddEventListener(LogicEventType.PenWholeGrab, BeginWholeGrab);
        }

        private void OnDestroy()
        {
            _zCore.TargetButtonPress -= HandleButtonPress;
            _zCore.TargetButtonRelease -= HandleButtonRelease;

            LocalEventManager.GetInstance().UintDispatcher.RemoveEventListener(LogicEventType.PenWholeGrab, BeginWholeGrab);
        }

        private void LineInit()
        {
            foreach (var material in endBallMaterials)
            {
                material.renderQueue = StylusRenderQueue;
            }
        }

        void Update()
        {
            //if (Time.frameCount % 2 == 0)
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
        }

        private void BeginGrab(GameObject hitObject, float hitDistance, Vector3 inputPosition, Quaternion inputRotation)
        {
            Vector3 inputEndPosition = inputPosition + (inputRotation * (Vector3.forward * hitDistance));

            // Cache the initial grab state.
            _initialGrabOffset = Quaternion.Inverse(hitObject.transform.rotation) * (hitObject.transform.position - inputEndPosition);
            _initialGrabRotation = Quaternion.Inverse(inputRotation) * hitObject.transform.rotation;
            _initialGrabDistance = hitDistance;
        }

        private void BeginWholeGrab(EventData note)
        {
            var tempData = note as EventDataEx<GameObject>;
            if (tempData == null)
            {
                return;
            }
            GameObject tempObj = tempData.GetData();
            BeginGrab(tempObj, mHit.distance, mCurPose.Position, mCurPose.Rotation);
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

        private void HandleButtonPress(ZCore sender, ZCore.TrackerButtonEventInfo info)
        {
            _stylusState = StylusState.Press;
            mPressPos = info.WorldPose;
            OnPenButtonPress(info);
        }

        private void HandleButtonRelease(ZCore sender, ZCore.TrackerButtonEventInfo info)
        {            
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

        public void OnPenEnter(GameObject varObj)
        {
            if (mPointerEnterEvent != null)
            {
                mPointerEnterEvent(varObj);
            }
#if develop
            Debuger.Log("进入" + varObj.name);
#endif
        }

        public void OnPenExit(GameObject varObj)
        {

            if (varObj != null)
            {
#if develop
                Debuger.Log("离开" + varObj.name);
#endif
                if (mPointerExitEvent != null)
                {
                    mPointerExitEvent(varObj);
                }
                mPointerEnterObj = null;
            }
        }

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

#if develop
            if (info.ButtonId == Global.mLeftKey.id)
            {
                Debuger.Log("左键按下");
            }
            else if (info.ButtonId == Global.mMiddleKey.id)
            {
                Debuger.Log("中键按下");
            }
            else if (info.ButtonId == Global.mRightKey.id)
            {
                Debuger.Log("右键按下");
            }
#endif

            if (mPressedEvent != null)
            {
                mPressedEvent(info);
            }
        }

        public void OnPenButtonRelease(ZCore.TrackerButtonEventInfo info)
        {
            if (info.ButtonId == Global.mLeftKey.id)
            {
                Global.mLeftKey.isPressed = false;
            }
            else if (info.ButtonId == Global.mMiddleKey.id)
            {
                Global.mMiddleKey.isPressed = false;
            }
            else if (info.ButtonId == Global.mRightKey.id)
            {
                Global.mRightKey.isPressed = false;
            }

#if develop
            if (info.ButtonId == Global.mLeftKey.id)
            {
                Debuger.Log("左键抬起");
            }
            else if (info.ButtonId == Global.mMiddleKey.id)
            {
                Debuger.Log("中键抬起");
            }
            else if (info.ButtonId == Global.mRightKey.id)
            {
                Debuger.Log("右键抬起");
            }
#endif

            if (mReleasedEvent != null)
            {
                mReleasedEvent(info);
            }
        }

        public void OnPenButtonClick(ZCore.TrackerButtonEventInfo info)
        {
#if develop
            Debuger.Log("触发点击");
#endif
            if (mClickEvent != null)
            {
                mClickEvent(info);
            }
        }

        public void OnPenGrabObjBegin()
        {
#if develop
            Debuger.Log("笔抓取开始");
#endif
            _stylusState = StylusState.Grab;
            if (mGrabBeginEvent != null)
            {
                mGrabBeginEvent(mGrabObj);
            }
        }

        public void OnPenGrabObjEnd()
        {
#if develop
            Debuger.Log("笔抓取结束");
#endif
            if (mGrabEndEvent != null)
            {
                mGrabEndEvent(mGrabObj);
            }
        }

        public void OnPenGrabObj()
        {
#if develop
            //Debuger.Log("笔抓取中");
#endif
            if (mGrabEvent != null)
            {
                mGrabEvent(mGrabObj);
            }
        }
    }

    public enum StylusState
    {
        Idle = 0,  // 笔空闲状态
        Press = 1,  // 笔按下按钮状态
        Grab = 2, // 笔抓物体状态
    }
}