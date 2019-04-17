//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace zSpace.SimsCommon
{
    public static class DisplayScaleUtility
    {
        public enum DisplayType
        {
            Size24Inch_Aspect16x9,
            Size15Inch_Aspect16x9,
            Unknown
        }

        // Get the hard-coded display sizes for each of our devices.
        static public Vector2 GetDisplaySizeByDisplayType(DisplayType targetDisplay)
        {
            switch (targetDisplay)
            {
                case DisplayType.Size15Inch_Aspect16x9:   // 400
                    return new Vector2(0.344f, 0.193f);
                case DisplayType.Size24Inch_Aspect16x9:   // 200 & 300 "All-in-One"
                default:
                    return new Vector2(0.521f, 0.293f);
            }
        }

        // Get the x/y scale to apply to game objects to fit them appropriately on the current display.
        static public Vector2 GetDisplayScaleFactor(Vector2 currentDisplaySize, DisplayType targetDisplay)
        {
            var targetDisplaySize = GetDisplaySizeByDisplayType(targetDisplay);

            return new Vector2(targetDisplaySize.x / currentDisplaySize.x, targetDisplaySize.y / currentDisplaySize.y);
        }

        // Get the x/y scale to apply to your Canvas's Width/Height in the RectTransform, to scale
        // it appropriately. It is the inverse of GetDisplayScaleFactor().
        static public Vector2 GetUIDimensionScaleFactor(Vector2 currentDisplaySize, DisplayType targetDisplay)
        {
            var targetDisplaySize = GetDisplaySizeByDisplayType(targetDisplay);

            return new Vector2(currentDisplaySize.x / targetDisplaySize.x, currentDisplaySize.y / targetDisplaySize.y);
        }
    }
}
