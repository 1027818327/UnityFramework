//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using System;
using System.IO;

using UnityEngine;

using Newtonsoft.Json;

using zSpace.Localization;

namespace zSpace.SimsCommon
{
    public static class LocalizationUtility
    {
        //////////////////////////////////////////////////////////////////
        // Public Methods
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initialize the global default localization service by loading
        /// localization configuration and data from a standard location in
        /// streaming assets.
        /// </summary>
        /// <param name="localizationsFileSearchPatternOverride">
        /// If not null, this overrides the localization data file search
        /// pattern that is used to find localization data files to load.
        /// </param>
        /// <remarks>
        /// This should generally be called once during the early
        /// initialization of an application using the localization system.
        /// </remarks>
        public static void InitializeDefaultLocalizationService(
            string localizationsFileSearchPatternOverride = null)
        {
            var localizationService =
                LocalizationService.DefaultLocalizationService;

            var localizationsPath = Path.Combine(
                Application.streamingAssetsPath,
                LocalizationsStreamingAssetsPath);

            var localizationConfigPath = Path.Combine(
                localizationsPath, LocalizationConfigurationFileName);

            var config = ReadLocalizationConfiguration(localizationConfigPath);
            if (config == null)
            {
                Debug.LogWarning(
                    "Localization config file missing. No localizations " +
                        "loaded.");
            }
            else
            {
                Debug.LogVerbose(
                    new
                    {
                        Message = "Localization config file found.",
                        Configuration = config,
                    });

                // Available locales.

                var availableLocales = new string[] { };

                if (!string.IsNullOrEmpty(config.AvailableLocales))
                {
                    Debug.Log(
                        "AvailableLocales '{0}'.", config.AvailableLocales);

                    // Lower case the local names to make comparison easier.
                    availableLocales =
                        config.AvailableLocales.ToLower().Split(';');
                }

                // Start locale.

                if (!string.IsNullOrEmpty(config.StartLocale) &&
                    !config.StartLocale.Equals(
                        " ", StringComparison.InvariantCultureIgnoreCase) &&
                    !config.StartLocale.Equals(
                        "automatic",
                        StringComparison.InvariantCultureIgnoreCase) &&
                    !config.StartLocale.Equals(
                        "auto", StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        Debug.Log("Setting locale '{0}'.", config.StartLocale);
                        localizationService.Locale = config.StartLocale;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(
                            new
                            {
                                Message =
                                    "Error assigning locale from " +
                                        "localization config file.",
                                Exception = e,
                            });
                    }
                }

                int startLocaleIndex = Array.IndexOf(
                    availableLocales, localizationService.Locale.ToLower());
                if (startLocaleIndex == -1)
                {
                    Debug.LogWarning(
                        "Starting locale {0} in configuration file is not " +
                            "supported. Setting locale to en-US.",
                        config.StartLocale);

                    localizationService.Locale = "en-US";
                }

                // Localization files.
                {
                    var pattern =
                        !string.IsNullOrEmpty(config.FileSearchPattern) ?
                            config.FileSearchPattern : "*.css";

                    if (!string.IsNullOrEmpty(
                        localizationsFileSearchPatternOverride))
                    {
                        pattern = localizationsFileSearchPatternOverride;
                    }

                    if (!pattern.Contains("."))
                    {
                        pattern += ".css";
                    }

                    try
                    {
                        Debug.Log(
                            "Loading localizations '{0}' in '{1}'.",
                            pattern,
                            localizationsPath);
                        localizationService.LoadLocalizationsFromPath(
                            localizationsPath, true, pattern);
                        Debug.Log(
                            "{0} localization(s) loaded.",
                            localizationService.Localizations.Count);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(new
                        {
                            Message = "Error loading localizations from path.",
                            Exception = e,
                        });
                    }
                }

                // Debug mode.
                {
                    var debugMode = config.Debug;
                    if (debugMode != null &&
                        Enum.IsDefined(typeof(DebugMode), debugMode.Value))
                    {
                        Debug.Log(
                            "Localization debug mode '{0}'.",
                            debugMode.Value.ToString());
                        localizationService.DebugMode = debugMode.Value;
                    }

                    var debugCharacterMask = config.DebugMask;
                    if (!string.IsNullOrEmpty(debugCharacterMask))
                    {
                        Debug.Log(
                            "Localization debug character mask '{0}'.",
                            debugCharacterMask);
                        localizationService.DebugMask = debugCharacterMask;
                    }

                    var debugMaskExceptions = config.DebugMaskExceptions;
                    if(debugMaskExceptions != null)
                    {
                        char[] chars = debugMaskExceptions.ToCharArray();
                        localizationService.DebugMaskExceptions = chars;
                    }
                }
            }
        }

        /// <summary>
        /// Read in and deserialize the localization configuration file at the
        /// specified path.
        /// </summary>
        /// <param name="localizationConfigPath">
        /// The file system path of the localization configuration file to read.
        /// </param>
        /// <returns>
        /// The read localization configuration file or null if the file could
        /// not be found.
        /// </returns>
        public static LocalizationConfiguration ReadLocalizationConfiguration(
            string localizationConfigPath)
        {
            try
            {
                using (var configStream = File.Open(
                    localizationConfigPath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read))
                {
                    var serializer = new JsonSerializer();

                    try
                    {
                        using (var reader = new StreamReader(configStream))
                        {
                            using (var jsonReader = new JsonTextReader(reader))
                            {
                                var configuration =
                                    serializer.Deserialize<
                                        LocalizationConfiguration>(jsonReader);

                                Debug.LogVerbose(
                                    "Localization configuration present.");
                                return configuration;
                            }
                        }
                    }
                    catch (IOException e)
                    {
                        Debug.LogError(e);
                        return null;
                    }
                    catch (JsonException e)
                    {
                        Debug.LogError(e);
                        return null;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.LogVerbose("Localization configuration not present.");
                Debug.LogVerbose(e);
                return null;
            }
            catch (DirectoryNotFoundException e)
            {
                Debug.LogVerbose("Localization configuration not present.");
                Debug.LogVerbose(e);
                return null;
            }
        }

        //////////////////////////////////////////////////////////////////
        // Private Constants
        //////////////////////////////////////////////////////////////////

        private const string LocalizationsStreamingAssetsPath =
            "Localizations";

        private const string LocalizationConfigurationFileName =
            "Configuration.json";

        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

        private static zSpace.Logging.DebugLogger Debug =
            zSpace.Logging.Log.Debug(typeof(LocalizationUtility));
    }
}

