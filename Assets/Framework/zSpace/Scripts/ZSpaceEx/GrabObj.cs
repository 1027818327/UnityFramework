using UnityEngine;

namespace ZSpaceEx
{
    public struct GrabObj
    {
        public GameObject mObj;  // 被拖拽的物体
        public Quaternion mQuaternion;  // 四元数角度
        public Vector3 mPos;  // 世界坐标
    }
}
