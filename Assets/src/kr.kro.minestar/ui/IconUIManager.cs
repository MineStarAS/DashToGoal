using src.kr.kro.minestar.player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace src.kr.kro.minestar.ui
{
    public class IconUIManager : MonoBehaviour
    {
        SkillIconUI PassiveSkillUI;
        SkillIconUI ActiveUSkillI1;
        SkillIconUI ActiveUSkillI2;

        List<EffectIconUI> EffectUIList;
        private Player _player;
        private void Start()
        {
            _player = GetComponent<Player>();
            PassiveSkillUI = new SkillIconUI(_player.PlayerCharacter.PassiveSkill);
            ActiveUSkillI1 = new SkillIconUI(_player.PlayerCharacter.ActiveSkill1);
            ActiveUSkillI2 = new SkillIconUI(_player.PlayerCharacter.ActiveSkill2);

            EffectUIList = new List<EffectIconUI>();
            
        }
        
        //private 

        private void Update()
        {
            PassiveSkillUI.UpdateUI();
            ActiveUSkillI1.UpdateUI();
            ActiveUSkillI2.UpdateUI();

            foreach (EffectIconUI effectUI in EffectUIList)
            {
                effectUI.UpdateUI();
            }
        }
        
        
    }
}