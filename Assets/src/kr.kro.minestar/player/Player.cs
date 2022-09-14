using System.Collections.Generic;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using src.sjh.Scripts;
using UnityEngine;

namespace src.kr.kro.minestar.player
{
    public class Player : MonoBehaviour
    {
        /// ##### Field #####
        private GameSystem _gameSystem;
        
        [SerializeField] PlayerCharacterEnum m_enum;
        private PlayerCharacter _playerCharacter;
        private List<Effect> _effects;
        private PlayerMove _playerMove;
        /// ##### Unity Functions #####
        private void Start()
        {
            Debug.Log("Hello");
            _effects = new List<Effect>();
            _gameSystem = GameObject.Find("GameManager").gameObject.GetComponent<GameSystem>();
            _playerMove = gameObject.AddComponent<PlayerMove>();
            _playerCharacter = PlayerCharacter.FromEnum(this, m_enum);
        }

        private void Update()
        {
            _playerMove.DoJump(); // 점프
            _playerMove.DoMove(); // 좌우 이동
            DoUseSkill();
        }

        private void FixedUpdate()
        {
            _playerMove.FixedCheck();
        }


        /// ##### Get Functions #####
        public GameSystem GetGameSystem() => _gameSystem;

        public PlayerCharacter GetPlayerCharacter() => _playerCharacter;

        public List<Effect> GetEffects() => new List<Effect>();

        public PlayerMove GetPlayerMove() => _playerMove;


        public PassiveSkill GetPassiveSkill() => _playerCharacter.GetPassiveSkill();
        public ActiveSkill GetGetActiveSkill1() => _playerCharacter.GetActiveSkill1();
        public ActiveSkill GetGetActiveSkill2() => _playerCharacter.GetActiveSkill2();

        ///##### Effect Functions #####
        public void AddEffect(Effect effect)
        {
            _effects.Add(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            _effects.Remove(effect);
        }

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
            var skill = GetGetActiveSkill1();
            if (skill.UseSkill(this)) _gameSystem.GameEventOperator.DoEvent(new PlayerUseActiveSkill1Event(this, skill));
        }

        private void DoUseActiveSkill2()
        {
            var skill = GetGetActiveSkill2();
            if (skill.UseSkill(this)) _gameSystem.GameEventOperator.DoEvent(new PlayerUseActiveSkill2Event(this, skill));
        }
    }
}