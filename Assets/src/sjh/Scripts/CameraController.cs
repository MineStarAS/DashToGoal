using src.kr.kro.minestar.player;
using src.kr.kro.minestar.utility;
using UnityEngine;

namespace src.sjh.Scripts
{
    public class CameraController : MonoBehaviour
    {
        private GameObject PlayerGameObject { get; set; }

        private void Start()
        {
        }

        private void LateUpdate()
        {
            try
            {
                if (PlayerGameObject == null) PlayerGameObject = FindObjectOfType<Player>().gameObject;    
            }
            catch
            {
                // ignored
            }


            Vector3 pos = PlayerGameObject.transform.position;
            pos.z = -10;
            Camera.main.transform.position = pos;
        }
    }
}
