using UnityEngine;
using zSpace.Core;

namespace zSpace.SimsCommon
{
    public class EventCameraAssigner : MonoBehaviour
    {
        private void Start()
        {
            SetEventCam();
        }

        public void SetEventCam()
        {
            this._zCore = GameObject.FindObjectOfType<ZCore>();
            this._canvas = GetComponent<Canvas>();
            if (this._canvas && this._zCore != null)
            {
                this._canvas.worldCamera = this._zCore.GetCenterCamera();
            }
        }

        private ZCore _zCore;
        private Canvas _canvas;
    }
}