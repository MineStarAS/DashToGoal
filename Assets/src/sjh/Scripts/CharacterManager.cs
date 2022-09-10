using System.Collections;
using System.Collections.Generic;
using src.kr.kro.minestar.player;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;
    PlayerCharacterEnum m_ePlayer;

    public PlayerCharacterEnum ePlayer { get => m_ePlayer; set => m_ePlayer = value; }

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public CharacterManager Instance
    {
        get
        {
            if(null == instance) return null;
            return instance;
        }
    }

    public void func_SetPlayerEnum(PlayerCharacterEnum argEnum)
    {
        m_ePlayer = argEnum;
    }
}
