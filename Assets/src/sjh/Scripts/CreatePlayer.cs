using src.kr.kro.minestar.player;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    [SerializeField] GameObject[] m_gPlayer;
    [SerializeField] GameObject m_gCamera;
    [SerializeField] Sprite[] m_gGame;
    bool m_IsPlaying = false;


    public bool IsPlaying { get => m_IsPlaying; set => m_IsPlaying = value; }


    private void Start()
    {
        GameObject gObj = null;
        if (GameObject.Find("CharacterSystem") == null)
        {
            SceneManager.LoadScene("CharacterSelect");
            return;
        }

        CharacterManager CM = GameObject.Find("CharacterSystem").gameObject.GetComponent<CharacterManager>();
        switch (CM.ePlayer)
        {
            case PlayerCharacterEnum.MineStar:
                gObj = Instantiate(m_gPlayer[(int)PlayerCharacterEnum.MineStar]);
                m_IsPlaying = true;
                break;
            case PlayerCharacterEnum.SonJunHo:
                gObj = Instantiate(m_gPlayer[(int)PlayerCharacterEnum.SonJunHo]);
                m_IsPlaying = true;
                break;
        }
        gObj.transform.position = new Vector3(0, 0, 0);
        m_gCamera.GetComponent<CameraController>().func_ChasePlayer();
    }

}
