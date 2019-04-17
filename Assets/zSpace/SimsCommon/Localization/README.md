# Localization
A framework that allows for localizing strings between different locales using CSS files.

# Instructions
1. Use the `LocalizationUtility`'s `InitializeDefaultLocalizationService()` method with the CSS file
    you want to use to spin up the localization framework.
2. You can find the base CSS file (that we use for our components) in Assets/StreamingAssets/Localizations/(your locale)/
3. For any string you want to localize, simply call one of the various `String()` methods on
    `LocalizationService.DefaultLocalizationService`
    and pass in the identifier that references a localized string in your CSS file.
    
**Note: To test changing locale, change the `StartLocale` within the Assets/StreamingAssets/Localizations/Configuration.json
    file to the locale you want to test.**

**Example:** *Check out `LocalizationSample` within the `Samples` folder for an example on how to set everything up!*
