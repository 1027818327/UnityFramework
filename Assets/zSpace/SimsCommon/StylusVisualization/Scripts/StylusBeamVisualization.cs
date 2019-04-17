//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace zSpace.SimsCommon
{
    public class StylusBeamVisualization : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////
        // Public Properties
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// The beam length to set all beam parts plus nib to. Can be updated
        /// on the fly and the visualization will update itself accordingly.
        /// Units must be specified in meters.
        /// </summary>
        [Tooltip("The beam length to set all beam parts plus nib to.")]
        public float Length = 0.18f;

        /// <summary>
        /// The "nib" object that sits on top of the beam.
        /// Units must be specified in meters.
        /// </summary>
        [Tooltip("The \"nib\" object that sits on top of the beam. Units must be specified in meters.")]
        public GameObject Nib = null;

        //////////////////////////////////////////////////////////////////
        // Private Inspector Fields
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// The "main" beam that will stretch to fit size designed by BeamLength.
        /// Units must be specified in meters.
        /// </summary>
        [SerializeField]
        [Tooltip("The \"main\" beam that will stretch to fit size designed by BeamLength. Units must be specified in meters.")]
        private GameObject _beamMain = null;
        /// <summary>
        /// Optional: An extra beam object that sits at the base of the beam and does not change size.
        /// Units must be specified in meters.
        /// </summary>
        [SerializeField]
        [Tooltip("Optional: An extra beam object that sits at the base of the beam and does not change size. Units must be specified in meters.")]
        private GameObject _beamExtraBottom = null;

        //////////////////////////////////////////////////////////////////
        // Unity Methods
        //////////////////////////////////////////////////////////////////

        // Use this for initialization
        void Awake()
        {
            if (this._beamExtraBottom == null)
            {
                this._beamMain.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                // Note: This logic means we can not support swapping out the extra bottom beam right now.
                //       To do so, we would have to update these two things within Update()
                this._originalExtraBottomLength = this._beamExtraBottom.transform.localScale.z;
                this._beamMain.transform.localPosition = new Vector3(0f, 0f, this._originalExtraBottomLength);
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (this.Length != this._previousBeamLength)
            {
                var beamScale = this._beamMain.transform.localScale;
                if (this._beamExtraBottom == null)
                {
                    beamScale.z = this.Length;
                }
                else
                {
                    var beamBottomScale = this._beamExtraBottom.transform.localScale;
                    var adjustedBeamLength = this.Length - this.Nib.transform.localScale.z;
                    if (adjustedBeamLength > this._originalExtraBottomLength)
                    {
                        beamScale.z = adjustedBeamLength - this._originalExtraBottomLength;
                        this._beamExtraBottom.transform.localScale =
                            new Vector3(beamBottomScale.x, beamBottomScale.y, this._originalExtraBottomLength);
                    }
                    else
                    {
                        beamScale.z = 0f;
                        this._beamExtraBottom.transform.localScale =
                            new Vector3(
                                beamBottomScale.x,
                                beamBottomScale.y,
                                (adjustedBeamLength > 0f) ? adjustedBeamLength : 0f);
                    }
                }
                this._beamMain.transform.localScale = beamScale;

                var nibPosition = Mathf.Max(this.Length - this.Nib.transform.localScale.z, 0f);
                this.Nib.transform.localPosition = new Vector3(0f, 0f, nibPosition);
                this._previousBeamLength = this.Length;
            }
        }

        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

#pragma warning disable 414
        private static zSpace.Logging.DebugLogger Debug =
            zSpace.Logging.Log.Debug(typeof(StylusBeamVisualization));
#pragma warning restore 414

        private float _previousBeamLength = 0.0f;
        private float _originalExtraBottomLength = 0.0f;
    }
}