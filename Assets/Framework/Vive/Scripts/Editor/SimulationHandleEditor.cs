using UnityEditor;
using UnityEngine;

namespace Vive
{
    //自定义Tset脚本
    [CustomEditor(typeof(SimulationHandle))]
    //在编辑模式下执行脚本，这里用处不大可以删除。
    [ExecuteInEditMode]
    //请继承Editor
    public class SimulationHandleEditor : Editor
    {
        //在这里方法中就可以绘制面板。
        public override void OnInspectorGUI()
        {
            //得到SimulationHandle对象
            SimulationHandle test = (SimulationHandle)target;
            test.LeftObj = EditorGUILayout.ObjectField("左手手持道具", test.LeftObj, typeof(GameObject), true) as GameObject;
            test.RightObj = EditorGUILayout.ObjectField("右手手持道具", test.RightObj, typeof(GameObject), true) as GameObject;
        }
    }
}
