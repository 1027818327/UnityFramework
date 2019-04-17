//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEditor;

namespace zSpace.SimsCommon
{
    [CustomEditor(typeof(WorldAutosizer))]
    public class WorldAutosizerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
}
