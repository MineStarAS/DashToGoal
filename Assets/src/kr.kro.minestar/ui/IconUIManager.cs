using src.kr.kro.minestar.player;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace src.kr.kro.minestar.ui
{
    public class IconUIManager : MonoBehaviour
    {
        SkillIconUI PassiveSkillUI;
        SkillIconUI ActiveUSkillI1;
        SkillIconUI ActiveUSkillI2;

        List<EffectIconUI> EffectUIList;
        [HideInInspector] public GameObject _canvas { get; set; }
        private Player _player;
        int m_skillCount;
        private void Start()
        {
            m_skillCount = 2;
            _canvas = GameObject.FindGameObjectWithTag("Canvas");
            _player = GetComponent<Player>();
            PassiveSkillUI = new SkillIconUI(_player.PlayerCharacter.PassiveSkill, this, 0);

            for (int i = 1; i < m_skillCount+1; i++)
            {
                if(i==1)ActiveUSkillI1 = new SkillIconUI(_player.PlayerCharacter.ActiveSkill1, this, 1);
                if(i==2)ActiveUSkillI2 = new SkillIconUI(_player.PlayerCharacter.ActiveSkill2, this, 2);
            }

            EffectUIList = new List<EffectIconUI>();
        }
        
        //private 

        private void Update()
        {
            //PassiveSkillUI.UpdateUI();
            //ActiveUSkillI1.UpdateUI();
            //ActiveUSkillI2.UpdateUI();
            //
            //foreach (EffectIconUI effectUI in EffectUIList)
            //{
            //    effectUI.UpdateUI();
            //}
        }
        
        
    }
}