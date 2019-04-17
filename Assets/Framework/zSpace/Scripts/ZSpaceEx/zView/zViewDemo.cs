using System;
using UnityEngine;
using zSpace.zView;

namespace ZSpaceEx
{
    public class zViewDemo : MonoBehaviour
    {

        private ZView _zView = null;
        IntPtr connection;
        // Use this for initialization
        void Start()
        {
            _zView = GameObject.FindObjectOfType<ZView>();
            if (_zView == null)
            {
                Debug.LogError("Unable to find reference to zSpace.zView.ZView Monobehaviour.");
                this.enabled = false;
                return;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetKey(KeyCode.Z))
                { ChangeMode(); }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    SwitchToAugmentedRealityMode();
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    SwitchToStandardMode();
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl))
                    if (Input.GetKey(KeyCode.Z))
                    { ChangeMode(); }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    SwitchToAugmentedRealityMode();
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    SwitchToStandardMode();
                }
            }
        }
        public void ChangeMode()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();

            if (connection == IntPtr.Zero)
            {
                _zView.ConnectToDefaultViewer();
            }
            else
            {
                _zView.CloseConnection(
                    connection,
                    ZView.ConnectionCloseAction.None,
                    ZView.ConnectionCloseReason.UserRequested,
                    string.Empty);
            }
        }
        public void SwitchToAugmentedRealityMode()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();
            _zView.SetConnectionMode(connection, _zView.GetAugmentedRealityMode());
        }

        public void SwitchToStandardMode()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();
            _zView.SetConnectionMode(connection, _zView.GetStandardMode());
        }


        void Up()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();
            float overlayOffsetY = _zView.GetSettingFloat(connection, ZView.SettingKey.OverlayOffsetY);
            _zView.SetSetting(connection, ZView.SettingKey.OverlayOffsetY, overlayOffsetY + 1.0f);
        }

        void Down()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();
            float overlayOffsetY = _zView.GetSettingFloat(connection, ZView.SettingKey.OverlayOffsetY);
            _zView.SetSetting(connection, ZView.SettingKey.OverlayOffsetY, overlayOffsetY - 1.0f);
        }

        void Left()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();
            float overlayOffsetX = _zView.GetSettingFloat(connection, ZView.SettingKey.OverlayOffsetX);
            _zView.SetSetting(connection, ZView.SettingKey.OverlayOffsetX, overlayOffsetX - 1.0f);
        }

        void Right()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();
            float overlayOffsetX = _zView.GetSettingFloat(connection, ZView.SettingKey.OverlayOffsetX);
            _zView.SetSetting(connection, ZView.SettingKey.OverlayOffsetX, overlayOffsetX + 1.0f);
        }

        void ScaleUp()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();

            float overlayScaleX = _zView.GetSettingFloat(connection, ZView.SettingKey.OverlayScaleX);
            _zView.SetSetting(connection, ZView.SettingKey.OverlayScaleX, overlayScaleX + 0.1f);

            float overlayScaleY = _zView.GetSettingFloat(connection, ZView.SettingKey.OverlayScaleY);
            _zView.SetSetting(connection, ZView.SettingKey.OverlayScaleY, overlayScaleY + 0.1f);
        }
        void ScaleDown()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();

            float overlayScaleX = _zView.GetSettingFloat(connection, ZView.SettingKey.OverlayScaleX);
            _zView.SetSetting(connection, ZView.SettingKey.OverlayScaleX, overlayScaleX - 0.1f);

            float overlayScaleY = _zView.GetSettingFloat(connection, ZView.SettingKey.OverlayScaleY);
            _zView.SetSetting(connection, ZView.SettingKey.OverlayScaleY, overlayScaleY - 0.1f);
        }

        public void OpenZview()
        {
            IntPtr connection = _zView.GetCurrentActiveConnection();
            if (connection == IntPtr.Zero)
            {
                _zView.ConnectToDefaultViewer();
            }
        }
    }
}
