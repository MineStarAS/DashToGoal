using System.Collections;
using System.Collections.Generic;
using src.kr.kro.minestar.player;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    [SerializeField] GameObject[] m_gPlayer;
    [SerializeField] GameObject m_gCamera;
    [SerializeField] Sprite[] m_gGame;

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
                break;
            case PlayerCharacterEnum.SonJunHo:
                gObj = Instantiate(m_gPlayer[(int)PlayerCharacterEnum.SonJunHo]);
                break;
        }
        gObj.transform.position = new Vector3(0, 0, 0);
        m_gCamera.GetComponent<CameraController>().func_ChasePlayer();
    }
}
