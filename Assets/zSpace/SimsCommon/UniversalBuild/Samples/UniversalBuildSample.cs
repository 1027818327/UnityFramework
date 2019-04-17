//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using zSpace.Core;

namespace zSpace.SimsCommon
{
    public class UniversalBuildSample : MonoBehaviour
    {
        public GameObject OriginalCanvasScaleText;
        public GameObject UiScaleFactorText;
        public GameObject CurrentViewerScaleText;
        public GameObject DisplaySizeText;
        public GameObject ViewportSizeText;
        public GameObject CanvasSizeText;

        // Use this for initialization
        void Awake()
        {
            this._zCore = GameObject.FindObjectOfType<ZCore>();

            this._uiCanvasAutosizer = GameObject.FindObjectOfType<UICanvasAutosizer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (this._zCore)
            {
                var originalCanvasScale = this._uiCanvasAutosizer.OriginalCanvasScale;
                this.OriginalCanvasScaleText.GetComponent<Text>().text = originalCanvasScale.x.ToString("F6");

                var uiScaleFactor = this._uiCanvasAutosizer.UIResolutionScaleFactor;
                this.UiScaleFactorText.GetComponent<Text>().text = uiScaleFactor.ToString("F3");

                var currentViewerScale = this._zCore.ViewerScale;
                this.CurrentViewerScaleText.GetComponent<Text>().text = currentViewerScale.ToString("F6");

                var displaySize = this._zCore.GetDisplaySize();
                this.DisplaySizeText.GetComponent<Text>().text =
                    "Width: " + displaySize.x + " Height: " + displaySize.y;

                var viewportSize = this._zCore.GetViewportSize();
                this.ViewportSizeText.GetComponent<Text>().text =
                    "Width: " + viewportSize.x + " Height: " + viewportSize.y;

                var canvasSize = this._uiCanvasAutosizer.GetComponent<RectTransform>();
                this.CanvasSizeText.GetComponent<Text>().text =
                    "Width: " + canvasSize.rect.width + " Height: " + canvasSize.rect.height;
            }
        }

        private ZCore _zCore;
        private UICanvasAutosizer _uiCanvasAutosizer;
    }
}
