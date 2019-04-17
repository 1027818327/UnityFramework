
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/17 14:53:11
 *  
 */
#endregion


using UnityEngine;

namespace Assets.Demo
{
    public class Scene2 : MonoBehaviour
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
        void Start()
        {
            SetCanvas();
        }
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
        private void SetCanvas()
        {
            GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;

            RectTransform tempRt = GetComponent<RectTransform>();

            tempRt.anchoredPosition = Vector2.zero;
            tempRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1920);
            tempRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1080);
            tempRt.anchorMin = Vector2.zero;
            tempRt.anchorMax = Vector2.zero;
            tempRt.pivot = 0.5f * Vector2.one;

            tempRt.rotation = Quaternion.identity;
            tempRt.localScale = Vector3.one;
        }
        #endregion

        #region Protected & Public Methods

        #endregion
    }
}