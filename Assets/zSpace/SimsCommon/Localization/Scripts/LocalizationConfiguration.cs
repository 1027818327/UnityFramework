//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using zSpace.Localization;

namespace zSpace.SimsCommon
{
    [JsonObject]
    public sealed class LocalizationConfiguration
    {
        [JsonProperty]
        public string StartLocale;
        [JsonProperty]
        public string FileSearchPattern;
        [JsonProperty]
        public string AvailableLocales;
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public DebugMode? Debug;
        [JsonProperty]
        public string DebugMask;
        [JsonProperty]
        public string DebugMaskExceptions;
    }
}
