using System.Collections.Generic;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using src.sjh.Scripts;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable ObjectCreationAsStatement

namespace src.kr.kro.minestar.player
{
    public class Player : MonoBehaviour
    {
        /// ##### Field #####
        public GameSystem GameSystem { get; private set; }

        [SerializeField] PlayerCharacterEnum m_enum;
        public PlayerCharacter PlayerCharacter { get; private set; }
        public Dictionary<string, Effect> Effects{ get; private set; }
        public PlayerMove PlayerMove{ get; private set; }

        /// ##### Unity Functions #####
        private void Start()
        {
            Effects = new Dictionary<string, Effect>();
            GameSystem = GameObject.Find("GameManager").gameObject.GetComponent<GameSystem>();
            PlayerMove = gameObject.AddComponent<PlayerMove>();
            PlayerCharacter = PlayerCharacter.FromEnum(this, m_enum);

            GameSystem.Players.Add(this);
        }

        private void Update()
        {
            PlayerMove.DoJump(); // 점프
            PlayerMove.DoMove(); // 좌우 이동
            DoUseSkill();
        }

        private void FixedUpdate()
        {
            PlayerMove.FixedCheck();
        }


        /// ##### Get Functions #####
        public GameSystem GetGameSystem() => GameSystem;

        public PlayerCharacter GetPlayerCharacter() => PlayerCharacter;

        public PlayerMove GetPlayerMove() => PlayerMove;

        ///##### Effect Functions #####
        public void AddEffect(Effect effect)
        {
            if (Effects.ContainsKey(effect.Name)) Effects[effect.Name].RemoveEffect();
            Effects.Add(effect.Name, effect);
        }

        public void RemoveEffect(Effect effect) => Effects.Remove(effect.Name);
        

        // ReSharper disable Unity.PerformanceAnalysis
        ///###### Do Functions #####
        private void DoUseSkill()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                DoUseActiveSkill1();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                DoUseActiveSkill2();
            }
        }

        private void DoUseActiveSkill1()
        {
            ActiveSkill skill = PlayerCharacter.ActiveSkill1;
            if (skill.UseSkill()) new PlayerUseActiveSkill1Event(this, skill);
        }

        private void DoUseActiveSkill2()
        {
            ActiveSkill skill = PlayerCharacter.ActiveSkill2;
            if (skill.UseSkill()) new PlayerUseActiveSkill2Event(this, skill);
        }
    }
}