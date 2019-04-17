//////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2018 zSpace, Inc.  All Rights Reserved.
//
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.EventSystems;
using zSpace.Core;

namespace zSpace.SimsCommon
{
    // [AddComponentMenu("Event/Standalone Input Module")]
    public class StylusInputModule : StandaloneInputModule
    {
        //////////////////////////////////////////////////////////////////
        // Public/Serialized Members
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// Objects on layers specified in the exclude mask will not receive input events.  By
        /// default nothing is excluded.
        /// </summary>
        [Tooltip("Objects on layers specified in the exclude mask will not receive input events.")]
        public LayerMask LayerExcludeMask = 0;

        /// <summary>
        /// The beam length to use to perform raycasts against objects in the scene. Also
        /// the fallback length to set the visualization to in case no objects hit.
        /// Defaults to 0.18f (default beam length) plus 0.02f (default helper length).
        /// </summary>
        [Tooltip("The beam length to use to perform physics raycasts against objects in the scene.")]
        public float BeamLength = 0.20f;

        //////////////////////////////////////////////////////////////////
        // Public Methods
        //////////////////////////////////////////////////////////////////

        protected StylusInputModule() { }

        public override void UpdateModule()
        {
            base.UpdateModule();
        }

        public override bool IsModuleSupported()
        {
            return true;
        }

        public override bool ShouldActivateModule()
        {
            return base.ShouldActivateModule();
        }

        public override void ActivateModule()
        {
            base.ActivateModule();
        }

        public override void DeactivateModule()
        {
            base.DeactivateModule();
        }

        public override void Process()
        {
            if (this._zCore == null)
            {
                this.GetZCoreReference();
            }

            SendUpdateEventToSelectedObject();

            ProcessStylusEvent();
        }

        /// <summary>
        /// Return the game object that is currently being interacted with.
        /// Returns null if there's nothing intersected.
        /// </summary>
        public GameObject GetCurrentIntersection()
        {
            return (this._currentEventData != null) ? this._currentEventData.pointerCurrentRaycast.gameObject : null;
        }

        /// <summary>
        /// Return distance from stylus tip to ui element being pointed to.  If no element is
        /// in line with the stylus ray, float.MaxValue is returned.
        /// </summary>
        public float GetStylusUiElementDistance()
        {
            PointerEventData stylusData = this._currentEventData;
            if (stylusData == null || stylusData.pointerCurrentRaycast.gameObject == null)
            {
                return float.MaxValue;
            }

            return this._uiHitDistance;
        }

        //////////////////////////////////////////////////////////////////
        // Private Methods
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// Process all mouse events.
        /// </summary>
        ///
        private void ProcessStylusEvent()
        {
            bool stylusButtonPressed = (this._zCore != null && this._zCore.IsTargetButtonPressed(ZCore.TargetType.Primary, 0));  // 是否中键按下
            //bool stylusButtonPressed = (this._zCore != null && this._zCore.IsTargetButtonPressed(ZCore.TargetType.Primary, ZSpaceEx.PenMgr.LeftKeyId));  // 是否左键按下
            bool buttonPressedThisFrame = (stylusButtonPressed && !this._stylusButtonWasPressed);
            bool buttonReleasedThisFrame = (!stylusButtonPressed && this._stylusButtonWasPressed);

            var stylusData = this.GetStylusPointerData(stylusButtonPressed);

            // If the stylus is not moving or is not pressed/released, we can process mouse events
            if (!this.UseStylus(buttonPressedThisFrame, buttonReleasedThisFrame, stylusData))
            {
                base.Process();
                return;
            }

            if (!this._uiOccluded)
            {
                this.ProcessStylusPress(stylusData, buttonPressedThisFrame, buttonReleasedThisFrame);
                {
                    // TODO: Wrap these with a timer that gets triggered on click, to delay the
                    // movement/drag.
                    this.ProcessMove(stylusData);
                    this.ProcessDrag(stylusData);
                }
            }

            // If we haven't done any events with the stylus this frame, we can process mouse events
            if (!this._stylusButtonWasPressed && !buttonPressedThisFrame && !buttonReleasedThisFrame) {
                base.Process();
            }

            this._stylusButtonWasPressed = stylusButtonPressed;
        }

        private void GetZCoreReference()
        {
            this._zCore = GameObject.FindObjectOfType<ZCore>();
        }

