using Framework.Unity.Editor;
using UnityEditor;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;
using VRTK.SecondaryControllerGrabActions;

namespace Vive
{
    public class AddModelGrab
    {
        [MenuItem("Tools/Add Model Grab")]
        static void DoAddModelDrab()
        {
            AddBoxCollider();
            AddScripts();
            AddRigiBody();
        }

        static void AddBoxCollider()
        {
            GameObject obj = Selection.activeGameObject;
            MeshEditor.AddBoxCollider(obj);
        }

        static void AddRigiBody()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj.GetComponent<Rigidbody>() == null)
            {
                obj.AddComponent<Rigidbody>();
            }
        }

        static void AddScripts()
        {
            GameObject obj = Selection.activeGameObject;
            VRTK_ChildOfControllerGrabAttach grabAttachMechanicScript = obj.GetComponent<VRTK_ChildOfControllerGrabAttach>();
            VRTK_SwapControllerGrabAction secondaryGrabActionScript = obj.GetComponent<VRTK_SwapControllerGrabAction>();

            if (grabAttachMechanicScript == null)
            {
                grabAttachMechanicScript = obj.AddComponent<VRTK_ChildOfControllerGrabAttach>();
            }

            if (secondaryGrabActionScript == null)
            {
                secondaryGrabActionScript = obj.AddComponent<VRTK_SwapControllerGrabAction>();
            }

            VRTK_InteractableObject tempInteractScript = obj.GetComponent<VRTK_InteractableObject>();
            if (tempInteractScript == null)
            {
                tempInteractScript = obj.AddComponent<VRTK_InteractableObject>();
                //tempInteractScript.touchHighlightColor = new Color();
            }
            tempInteractScript.isGrabbable = true;
            tempInteractScript.holdButtonToGrab = false;
            tempInteractScript.stayGrabbedOnTeleport = true;
            tempInteractScript.isUsable = true;
            tempInteractScript.holdButtonToGrab = false;
            tempInteractScript.useOnlyIfGrabbed = false;
            tempInteractScript.grabAttachMechanicScript = grabAttachMechanicScript;
            tempInteractScript.secondaryGrabActionScript = secondaryGrabActionScript;
            obj.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
}
