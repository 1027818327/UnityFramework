//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using zSpace.Core;

namespace zSpace.SimsCommon
{
    public class UICanvasAutosizer : MonoBehaviour
    {
        public Vector2 OriginalCanvasScale { get; private set; }
        public Vector2 UIResolutionScaleFactor { get; private set; }

        private void Awake()
        {
            this.Initialize();

            if (!this._zCore || !this._canvasRect)
            {
                this.enabled = false;
            }
        }

        private void Start()
        {
            var currentDisplaySize = this._zCore.GetDisplaySize();

            var worldAutosizerCanceler = this.GetComponent<WorldAutosizerCanceler>();

            // If both the autosizer and canceler are in action, we want to use the UI scale factor based on the current
            // display and design-time hardware. Otherwise we will not be using this scale factor when resizing UI.
            if (this._worldAutosizer && this._worldAutosizer.enabled
                && worldAutosizerCanceler && worldAutosizerCanceler.enabled)
            {
                this.UIResolutionScaleFactor =
                    DisplayScaleUtility.GetUIDimensionScaleFactor(currentDisplaySize, this._worldAutosizer.DesignTimeZSpaceHardware);
            }
            else
            {
                this.UIResolutionScaleFactor = new Vector2(1.0f, 1.0f);
            }

            this.ScaleToDisplaySize();
            
            this._canvasOriginalSize = new Vector2(this._canvasRect.rect.width, this._canvasRect.rect.height);
            this._displayNativeResolution = this._zCore.GetDisplayNativeResolution();
            if (this._displayNativeResolution.x == 0 || this._displayNativeResolution.y == 0)
            {
                // Default to this since both our current displays support this
                this._displayNativeResolution = new Vector2(1920, 1080);
            }
        }

        private void Update()
        {
            if (!this._currentViewerScale.HasValue)
            {
                // Late initialization to wait for any changes in Awake/Start
                // We only want to handle the cases after startup
                this._currentViewerScale = this._zCore.ViewerScale;
            } else
            {
                // In the case that viewer scale has changed (somebody modifying it on the fly)
                if (this._currentViewerScale != this._zCore.ViewerScale)
                {
                    var scaleFactor = this._zCore.ViewerScale / this._currentViewerScale.Value;
                    this._canvasRect.localScale = new Vector3(
                        this._canvasRect.localScale.x * scaleFactor,
                        this._canvasRect.localScale.y * scaleFactor,
                        1.0f);
                    this._currentViewerScale = this._zCore.ViewerScale;
                }
            }

            this.ResizeToViewport();
            this.RotateToDisplay();
            this.PositionAtViewportCenter();
        }

        public void FitToDisplayInEditor()
        {
            this.Initialize();
            if (this._zCore && this._canvasRect)
            {
                this.ScaleToDisplaySize();
                this.RotateToDisplay();
                this.PositionAtViewportCenter();
            }
        }

        /* Private Methods */

        private void Initialize()
        {
            this._zCore = GameObject.FindObjectOfType<ZCore>();
            if (this._zCore == null)
            {
                Debug.LogError("Unable to find reference to zSpace.Core.ZCore Monobehaviour.");
            }

            this._canvasRect = GetComponent<RectTransform>();
            if (!this._canvasRect)
            {
                Debug.LogError("This script needs to be attached to a GameObject with a RectTransform to function correctly.");
            }

            this._worldAutosizer = GameObject.FindObjectOfType<WorldAutosizer>();
        }

        private void ResizeToViewport()
        {
            var viewportSize = this._zCore.GetViewportSize();
            if (viewportSize != this._currentViewportSize)
            {
                this._currentViewportSize = viewportSize;
                var viewportRatio = new Vector2(
                    this._currentViewportSize.x / this._displayNativeResolution.x,
                    this._currentViewportSize.y / this._displayNativeResolution.y);

                // Need to use the original canvas size here to support different-sized starting sizes
                var newSize = new Vector2(
                    this._canvasOriginalSize.x * viewportRatio.x,
                    this._canvasOriginalSize.y * viewportRatio.y);

                // Set the dimensions of the canvas to a new size
                this._canvasRect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    newSize.x * this.UIResolutionScaleFactor.x);
                this._canvasRect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical,
                    newSize.y * this.UIResolutionScaleFactor.y);
            }
        }

        private void ScaleToDisplaySize()
        {
            // If there is an autosizer, we will want to scale to the design-time hardware. Otherwise, we scale to the current hardware size.
            var targetDisplaySize = this._worldAutosizer ?
                DisplayScaleUtility.GetDisplaySizeByDisplayType(this._worldAutosizer.DesignTimeZSpaceHardware) :
                this._zCore.GetDisplaySize();
            // Take into account the viewer scale
            targetDisplaySize *= this._zCore.ViewerScale;

            var canvasDimensions = this._canvasRect.rect;
            this.OriginalCanvasScale = new Vector2(
                (1.0f / canvasDimensions.width * targetDisplaySize.x),
                (1.0f / canvasDimensions.height * targetDisplaySize.y));

            // For different aspect ratios. Fit the correct dimension.
            var canvasScaleFactor =
                ((canvasDimensions.width / canvasDimensions.height) < (targetDisplaySize.x / targetDisplaySize.y)) ?
                this.OriginalCanvasScale.y :
                this.OriginalCanvasScale.x;
            this._canvasRect.localScale = new Vector3(
                canvasScaleFactor,
                canvasScaleFactor,
                canvasScaleFactor);
        }

        private void RotateToDisplay()
        {
            this._canvasRect.transform.rotation = this._zCore.GetViewportWorldRotation();
        }

        private void PositionAtViewportCenter()
        {
            this._canvasRect.position = this._zCore.GetViewportWorldCenter();
        }

        private RectTransform _canvasRect = null;
        private ZCore _zCore = null;
        private WorldAutosizer _worldAutosizer = null;

        private Vector2 _canvasOriginalSize = Vector2.zero;
        private Vector2 _displayNativeResolution = Vector2.zero;
        private float? _currentViewerScale;

        private Vector2 _currentViewportSize = Vector2.zero;
    }
}