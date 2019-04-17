//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using zSpace.Core;

namespace zSpace.SimsCommon
{
    public class WorldAutosizerCanceler : MonoBehaviour
    {
        private void Awake()
        {
            this.Initialize();
        }

        private void Start() {
            if (this._zCore && this._worldAutosizer)
            {
                var designTimeHardware = this._worldAutosizer.DesignTimeZSpaceHardware;

                var displaySize = this._zCore.GetDisplaySize();
                var displayScaleFactor = DisplayScaleUtility.GetDisplayScaleFactor(displaySize, designTimeHardware);
                this.transform.localScale = new Vector3(
                    this.transform.localScale.x * displayScaleFactor.x,
                    this.transform.localScale.y * displayScaleFactor.x,
                    this.transform.localScale.z * displayScaleFactor.x);
            }
        }

        private void Initialize()
        {
            this._zCore = GameObject.FindObjectOfType<ZCore>();
            if (this._zCore == null)
            {
                Debug.LogError("Unable to find reference to zSpace.Core.ZCore Monobehaviour.");
            }

            this._worldAutosizer = GameObject.FindObjectOfType<WorldAutosizer>();
            if (!this._worldAutosizer)
            {
                Debug.LogError("This script relies on a ViewerScaleAutosizer script being somewhere in your scene.");
            }
        }

        private ZCore _zCore;
        private WorldAutosizer _worldAutosizer;
    }
}
