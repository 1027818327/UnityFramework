//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using zSpace.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace zSpace.SimsCommon
{
    public class WorldAutosizer : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////
        // Public/Serialized Members
        //////////////////////////////////////////////////////////////////


        /// <summary>
        /// Viewer scale and camera position are changed as a result of this. Only applied at start-up in play mode.
        /// </summary>
        public DisplayScaleUtility.DisplayType DesignTimeZSpaceHardware = DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9;

        /// <summary>
        /// Select this to show the design time hardware display size in-editor. (Light blue outline)
        /// </summary>
        [Tooltip("Select this to show the design time hardware display size in-editor. (Light blue outline)")]
        public bool ShowDisplay = false;

        //////////////////////////////////////////////////////////////////
        // Properties
        //////////////////////////////////////////////////////////////////

        public float AutosizedViewerScale
        {
            get { return this._autosizedViewerScale; }
        }

        //////////////////////////////////////////////////////////////////
        // Unity Methods
        //////////////////////////////////////////////////////////////////

        // Use this for initialization
        private void Start()
        {
            var success = this.TryInitialize();

            if (success)
            {
                var displaySize = this._zCore.GetDisplaySize();
                this._displayScaleFactor = DisplayScaleUtility.GetDisplayScaleFactor(displaySize, this.DesignTimeZSpaceHardware);

                // TODO: Implement support for different aspect ratio's (instead of only using x dimension)
                this._autosizedViewerScale = this._zCore.ViewerScale * this._displayScaleFactor.x;
                this._zCore.SetViewportWorldTransform(
                    this._zCore.GetViewportWorldCenter(),
                    this._zCore.GetViewportWorldRotation(),
                    this._autosizedViewerScale);
                this._zCore.InitialViewerScale = this._autosizedViewerScale;
            }
        }

        //////////////////////////////////////////////////////////////////
        // Private Methods
        //////////////////////////////////////////////////////////////////

        private bool TryInitialize()
        {
            if (!this._initialized)
            {
                this._initialized = true;
#if UNITY_EDITOR
                this._checkedForZCore = true;
#endif

                this._zCore = GameObject.FindObjectOfType<ZCore>();
                if (this._zCore == null || !this._zCore.enabled)
                {
                    Debug.LogError("Unable to find reference to zSpace.Core.ZCore Monobehaviour.");
                    this.enabled = false;

                    return false;
                }

            }
            return true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (this.ShowDisplay)
            {
                if (!this._initialized)
                {
                    if (this._checkedForZCore)
                    {
                        return;
                    }

                    var success = this.TryInitialize();
                    if (!success)
                    {
                        return;
                    }
                }

                var displaySize = this._zCore.GetDisplaySize();
                this._displayScaleFactor = DisplayScaleUtility.GetDisplayScaleFactor(displaySize, this.DesignTimeZSpaceHardware);

                // Draw the wireframe for the target display size in the editor
                var viewportCenter = this._zCore.GetViewportWorldCenter();
                var viewportRotation = this._zCore.GetViewportWorldRotation();
                var targetDisplaySize = DisplayScaleUtility.GetDisplaySizeByDisplayType(this.DesignTimeZSpaceHardware);
                var viewportToWorld = Matrix4x4.TRS(viewportCenter, viewportRotation, Vector3.one);

                Gizmos.color = Color.cyan;
                Gizmos.matrix = viewportToWorld;
                Gizmos.DrawWireCube(Vector3.zero, targetDisplaySize);

                // Draw a label for the wireframe
                Handles.matrix = viewportToWorld;
                Handles.Label((Vector3.up * (targetDisplaySize.y / 2)), new GUIContent("Target Display Size"));
            }
        }
#endif

        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

#if UNITY_EDITOR
        private bool _checkedForZCore = false;
#endif
        private bool _initialized = false;
        private ZCore _zCore = null;
        private Vector2 _displayScaleFactor = Vector2.zero;
        private float _autosizedViewerScale = 0.0f;
    }
}
