using UnityEngine;

namespace src.sjh.Scripts
{
    public class CameraController : MonoBehaviour
    {
        GameObject m_gPlayer;
        private bool m_isChasing;

        private void LateUpdate()
        {
            if (!m_isChasing) return;
            Vector3 pos = m_gPlayer.transform.position;
            pos.z = -10;
            Camera.main.transform.position = pos;
        }

        public void func_ChasePlayer()
        {
            m_isChasing = true;
            m_gPlayer = GameObject.FindWithTag("Player");
        }
    }
}
