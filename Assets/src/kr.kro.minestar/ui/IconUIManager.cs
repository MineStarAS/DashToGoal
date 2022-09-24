using src.kr.kro.minestar.player;
using src.kr.kro.minestar.utility;
using src.kr.kro.minestar.player.effect;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

namespace src.kr.kro.minestar.ui
{
    public sealed class IconUIManager : MonoBehaviour
    {
        public SkillIconUI PassiveSkillUI { get; private set; }
        public SkillIconUI ActiveSkill1UI { get; private set; }
        public SkillIconUI ActiveSkill2UI { get; private set; }
        public Dictionary<string, EffectIconUI> EffectUIMap { get; private set; }
        public Effect CurrnetEffect;
        public EffectIconUI CurrentEffectIconUI;
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

            EffectUIMap = new Dictionary<string, EffectIconUI>();
        }

        public void SetEffect(Effect effect)
        {
            //이펙트 지정
            CurrnetEffect = effect;
            Debug.Log(effect.Name);
            if (EffectUIMap.ContainsKey(effect.Name)) return;

            //이펙트UI 생성
            GameObject effectObj = new GameObject(CurrnetEffect.Name + "Effect");
            effectObj.transform.SetParent(gameObject.transform);
            CurrentEffectIconUI = effectObj.AddComponent<EffectIconUI>();
            CurrentEffectIconUI.Init(effect, Player, EffectUIMap.Count);

            //이펙트 딕셔너리에 추가
            EffectUIMap.Add(CurrnetEffect.Name, CurrentEffectIconUI);
        }

        public void RemoveEffectUI(Type type)
        {
            EffectUIMap.Remove(type.Name);
            GameObject RemoveEffectUI = GameObject.Find(type.Name + "Effect");
            Destroy(RemoveEffectUI);
        }

        private void Update()
        {
            PassiveSkillUI.UpdateUI();
            ActiveSkill1UI.UpdateUI();
            ActiveSkill2UI.UpdateUI();

            int i = 0;

            foreach(EffectIconUI kvp in EffectUIMap.Values)
            {
                kvp.UpdateUI(i);
                i++;
            }
        }
    }
}