using System;
using UnityEngine;
using UnityEngine.Events;
using zSpace.Core;

namespace ZSpaceEx
{
    public interface IPenEvent
    {
        /// <summary>
        /// 笔进入物体
        /// </summary>
        void OnPenEnter(GameObject varObj);

        /// <summary>
        /// 笔离开物体
        /// </summary>
        void OnPenExit(GameObject varObj);

        /// <summary>
        /// 笔按钮按下
        /// </summary>
        void OnPenButtonPress(ZCore.TrackerButtonEventInfo info);

        /// <summary>
        /// 笔按钮释放
        /// </summary>
        void OnPenButtonRelease(ZCore.TrackerButtonEventInfo info);

        /// <summary>
        /// 笔按钮点击
        /// </summary>
        void OnPenButtonClick(ZCore.TrackerButtonEventInfo info);

        /// <summary>
        /// 笔抓物体开始
        /// </summary>
        void OnPenGrabObjBegin();

        /// <summary>
        /// 笔抓物体结束
        /// </summary>
        void OnPenGrabObjEnd();

        /// <summary>
        /// 笔正在抓物体
        /// </summary>
        void OnPenGrabObj();
    }

    [Serializable]
    public sealed class PenSimuMouseClick : UnityEvent<RaycastHit> { }
}
