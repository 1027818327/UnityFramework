//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEditor;
using UnityEngine;

namespace zSpace.SimsCommon
{
    [CustomPropertyDrawer(typeof(DisplayScaleUtility.DisplayType))]
    public class DisplayTypeDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            property.enumValueIndex =
                EditorGUILayout.Popup(
                    new GUIContent(
                        "zSpace Design-Time Hardware:",
                        "Viewer scale and camera position are changed as a result of this. Only applied at start-up in play mode."),
                    property.enumValueIndex,
                    DesignTimeHardwareDescriptions);

            EditorGUI.EndProperty();
        }

        private readonly static GUIContent[] DesignTimeHardwareDescriptions = new GUIContent[]
        {
            new GUIContent("24-inch (300|200|All-In-One)"),
            new GUIContent("15-inch (Laptop)")
        };
    }
}