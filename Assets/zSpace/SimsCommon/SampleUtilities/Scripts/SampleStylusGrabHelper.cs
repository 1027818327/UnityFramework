//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using zSpace.Core;

namespace zSpace.SimsCommon
{
    public class SampleStylusGrabHelper : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////
        // Unity Monobehaviour Callbacks
        //////////////////////////////////////////////////////////////////

        void Start()
        {
            this._zCore = GameObject.FindObjectOfType<ZCore>();
            if (this._zCore == null)
            {
                Debug.LogError("Unable to find reference to zSpace.Core.Core Monobehaviour.");
                this.enabled = false;
                return;
            }
            this._StylusBeamRaycasterSample = this.GetComponent<SampleStylusBeamRaycaster>();
            if (this._StylusBeamRaycasterSample == null)
            {
                Debug.LogError("Needs a StylusBeamVisualization component attached to function correctly.");
            }
            this._stylusInputModule = GameObject.FindObjectOfType<StylusInputModule>();
            if (this._stylusInputModule == null)
            {
                Debug.LogError("Needs a StylusInputModule component attached to function correctly.");
            }
        }

        void Update()
        {
            // Grab the latest stylus pose and button state information.
            ZCore.Pose pose = this._zCore.GetTargetPose(ZCore.TargetType.Primary, ZCore.CoordinateSpace.World);
            bool isButtonPressed = this._zCore.IsTargetButtonPressed(ZCore.TargetType.Primary, 0);

            switch (this._stylusState)
            {
                case StylusState.Idle:
                    // Perform a raycast on the entire scene to determine what the
                    // stylus is currently colliding with.
                    if (this._stylusInputModule.GetCurrentIntersection() == null)
                    {
                        RaycastHit hit;
                        var stylusLength = (this._StylusBeamRaycasterSample.BeamLength + this._StylusBeamRaycasterSample.HelperLength) * this._zCore.ViewerScale;
                        if (Physics.Raycast(pose.Position, pose.Direction, out hit, stylusLength))
                        {
                            // If the front stylus button was pressed, initiate a grab.
                            if (isButtonPressed && !this._wasButtonPressed)
                            {
                                // Begin the grab.
                                this.BeginGrab(hit.collider.gameObject, hit.distance, pose.Position, pose.Rotation);

                                this._stylusState = StylusState.Grab;
                            }
                        }
                    }
                    break;
                case StylusState.Grab:
                    // Update the grab.
                    this.UpdateGrab(pose.Position, pose.Rotation);

                    // End the grab if the front stylus button was released.
                    if (!isButtonPressed && this._wasButtonPressed)
                    {
                        this._stylusState = StylusState.Idle;
                    }
                    break;
                default:
                    break;
            }

            // Cache state for next frame.
            this._wasButtonPressed = isButtonPressed;
        }


        //////////////////////////////////////////////////////////////////
        // Private Helpers
        //////////////////////////////////////////////////////////////////

        private void BeginGrab(GameObject hitObject, float hitDistance, Vector3 inputPosition, Quaternion inputRotation)
        {
            Vector3 inputEndPosition = inputPosition + (inputRotation * (Vector3.forward * hitDistance));

            // Cache the initial grab state.
            this._grabObject = hitObject;
            this._initialGrabOffset = Quaternion.Inverse(hitObject.transform.rotation) * (hitObject.transform.position - inputEndPosition);
            this._initialGrabRotation = Quaternion.Inverse(inputRotation) * hitObject.transform.rotation;
            this._initialGrabDistance = hitDistance;
        }

        private void UpdateGrab(Vector3 inputPosition, Quaternion inputRotation)
        {
            Vector3 inputEndPosition = inputPosition + (inputRotation * (Vector3.forward * this._initialGrabDistance));

            // Update the grab object's rotation.
            Quaternion objectRotation = inputRotation * this._initialGrabRotation;
            this._grabObject.transform.rotation = objectRotation;

            // Update the grab object's position.
            Vector3 objectPosition = inputEndPosition + (objectRotation * this._initialGrabOffset);
            this._grabObject.transform.position = objectPosition;
        }

        //////////////////////////////////////////////////////////////////
        // Private Enumerations
        //////////////////////////////////////////////////////////////////

        private enum StylusState
        {
            Idle = 0,
            Grab = 1,
        }

        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

#pragma warning disable 414
        private static zSpace.Logging.DebugLogger Debug =
            zSpace.Logging.Log.Debug(typeof(SampleStylusGrabHelper));
#pragma warning restore 414

        private ZCore _zCore = null;
        private SampleStylusBeamRaycaster _StylusBeamRaycasterSample;
        private StylusInputModule _stylusInputModule;
        private bool _wasButtonPressed = false;

        private StylusState _stylusState = StylusState.Idle;
        private GameObject _grabObject = null;
        private Vector3 _initialGrabOffset = Vector3.zero;
        private Quaternion _initialGrabRotation = Quaternion.identity;
        private float _initialGrabDistance = 0.0f;
    }
}