//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using NUnit.Framework;
using UnityEngine;

namespace zSpace.SimsCommon
{
    public class UniversalBuildTests
    {
        // Tests for the scale factor that gets applied to the viewer scale and canvas scale
        [Test]
        public void ScaleFactor_GoingFromDesktopToLaptop_WillBeConstant()
        {
            var currentDisplaySize = DisplayScaleUtility.GetDisplaySizeByDisplayType(DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9);
            var targetDisplay = DisplayScaleUtility.DisplayType.Size15Inch_Aspect16x9;
            var scaleFactor = DisplayScaleUtility.GetDisplayScaleFactor(currentDisplaySize, targetDisplay);

            Assert.AreEqual(scaleFactor.x, 0.6602f, 0.0001f);
            Assert.AreEqual(scaleFactor.y, 0.6587f, 0.0001f);
        }
        [Test]
        public void ScaleFactor_GoingFromLaptopToDesktop_WillBeConstant()
        {
            var currentDisplaySize = DisplayScaleUtility.GetDisplaySizeByDisplayType(DisplayScaleUtility.DisplayType.Size15Inch_Aspect16x9);
            var targetDisplay = DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9;
            var scaleFactor = DisplayScaleUtility.GetDisplayScaleFactor(currentDisplaySize, targetDisplay);

            Assert.AreEqual(scaleFactor.x, 1.5145f, 0.0001f);
            Assert.AreEqual(scaleFactor.y, 1.5181f, 0.0001f);
        }
        [Test]
        public void ScaleFactor_GoingFromLaptopToLaptop_WillBeConstant()
        {
            var currentDisplaySize = DisplayScaleUtility.GetDisplaySizeByDisplayType(DisplayScaleUtility.DisplayType.Size15Inch_Aspect16x9);
            var targetDisplay = DisplayScaleUtility.DisplayType.Size15Inch_Aspect16x9;
            var scaleFactor = DisplayScaleUtility.GetDisplayScaleFactor(currentDisplaySize, targetDisplay);

            Assert.AreEqual(scaleFactor.x, 1.0f);
            Assert.AreEqual(scaleFactor.y, 1.0f);
        }
        [Test]
        public void ScaleFactor_GoingFromDesktopToDesktop_WillBeConstant()
        {
            var currentDisplaySize = DisplayScaleUtility.GetDisplaySizeByDisplayType(DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9);
            var targetDisplay = DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9;
            var scaleFactor = DisplayScaleUtility.GetDisplayScaleFactor(currentDisplaySize, targetDisplay);

            Assert.AreEqual(scaleFactor.x, 1.0f);
            Assert.AreEqual(scaleFactor.y, 1.0f);
        }

        // Tests for the "inverse" scale factor that gets applied to the height/width of the canvas
        [Test]
        public void UIScaleFactor_GoingFromDesktopToLaptop_WillBeConstant()
        {
            var currentDisplaySize = DisplayScaleUtility.GetDisplaySizeByDisplayType(DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9);
            var targetDisplay = DisplayScaleUtility.DisplayType.Size15Inch_Aspect16x9;
            var scaleFactor = DisplayScaleUtility.GetUIDimensionScaleFactor(currentDisplaySize, targetDisplay);

            Assert.AreEqual(scaleFactor.x, 1.5145f, 0.0001f);
            Assert.AreEqual(scaleFactor.y, 1.5181f, 0.0001f);
        }
        [Test]
        public void UIScaleFactor_GoingFromLaptopToDesktop_WillBeConstant()
        {
            var currentDisplaySize = DisplayScaleUtility.GetDisplaySizeByDisplayType(DisplayScaleUtility.DisplayType.Size15Inch_Aspect16x9);
            var targetDisplay = DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9;
            var scaleFactor = DisplayScaleUtility.GetUIDimensionScaleFactor(currentDisplaySize, targetDisplay);

            Assert.AreEqual(scaleFactor.x, 0.6602f, 0.0001f);
            Assert.AreEqual(scaleFactor.y, 0.6587f, 0.0001f);
        }
        [Test]
        public void UIScaleFactor_GoingFromLaptopToLaptop_WillBeConstant()
        {
            var currentDisplaySize = DisplayScaleUtility.GetDisplaySizeByDisplayType(DisplayScaleUtility.DisplayType.Size15Inch_Aspect16x9);
            var targetDisplay = DisplayScaleUtility.DisplayType.Size15Inch_Aspect16x9;
            var scaleFactor = DisplayScaleUtility.GetUIDimensionScaleFactor(currentDisplaySize, targetDisplay);

            Assert.AreEqual(scaleFactor.x, 1.0f);
            Assert.AreEqual(scaleFactor.y, 1.0f);
        }
        [Test]
        public void UIScaleFactor_GoingFromDesktopToDesktop_WillBeConstant()
        {
            var currentDisplaySize = DisplayScaleUtility.GetDisplaySizeByDisplayType(DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9);
            var targetDisplay = DisplayScaleUtility.DisplayType.Size24Inch_Aspect16x9;
            var scaleFactor = DisplayScaleUtility.GetUIDimensionScaleFactor(currentDisplaySize, targetDisplay);

            Assert.AreEqual(scaleFactor.x, 1.0f);
            Assert.AreEqual(scaleFactor.y, 1.0f);
        }
    }
}
