using System.Collections;
using System.Collections.Generic;
using src.kr.kro.minestar.player;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public GameObject PrefabPlayer { get; private set; } 

    private void Awake()
    {
        if(null == _instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CharacterManager Instance => null == _instance ? null : _instance;

    public void SetPlayer(GameObject prefab)
    {
        if (prefab.GetComponent<Player>() == null) return;
        PrefabPlayer = prefab;
    }
}
