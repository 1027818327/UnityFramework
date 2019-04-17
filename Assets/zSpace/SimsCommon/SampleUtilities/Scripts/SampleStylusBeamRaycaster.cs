//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using zSpace.Core;

namespace zSpace.SimsCommon
{
    [RequireComponent(typeof(StylusBeamVisualization))]
    public class SampleStylusBeamRaycaster : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////
        // Public Properties
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// The beam length to use to perform raycasts against objects in the scene. Also
        /// the fallback length to set the visualization to in case no objects hit.
        /// Units must be specified in meters.
        /// </summary>
        [Tooltip("The beam length to use to perform raycasts against objects in the scene.")]
        public float BeamLength = 0.18f;
        /// <summary>
        /// The "helper" length that is used to check collisions and artificially extends the
        /// beam collision length.
        /// Units must be specified in meters.
        /// </summary>
        [Tooltip("The \"helper\" length that is used to check collisions and artificially extends the beam collision length.")]
        public float HelperLength = 0.02f;

        //////////////////////////////////////////////////////////////////
        // Unity Methods
        //////////////////////////////////////////////////////////////////

        // Use this for initialization
        void Start()
        {
            this._zCore = GameObject.FindObjectOfType<ZCore>();
            this._stylusBeam = this.GetComponent<StylusBeamVisualization>();
            if (this._stylusBeam == null)
            {
                Debug.LogError("Needs a StylusBeamVisualization component attached to function correctly.");
            }
            this._stylusInputModule = GameObject.FindObjectOfType<StylusInputModule>();
            if (this._stylusInputModule == null)
            {
                Debug.LogError("Needs a StylusInputModule component somewhere in the scene to function correctly.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (this._stylusBeam != null)
            {
                // Gets the current object and distance from the UI raycast, if any.
                float uiHitDistance = float.MaxValue;
                GameObject uiHitObject = null;
                if (this._stylusInputModule != null)
                {
                    uiHitObject = this._stylusInputModule.GetCurrentIntersection();
                    if (uiHitObject != null)
                    {
                        uiHitDistance = this._stylusInputModule.GetStylusUiElementDistance();
                    }
                }

                // Raycasts into world space against colliders.
                // Gets a distance to the first object hit, if any.
                ZCore.Pose pose = this._zCore.GetTargetPose(ZCore.TargetType.Primary, ZCore.CoordinateSpace.World);
                float worldHitDistance = float.MaxValue;
                GameObject worldHitObject = null;
                RaycastHit currentHit;
                var raycastDistance = (this.BeamLength + this.HelperLength) * this._zCore.ViewerScale;
                if (Physics.Raycast(pose.Position, pose.Direction, out currentHit, raycastDistance))
                {
                    worldHitObject = currentHit.collider.gameObject;
                    worldHitDistance = currentHit.distance / this._zCore.ViewerScale;
                }

                // Use the smallest beam length found, if any. Else, default to BeamLength.
                if (uiHitObject != null || worldHitObject != null)
                {
                    this._stylusBeam.Length = Mathf.Min(uiHitDistance, worldHitDistance);
                }
                else
                {
                    this._stylusBeam.Length = this.BeamLength;
                }
            }
        }

        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

        #pragma warning disable 414
        private static zSpace.Logging.DebugLogger Debug =
            zSpace.Logging.Log.Debug(typeof(SampleStylusBeamRaycaster));
        #pragma warning restore 414

        private ZCore _zCore;
        private StylusBeamVisualization _stylusBeam;
        private StylusInputModule _stylusInputModule;
    }
}
