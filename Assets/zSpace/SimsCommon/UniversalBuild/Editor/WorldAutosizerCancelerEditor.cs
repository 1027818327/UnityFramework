//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;
using zSpace.Core;

namespace zSpace.SimsCommon
{
    [CustomEditor(typeof(WorldAutosizerCanceler))]
    public class WorldAutosizerCancelerEditor : Editor
    {
        public void OnEnable()
        {
            this._worldAutosizer = GameObject.FindObjectOfType<WorldAutosizer>();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();

            if (!this._worldAutosizer)
            {
                EditorGUILayout.HelpBox(
                    "zSpace has detected that you don't have a WorldAutoSizer script in your scene." +
                        " This script depends on that, and will be disabled.",
                    MessageType.Warning);
            }
        }

        private WorldAutosizer _worldAutosizer;
    }
}
