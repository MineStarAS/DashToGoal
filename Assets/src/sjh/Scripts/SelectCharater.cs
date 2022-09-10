using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using src.kr.kro.minestar.player;
using Unity.VisualScripting;

public class SelectCharater : MonoBehaviour
{
    [SerializeField] GameObject m_gManager;
    PlayerCharacterEnum m_ePlayer;
    CharacterManager m_CM;

    public void Start()
    {
        m_ePlayer = PlayerCharacterEnum.SonJunHo;
        m_CM = m_gManager.GetComponent<CharacterManager>();
    }

    public void func_ClickPlayer()
    {
        if(this.gameObject.name == "Btn_SJH")
        {
            m_ePlayer = PlayerCharacterEnum.SonJunHo;
        }
        else if (this.gameObject.name == "Btn_MineStar")
        {
            m_ePlayer = PlayerCharacterEnum.MineStar;
        }
        m_CM.func_SetPlayerEnum(m_ePlayer);
    }
}