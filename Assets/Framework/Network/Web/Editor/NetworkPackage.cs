
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/19 14:43:08
 *  
 */
#endregion


using Framework.Unity.Editor;
using System.Collections.Generic;
using UnityEditor;

namespace Framework.Network.Web.Editor
{
    public class NetworkPackage
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Unity Messages
        //    void Awake()
        //    {
        //
        //    }
        //    void OnEnable()
        //    {
        //
        //    }
        //
        //    void Start() 
        //    {
        //    
        //    }
        //    
        //    void Update() 
        //    {
        //    
        //    }
        //
        //    void OnDisable()
        //    {
        //
        //    }
        //
        //    void OnDestroy()
        //    {
        //
        //    }

        #endregion

        #region Private Methods
        [MenuItem("Tools/Export Package/网络扩展包")]
        static void OneKeyExportNetwork()
        {
            string targetPackageName = "NetworkKit.unitypackage";
            List<string> filterArr = new List<string>
            {
                "Assets/WSATestCertificate.pfx",
                "Assets/Best HTTP (Pro)/",
                "Assets/Framework/Network/",
                "Assets/Plugins/KeJun/NetworkExt/",
                "Assets/Plugins/LitJson.dll",
                "Assets/StreamingAssets/WebConfig.txt"
            };

            ExportPackage.OneKeyExportPackage(targetPackageName, ExportPackageOptions.Interactive, filterArr);
        }
        #endregion

        #region Protected & Public Methods

        #endregion
    }
}