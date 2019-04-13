using UnityEngine;
using Framework.Event;

namespace Vive
{
    /// <summary>
    /// 指引类
    /// </summary>
    public class GuidePost : MonoBehaviour
    {
        /// <summary>
        /// 起始点
        /// </summary>
        [Header("起始点")]
        public GameObject start;
        /// <summary>
        /// 终点
        /// </summary>
        [Header("终点")]
        public GameObject destination;
        /// <summary>
        /// 箭头
        /// </summary>
        [Header("箭头")]
        public GameObject mArrow;
        private Transform ArrowTrans
        {
            get
            {
                if (mArrow == null) return null;
                return mArrow.transform;
            }
        }

        void Awake()
        {
            EventManager.Instance.AddEventListener(LogicEventType.GuideChange, GuideChange);
        }

        void OnDestroy()
        {
            EventManager.Instance.RemoveEventListener(LogicEventType.GuideChange, GuideChange);
        }

        void Update()
        {
            if (start == null || destination == null)
            {
                mArrow.SetActive(false);
                return;
            }

            Vector3 InitPosition = new Vector3(start.transform.position.x, this.transform.position.y, start.transform.position.z);
            Vector3 DestinationPosition = new Vector3(destination.transform.position.x, this.transform.position.y, destination.transform.position.z);
            Vector3 point = DestinationPosition - InitPosition;
            Vector2 pos2 = new Vector2(point.x, point.z);
            Vector2 pos1 = new Vector2(1, 0);
            float z = GetRotateAngle(pos2.x, pos2.y, pos1.x, pos1.y);
            float width = pos2.magnitude;
            this.transform.position = InitPosition;
            ArrowTrans.localEulerAngles = new Vector3(0, z, 0);
            if (width < 1.53f)
            {
                mArrow.SetActive(false);
            }
            else
            {
                mArrow.SetActive(true);
            }
        }

        float GetRotateAngle(float x1, float y1, float x2, float y2)
        {
            float sin = x1 * y2 - x2 * y1;
            float cos = x1 * x2 + y1 * y2;
            return Mathf.Atan2(sin, cos) * (180 / Mathf.PI);
        }

        /// <summary>
        /// 接受到了指引事件
        /// </summary>
        /// <param name="varData"></param>
        public void GuideChange(EventData varData)
        {
            var tempData = varData as EventDataEx<GameObject>;
            if (tempData == null)
            {
                destination = null;
            }
            else
            {
                destination = tempData.GetData();
            }
        }
    }
}
