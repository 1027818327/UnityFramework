
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2018/11/16 11:35:24
 *  
 */
#endregion


using AssetBundles;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.Unity.AssetBundles
{
    public class AssetbundlesMenuItemsExtend
    {
        #region Fields
        //资源存放路径
        static string AssetDir = Application.dataPath + "/AssetBundleRes";
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
        [MenuItem("Tools/AssetBundles/Delete AssetBundles")]
        private static void DeleteAllAssetBundles()
        {
            string outPath = string.Empty;
            outPath = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());
            Debug.Log("删除资源:" + outPath);
            if (!string.IsNullOrEmpty(outPath))
            {
                if (Directory.Exists(outPath))
                {
                    Directory.Delete(outPath, true);
                    File.Delete(outPath + ".meta");
                }
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 清除所有的AssetBundleName，由于打包方法会将所有设置过AssetBundleName的资源打包，所以自动打包前需要清理
        /// </summary>
        [MenuItem("Tools/AssetBundles/Clear All AssetBundleName")]
        static void ClearAssetBundlesName()
        {
            //获取所有的AssetBundle名称
            string[] abNames = AssetDatabase.GetAllAssetBundleNames();

            //强制删除所有AssetBundle名称
            for (int i = 0; i < abNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(abNames[i], true);
            }
        }

        /// <summary>
        /// 移除未使用的AssetBundleName
        /// </summary>
        [MenuItem("Tools/AssetBundles/Remove Unused AssetBundles")]
        static void RemoveUnusedAssetBundles()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        [MenuItem("Tools/AssetBundles/SetAssetBundlesName")]
        static void SetAssetBundlesName()
        {
            if (!Directory.Exists(AssetDir))
            {
                Directory.CreateDirectory(AssetDir);
            }

            //设置指定路径下所有需要打包的assetbundlename
            SetAssetBundlesName(AssetDir);

            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        /// <summary>
        /// 设置所有在指定路径下的AssetBundleName
        /// </summary>
        static void SetAssetBundlesName(string _assetsPath)
        {
            //先获取指定路径下的所有Asset，包括子文件夹下的资源
            DirectoryInfo dir = new DirectoryInfo(_assetsPath);
            FileSystemInfo[] files = dir.GetFileSystemInfos(); //GetFileSystemInfos方法可以获取到指定目录下的所有文件以及子文件夹

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] is DirectoryInfo)  //如果是文件夹则递归处理
                {
                    SetAssetBundlesName(files[i].FullName);
                }
                else if (!files[i].Name.EndsWith(".meta")) //如果是文件的话，则设置AssetBundleName，并排除掉.meta文件
                {
                    SetABName(files[i].FullName);     //逐个设置AssetBundleName
                }
            }

        }

        /// <summary>
        /// 设置单个AssetBundle的Name
        /// </summary>
        /// <param name="filePath"></param>
        static void SetABName(string assetPath)
        {
            string importerPath = "Assets" + assetPath.Substring(Application.dataPath.Length);  //这个路径必须是以Assets开始的路径
            AssetImporter assetImporter = AssetImporter.GetAtPath(importerPath);  //得到Asset

            string tempBuildPath = assetPath.Remove(0, AssetDir.Length);
            Debug.Log(tempBuildPath);
            if (tempBuildPath.Contains("None"))
            {
                assetImporter.assetBundleName = string.Empty;
                assetImporter.assetBundleVariant = string.Empty;
                return;
            }

            string tempName = assetPath.Substring(AssetDir.Length + 1);
            string tempAssetBundleName = tempName.Remove(tempName.LastIndexOf("."));
            assetImporter.assetBundleName = tempAssetBundleName.ToLower();    //最终设置assetBundleName
            assetImporter.assetBundleVariant = "ab";



            /*
            string tempName = assetPath.Substring(assetPath.LastIndexOf(@"\") + 1);
            string assetName = tempName.Remove(tempName.LastIndexOf(".")); //获取asset的文件名称
            assetImporter.assetBundleName = assetName;    //最终设置assetBundleName
            */
        }
        #endregion

        #region Protected & Public Methods

        #endregion
    }
}