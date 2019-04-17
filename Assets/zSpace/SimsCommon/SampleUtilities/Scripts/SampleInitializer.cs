//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace zSpace.SimsCommon
{
    /// <summary>
    /// Initializes common sub-systems like logging and localization for sample
    /// scenes.
    /// </summary>
    /// <remarks>
    /// This component is only intended to be used in standalone sample scenes.
    /// Applications should not use this component and should instead perform
    /// their own intialization.
    /// </remarks>
    public class SampleInitializer : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////
        // Public Inspector Fields
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// The name of the sample that is being intialized.  Used for
        /// generating log directory and file names.
        /// </summary>
        [Tooltip("The name of the sample that is being intialized.")]
        public string SampleName;

        //////////////////////////////////////////////////////////////////
        // MonoBehaviour Callbacks
        //////////////////////////////////////////////////////////////////

        void Awake()
        {
            LoggingUtility.InitializeLogging(this.SampleName);

            LocalizationUtility.InitializeDefaultLocalizationService(
                "SimsCommonSamples.css");
        }

        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

#pragma warning disable 414
        private static zSpace.Logging.DebugLogger Debug =
            zSpace.Logging.Log.Debug(typeof(SampleInitializer));
#pragma warning restore 414
    }
}
