//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using System.IO;

using zSpace.Logging;
using zSpace.Logging.Journals;

namespace zSpace.SimsCommon
{
    public static class LoggingUtility
    {
        //////////////////////////////////////////////////////////////////
        // Public Methods
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes the logging system and configures the locations that
        /// log messages will be written to.
        /// </summary>
        /// <param name="applicationName">
        /// The name of the application using the logging system.  This is used
        /// to generate the names of log directories and files.  The name
        /// should be in UpperCamel style and only use ASCII letters and
        /// digits.
        /// </param>
        /// <remarks>
        /// This should generally be called once during the early
        /// initialization of an application that uses the logging system.  If
        /// possible, it should be called prior to running any other code.
        /// This ensures that the logging system will be initialzed before any
        /// other code attempts to emit log messages.
        /// </remarks>
        public static void InitializeLogging(string applicationName)
        {
            if (string.IsNullOrEmpty((applicationName ?? "").Trim()))
            {
                applicationName = "Log";
            }

            // Configure detail levels. This is what is used if there is no
            // LogOptions.json file present.
            Log.Options.DetailLevel = 9;
            Log.Options.DetailLevels[""] = 1;

            // Configure output.

#if UNITY_EDITOR
            Log.Options.Journal = new MultiJournal(
                new ConsoleJournal(),
                new FileSystemJournal(
                    Path.Combine(
                        InEditorLogDirPath,
                        applicationName + "-{#}.txt")),
                new AnalyticsJournal(
                    Path.Combine(
                        InEditorLogDirPath,
                        applicationName + "Analytics-{#}.txt")));
#else
            string logDirectory;
            logDirectory = Path.Combine(
                System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.CommonApplicationData),
                AppDataLogDirPath);
            logDirectory = Path.Combine(logDirectory, applicationName);

            string logFilePath =
                Path.Combine(logDirectory, applicationName + "-{#}.txt");

            string analyticsDirectory = Path.Combine(
                System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.CommonApplicationData),
                AppDataAnalyticsLogDirPath);
            analyticsDirectory =
                Path.Combine(analyticsDirectory, applicationName);

            string analyticsFilePath =
                Path.Combine(analyticsDirectory, applicationName + "-{#}.txt");

            Log.Options.Journal = new MultiJournal(
                new FileSystemJournal(logFilePath),
                new AnalyticsJournal(analyticsFilePath));
#endif

            // Reroute Unity logging.

            try
            {
                UnityEngine.Application.logMessageReceived += UnityLogCallback;
            }
            catch
            {
                UnityEngine.Debug.LogError(
                    "Unity error interceptor could not be registered. Make " +
                        "sure you're not calling " +
                        "LoggingUtility.InitializeLogging() from a " +
                        "background thread.");
            }
        }

        //////////////////////////////////////////////////////////////////
        // Private Methods
        //////////////////////////////////////////////////////////////////

        private static void UnityLogCallback(
            string message,
            string stackTrace,
            UnityEngine.LogType type)
        {
            if (message.Length > 0 &&
                message[0] ==
                    zSpace.Logging.Journals.ConsoleJournal.UnityMagicPrefix)
            {
                return;
            }

            int outLevel;
            string outType;
            switch (type)
            {
                case UnityEngine.LogType.Assert:
                    outLevel = DebugLogger.LevelAssert;
                    outType = DebugLogger.TypeAssert;
                    break;
                case UnityEngine.LogType.Error:
                    outLevel = DebugLogger.LevelError;
                    outType = DebugLogger.TypeError;
                    break;
                case UnityEngine.LogType.Exception:
                    outLevel = DebugLogger.LevelError;
                    outType = DebugLogger.TypeException;
                    break;
                default:
                case UnityEngine.LogType.Log:
                    outLevel = DebugLogger.LevelLog;
                    outType = DebugLogger.TypeLog;
                    break;
                case UnityEngine.LogType.Warning:
                    outLevel = DebugLogger.LevelWarning;
                    outType = DebugLogger.TypeWarning;
                    break;
            }

            UnityRerouteLogger.Log(
                outLevel,
                outType,
                "",
                new
                {
                    Message = message,
                    StackTrace = stackTrace,
                });
        }

        //////////////////////////////////////////////////////////////////
        // Private Constants
        //////////////////////////////////////////////////////////////////

        private const string AppDataLogDirPath = @"zSpace\Logs";
        private const string AppDataAnalyticsLogDirPath = @"zSpace\Analytics";

#if UNITY_EDITOR
        private const string InEditorLogDirPath = @".\Logs";
#endif

        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

#pragma warning disable 414
        private static DebugLogger Debug =
            Log.Debug(typeof(LoggingUtility));
#pragma warning restore 414

        private static SimpleLogger UnityRerouteLogger =
            Log.Simple("OtherUnity");
    }
}
