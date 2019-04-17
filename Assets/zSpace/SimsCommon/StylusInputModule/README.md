# Stylus Input Module
A custom Unity input module that replaces the Standalone Input Module that is placed by default on the
Event System object. This module adds on stylus support for Unity's UI. 

# Instructions
1) Set up your Unity UI (add a UI element)
2) Navigate to your `EventSystem` GameObject
3) Click on the gear symbol to the top-right of the `StandaloneInputModule`
    component and click 'Remove Component'
4) Click on 'Add Component' and add the `StylusInputModule`
5) To correctly handle mouse input now, you will need to include the `ZCanvasScaler`
    component somewhere in your scene (from the Universal Build project) and check
    the 'Use Center Camera On Canvas' option.

**Warning:** *This script requires that your UI is at zero parallax.*

# Scripts:
* **StylusInputModule.cs**
	* The input module which contains all of the logic for performing stylus raycasts against UI elements.