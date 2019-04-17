# Stylus Visualization
A small script that allows for showing the zSpace stylus at the length
that you wish it to be (in meter units). Also demonstrates other stylus
interactions like raycasting and object manipulation.

# Instructions
1) Drag in the prefab you want from the `Resources` directory (or instantiate it programatically).
2) If you want a different nib, swap it out and make sure to update 'Nib'
    property in the `StylusBeamVisualization` component on top-level game object.
3) Update Length on `StylusBeamVisualization` component to meter length that you want,
    also works programatically at any time.

**Example:** *Check out `StylusVisualizationSample` within the `Samples` folder for an example on how to set everything up!*

# Scripts:
* **StylusBeamVisualization.cs**
	* Contains logic for resizing the beam to different legths and moving the nib around.

# Other Scripts in Sample Scene
* **ViewerScaleCanceler.cs**
	* Makes sure that the stylus is always the same size, no matter what viewer scale is.
* **StylusAligner.cs**
	* Talks to zCore, gets the real-world stylus position, and updates the object to follow that.
* **SampleStylusBeamRaycaster.cs**
	* Does a raycast against UI/GameObject's and updates beam length based on that.
* **SampleStylusGrabHelper.cs**
	* Allows you to drag objects with stylus as long as object has a collider.