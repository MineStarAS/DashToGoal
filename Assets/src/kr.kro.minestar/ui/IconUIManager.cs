using src.kr.kro.minestar.player;
using src.kr.kro.minestar.utility;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace src.kr.kro.minestar.ui
{
    public sealed class IconUIManager : MonoBehaviour
    {
        public SkillIconUI PassiveSkillUI { get; private set; }
        public SkillIconUI ActiveSkill1UI { get; private set; }
        public SkillIconUI ActiveSkill2UI { get; private set; }

        public List<EffectIconUI> EffectUIList { get; private set; }
        public Canvas Canvas { get; private set; }
        public Player Player { get; private set; }

        private void Start()
        {
            Canvas = FindObjectOfType<Canvas>();
            Player = FindObjectOfType<Player>();

            GameObject passiveGameObject = new GameObject("PassiveSkillUI");
            GameObject active1GameObject = new GameObject("ActiveSkill1UI");
            GameObject active2GameObject = new GameObject("ActiveSkill2UI");

            passiveGameObject.transform.SetParent(gameObject.transform);
            active1GameObject.transform.SetParent(gameObject.transform);
            active2GameObject.transform.SetParent(gameObject.transform);

            PassiveSkillUI = passiveGameObject.AddComponent<SkillIconUI>();
            ActiveSkill1UI = active1GameObject.AddComponent<SkillIconUI>();
            ActiveSkill2UI = active2GameObject.AddComponent<SkillIconUI>();

            PassiveSkillUI.Init(Player.PlayerCharacter.PassiveSkill, 0);
            ActiveSkill1UI.Init(Player.PlayerCharacter.ActiveSkill1, 1);
            ActiveSkill2UI.Init(Player.PlayerCharacter.ActiveSkill2, 2);

            EffectUIList = new List<EffectIconUI>();
        }

        private void Update()
        {
            PassiveSkillUI.UpdateUI();
            ActiveSkill1UI.UpdateUI();
            ActiveSkill2UI.UpdateUI();

            foreach (EffectIconUI effectUI in EffectUIList)
            {
                effectUI.UpdateUI();
            }
        }
    }
}