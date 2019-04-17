//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;

namespace zSpace.SimsCommon
{
    [CustomEditor(typeof(UICanvasAutosizer))]
    public class UICanvasAutosizerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();

            if (GUILayout.Button(
                new GUIContent(
                    "Fit Canvas to Display",
                    "Positions/rotates/scales your canvas to fit design-time display size. This is useful "
                    + "during edit mode when laying out 3D content relative to UI. Note: This same action is "
                    + "performed at runtime every frame.")))
            {
                UICanvasAutosizer canvasAutosizer = (UICanvasAutosizer)this.target;
                canvasAutosizer.FitToDisplayInEditor();
            }
        }
    }
}