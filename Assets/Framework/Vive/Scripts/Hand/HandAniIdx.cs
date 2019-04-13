using UnityEngine;

namespace Vive
{
    public enum EnHandAni
    {
        Type0 = 0,//帽子
        Type1,//手电筒
        Type2,//鞋子
		Max
    }

    public class HandAniIdx : MonoBehaviour
    {
        public EnHandAni m_handAni = EnHandAni.Type0;
    }
}
