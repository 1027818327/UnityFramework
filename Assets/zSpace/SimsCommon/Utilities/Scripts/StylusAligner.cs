//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;

using zSpace.Core;

namespace zSpace.SimsCommon
{
    public class StylusAligner : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////
        // Public Inspector Fields
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// Offset in meters from the physical tip of the stylus.
        /// </summary>
        [Tooltip("Offset in meters from the physical tip of the stylus.")]
        public Vector3 Offset = Vector3.zero;

        /// <summary>
        /// Whether or not to ignore viewer scale.
        /// </summary>
        [Tooltip("Whether or not to ignore viewer scale.")]
        public bool IgnoreViewerScale = true;


        //////////////////////////////////////////////////////////////////
        // MonoBehaviour Callbacks
        //////////////////////////////////////////////////////////////////

        void Start()
        {
            _zCore = GameObject.FindObjectOfType<ZCore>();
            if (_zCore == null)
            {
                this.enabled = false;
                return;
            }
        }

        void LateUpdate()
        {
            if (_zCore != null)
            {
                ZCore.Pose pose =
                    _zCore.GetTargetPose(ZCore.TargetType.Primary, ZCore.CoordinateSpace.World);

                float scaleFactor = this.IgnoreViewerScale ? _zCore.ViewerScale : 1.0f;

                this.transform.position = pose.Position + (pose.Rotation * (this.Offset * scaleFactor));
                this.transform.rotation = pose.Rotation;
            }
        }


        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

#pragma warning disable 414
        private static zSpace.Logging.DebugLogger Debug =
            zSpace.Logging.Log.Debug(typeof(StylusAligner));
#pragma warning restore 414

        private ZCore _zCore = null;
    }
}