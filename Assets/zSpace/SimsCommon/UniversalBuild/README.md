# Universal Build
A couple scripts that help resize simulations when transitioning between the
two different systems with different physical display sizes. Also includes a
couple other helpful utilities.

# Instructions
1) Add the WorldAutosizer script to any GameObject.
2) Set the 'zSpace Design-Time Hardware' property on it to the form factor
    you are designing on. It will change the viewer scale if you switch screen sizes.
3) Add the WorldAutosizerCanceler script to any object you wish to back out
    this scale on. If you have a UI, that would be a prime candidate for this script.
4) Done! Everything should now resize appropriately.

# Scripts:
* **WorldAutosizer.cs**
    * This will apply a scale factor to the zCore "viewer scale" property based on display size.
* **WorldAutosizerCanceler.cs**
    * This will apply the same scale factor as the WorldAutosizer but on any GameObject. This effectively "cancels" out the size change from viewer scale changing.
* **UICanvasAutosizer.cs**
    * This script handles scaling and resizing RectTransform elements. There is a utility on it that let's you scale in-editor, and then it will also scale at runtime. This script also handles the case if viewerscale changes on the fly.