        private PointerEventData GetStylusPointerData(bool stylusButtonPressed)
        {
            if (this._zCore == null)
            {
                return null;
            }

            this._uiOccluded = false;
            PointerEventData stylusData;
            var stylusPosition = new Vector2(0, 0);
            ZCore.Pose pose = this._zCore.GetTargetPose(ZCore.TargetType.Primary, ZCore.CoordinateSpace.World);

            // Then try and find a UI hit at the stylus beam's intersection with zero parallax
            var intersectInfo = this._zCore.IntersectDisplay(pose);
            var viewportPosition = this._zCore.GetViewportPosition();
            var viewportSize = this._zCore.GetViewportSize();

            if (intersectInfo.Hit && this.BeamLength >= intersectInfo.Distance)
            {
                this._uiHitDistance = intersectInfo.Distance;

                RaycastHit raycastHit;
                this._uiOccluded = Physics.Raycast(
                    pose.Position, pose.Direction, out raycastHit, this._uiHitDistance * this._zCore.ViewerScale);

                var stylusPositionX = intersectInfo.X - viewportPosition.x;
                // Need to invert the stylus position since Unity's origin is at the top left
                // of page and ours is in bottom left of page.
                var stylusPositionY = viewportSize.y - (intersectInfo.Y - viewportPosition.y);
                stylusPosition = new Vector2(stylusPositionX, stylusPositionY);
            }

            if (this._currentEventData == null)
            {
                stylusData = new PointerEventData(eventSystem);
                stylusData.position = stylusPosition;
            }
            else
            {
                stylusData = this._currentEventData;
                stylusData.Reset();
            }

            stylusData.delta = stylusPosition - stylusData.position;
            stylusData.position = stylusPosition;

            if (!this._stylusButtonWasPressed || !stylusButtonPressed)
            {   // Then we are definitely not dragging, so pick a new raycast target
                eventSystem.RaycastAll(stylusData, m_RaycastResultCache);
                var raycast = FindFirstRaycast(m_RaycastResultCache);
                stylusData.pointerCurrentRaycast = raycast;
                m_RaycastResultCache.Clear();
            }   // If we aren't dragging, we want to maintain our same raycast target

            this._currentEventData = stylusData;
            return stylusData;
        }

        private Vector2 GetStylusPositionFromPlane(ZCore.Pose pose, Camera centerCamera)
        {
            Vector2 stylusPosition = new Vector2(0, 0);

            if (this._currentPlane.HasValue)
            {
                var poseRay = new Ray(pose.Position, pose.Direction);
                float rayDistance = 0.0f;
                if (this._currentPlane.Value.Raycast(poseRay, out rayDistance))
                {
                    var stylusPoint = poseRay.GetPoint(rayDistance);
                    stylusPosition = centerCamera.WorldToScreenPoint(stylusPoint);
                }
            }

            return stylusPosition;
        }

        private bool UseStylus(bool pressed, bool released, PointerEventData pointerData)
        {
            return (pressed || released || (pointerData != null && pointerData.IsPointerMoving()));
        }

        /// <summary>
        /// Process the current mouse press.
        /// </summary>
        private void ProcessStylusPress(PointerEventData pointerEvent, bool buttonPressedThisFrame, bool buttonReleasedThisFrame)
        {
            var currentRaycastTarget = pointerEvent.pointerCurrentRaycast.gameObject;

            // Bail if target object is on an excluded layer.
            if (currentRaycastTarget != null)
            {
                if (0 != (LayerExcludeMask & (1 << currentRaycastTarget.layer))) return;
            }

            if (buttonPressedThisFrame)
            {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

                DeselectIfSelectionChanged(currentRaycastTarget, pointerEvent);

                // search for the control that will receive the press
                // if we can't find a press handler set the press
                // handler to be what would receive a click.
                var newPressed = ExecuteEvents.ExecuteHierarchy(currentRaycastTarget, pointerEvent, ExecuteEvents.pointerDownHandler);

                // didnt find a press handler... search for a click handler
                if (newPressed == null)
                {
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentRaycastTarget);
                }

                float currentTime = Time.unscaledTime;

                if (newPressed == pointerEvent.lastPress)
                {
                    var timeDifference = currentTime - pointerEvent.clickTime;
                    pointerEvent.clickCount = (timeDifference < 0.3f) ? pointerEvent.clickCount + 1 : 1;

                    pointerEvent.clickTime = currentTime;
                }
                else
                {
                    pointerEvent.clickCount = 1;
                }

                pointerEvent.pointerPress = newPressed;
                pointerEvent.rawPointerPress = currentRaycastTarget;
                pointerEvent.clickTime = currentTime;

                // Save the drag handler as well
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentRaycastTarget);

                if (pointerEvent.pointerDrag != null)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
                }
            }

            // PointerUp notification
            if (buttonReleasedThisFrame)
            {
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                // see if we button up on the same element that we clicked on...
                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentRaycastTarget);

                // PointerClick and Drop events
                if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
                }
                else if (pointerEvent.pointerDrag != null)
                {
                    ExecuteEvents.ExecuteHierarchy(currentRaycastTarget, pointerEvent, ExecuteEvents.dropHandler);
                }

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;

                if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
                }

                pointerEvent.dragging = false;
                pointerEvent.pointerDrag = null;

                // redo pointer enter / exit to refresh state
                // so that if we moused over somethign that ignored it before
                // due to having pressed on something else
                // it now gets it.
                if (currentRaycastTarget != pointerEvent.pointerEnter)
                {
                    HandlePointerExitAndEnter(pointerEvent, null);
                    HandlePointerExitAndEnter(pointerEvent, currentRaycastTarget);
                }
            }
        }

        //////////////////////////////////////////////////////////////////
        // Private Members
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// Stylus input module looks for zcore in the current scene.  If zcore is not in the
        /// scene, instantiate it and assign this.
        /// </summary>
        private ZCore _zCore;

        private PointerEventData _currentEventData = null;
        private float _uiHitDistance = float.MaxValue;
        private bool _uiOccluded = false;
        private bool _stylusButtonWasPressed = false;
        private Plane? _currentPlane = null;
    }
}
