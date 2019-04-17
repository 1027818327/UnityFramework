using Framework.Unity.Mouse;
using UnityEngine;
using zSpace.Core;
using zSpace.SimsCommon;

namespace ZSpaceEx
{
    public class FollowPenMove : MonoBehaviour
    {
        public ZCore _zCore;
        public SampleStylusBeamRaycaster mStylusBeamRaycaster;
        private StylusState _stylusState = StylusState.Idle;

        public PenSimuMouseClick mClickEvent = new PenSimuMouseClick();

        void Start()
        {
            if (_zCore == null)
            {
                _zCore = FindObjectOfType<ZCore>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            bool tempPress = _zCore.IsTargetButtonPressed(ZCore.TargetType.Primary, PenMgr.LeftKeyId);
            if (!tempPress)
            {
                _stylusState = StylusState.Idle;
            }

            if (_stylusState == StylusState.Idle && tempPress)
            {
                _stylusState = StylusState.Press;

                ZCore.Pose pose = _zCore.GetTargetPose(ZCore.TargetType.Primary, ZCore.CoordinateSpace.World);
                RaycastHit currentHit;
                var raycastDistance = (mStylusBeamRaycaster.BeamLength + mStylusBeamRaycaster.HelperLength) * _zCore.ViewerScale;
                if (Physics.Raycast(pose.Position, pose.Direction, out currentHit, raycastDistance))
                {
                    if (currentHit.collider.CompareTag("Ground"))
                    {
                        if (mClickEvent != null)
                        {
                            mClickEvent.Invoke(currentHit);
                        }
                    }
                }
            }
        }
    }
}
