//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;

using zSpace.Core;

namespace zSpace.SimsCommon
{
    /// <summary>
    /// This script should only be attached to game objects that are scaled to be the
    /// correct size when the viewer scale is set to 1 (i.e. when one unit equals one meter).
    /// This script will not work when attached to game objects that expect a different viewer scale.
    /// </summary>
    public class ViewerScaleCanceler : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////
        // MonoBehaviour Callbacks
        //////////////////////////////////////////////////////////////////

        void Awake()
        {
            this._originalScale = this.transform.localScale;
        }

        void Start()
        {
            this._zCore = GameObject.FindObjectOfType<ZCore>();
            if (this._zCore == null)
            {
                this.enabled = false;
                return;
            }
        }

        void LateUpdate()
        {
            if (this._zCore != null)
            {
                this.transform.localScale = this._originalScale * this._zCore.ViewerScale;
            }
        }

        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

#pragma warning disable 414
        private static zSpace.Logging.DebugLogger Debug =
            zSpace.Logging.Log.Debug(typeof(ViewerScaleCanceler));
#pragma warning restore 414

        private ZCore _zCore = null;
        private Vector3 _originalScale = Vector3.zero;
    }
}