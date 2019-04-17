
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/01/09 11:51:03
 *  
 */
#endregion

using Framework.Unity.Tools;
using UnityEditor;
using UnityEngine;
using zSpace.SimsCommon;

namespace ZSpaceEx.Editor
{
    public class GraphicAdapterEditor
    {
        #region Private Methods
        [MenuItem("zSpace/给物体适配")]
        private static void AddGameObejctAdapter()
        {
            GameObject[] tempArray = Selection.gameObjects;
            foreach (GameObject tempObj in tempArray)
            {
                Canvas tempCanvas = tempObj.GetComponent<Canvas>();
                if (tempCanvas != null)
                {
                    // UI适配
                    GameObjectUtils.EnsureComponent<WorldAutosizerCanceler>(tempObj);
                    GameObjectUtils.EnsureComponent<UICanvasAutosizer>(tempObj);
                    GameObjectUtils.EnsureComponent<EventCameraAssigner>(tempObj);
                }
                else
                {
                    // 模型适配
                    GameObjectUtils.EnsureComponent<WorldAutosizerCanceler>(tempObj);
                }
            }
        }
        #endregion

        #region Protected & Public Methods

        #endregion
    }
}