using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.sjh.Scripts
{
    public class CreatePlayer : MonoBehaviour
    {
        [SerializeField] GameObject m_gCamera;
        [SerializeField] Sprite[] m_gGame;


        public bool IsPlaying { get; set; }


        private void Start()
        {
            if (GameObject.Find("CharacterSystem") == null)
            {
                SceneManager.LoadScene("CharacterSelect");
                return;
            }

            CharacterManager characterManager = GameObject.Find("CharacterSystem").gameObject.GetComponent<CharacterManager>();
            Instantiate(characterManager.PrefabPlayer).transform.position = Vector3.zero;
            IsPlaying = true;
            // m_gCamera.GetComponent<CameraController>().func_ChasePlayer();
        }
    }
}