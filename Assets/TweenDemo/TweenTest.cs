
/// <summary>
/// TweenTest.cs
/// </summary>
/// <remarks>
/// 2019/11/27: 创建. 陈伟超 <br/>
/// </remarks>

using UnityEngine;
using DG;
using DG.Tweening;

namespace Assets.TweenDemo
{
    public class TweenTest : MonoBehaviour
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
            transform.parent.DOMove(new Vector3(5, 5, 0), 10f);
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
        [ContextMenu("Stop")]
        private void Stop()
        {
            transform.parent.DOKill();
        }
        #endregion

        #region Protected & Public Methods

        #endregion
    }
}