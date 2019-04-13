
#region 版权信息
/*
 * -----------------------------------------------------------
 *  Copyright (c) KeJun All rights reserved.
 * -----------------------------------------------------------
 *		描述: 
 *      创建者：陈伟超
 *      创建时间: 2019/04/09 19:24:45
 *  
 */
#endregion


using Framework.Unity.Tools;
using Framework.Unity.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Demo
{
    public class Test : MonoBehaviour
    {
        #region Fields
        
        #endregion

        #region Properties

        #endregion

        #region Unity Messages
        void Awake()
        {
            
            UIManager.GetInstance();
        }
        //    void OnEnable()
        //    {
        //
        //    }
        //
        void Start()
        {
            LoadScene();


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
        private void LoadScene()
        {
            UIManager.GetInstance().LoadScene("Scene2");
        }
        #endregion

        #region Protected & Public Methods

        #endregion
    }
}