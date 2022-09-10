using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using src.sjh.Scripts;
using UnityEngine;

namespace src.kr.kro.minestar.player
{
    public class Player :MonoBehaviour
    {
        [SerializeField] PlayerCharacterEnum m_enum;
        /// ##### Field #####
        private /*readonly*/ GameSystem _gameSystem;

        private /*readonly*/ PlayerCharacter _playerCharacter;

        private /*readonly*/ List<Effect> _effects = new ();
        [CanBeNull] private string _item;

        /// ##### Constructor #####
        /*
        public Player(GameSystem gameSystem, PlayerCharacterEnum playerCharacterEnum)
        {
            _gameSystem = gameSystem;
            _playerCharacter = PlayerCharacter.FromEnum(this, playerCharacterEnum);
            // _effects = new List<Effect>();
        }*/
        private void Start()
        {
            _gameSystem = GameObject.Find("GameManager").gameObject.GetComponent<GameSystem>();
            _playerCharacter = PlayerCharacter.FromEnum(this, m_enum);
            _effects = new List<Effect>();
        }

        /// ##### Get Functions #####
        public PlayerCharacter GetPlayerCharacter() => _playerCharacter;

        public List<Effect> GetEffects => _effects;

        public float GetMoveForce()
        {
            var value = 1f;

            if (_effects == null || _effects.Count == 0) return value;

            // Add Calculate
            foreach (var effect in _effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Add))
            {
                switch (effect.GetEffectType())
                {
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.Bondage:
                        return 0F;
                    case EffectType.BonusJump:
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            // Multi Calculate
            foreach (var effect in _effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Multi))
            {
                switch (effect.GetEffectType())
                {
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.Bondage:
                        return 0F;
                    case EffectType.BonusJump:
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            return value;
        }

        public float GetJumpForce()
        {
            var value = 1f;

            if (_effects == null || _effects.Count == 0) return value;
                
            foreach (var effect in _effects)
            {
                switch (effect.GetEffectType())
                {
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.Bondage:
                        return 0F;
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                    case EffectType.BonusJump:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            return value;
        }

        public GameSystem GetGameSystem() => _gameSystem;

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

        /// ##### Calculate Functions #####
        private static float Calculate(float value, Effect effect)
        {
            return effect.GetValueCalculator() switch
            {
                ValueCalculator.Add => value + effect.GetCalculatorValue(),
                ValueCalculator.Multi => value * effect.GetCalculatorValue(),
                _ => value
            };
        }

        private static int Calculate(int value, Effect effect)
        {
            return effect.GetValueCalculator() switch
            {
                ValueCalculator.Add => value + Convert.ToByte(effect.GetCalculatorValue()),
                ValueCalculator.Multi => value * Convert.ToByte(effect.GetCalculatorValue()),
                _ => value
            };
        }

        ///###### Do Functions #####

        public void DoUseActiveSkill1() // PlayerMove에서 사용할 수 있게 public 으로 수정 - 손준호
        {
            var skill = GetGetActiveSkill1();
            skill.UseSkill(this);

            var playerEvent = new PlayerUseActiveSkill1Event(this, skill);
        }

        public void DoUseActiveSkill2() // PlayerMove에서 사용할 수 있게 public 으로 수정 - 손준호
        {
            var skill = GetGetActiveSkill2();
            skill.UseSkill(this);

            var playerEvent = new PlayerUseActiveSkill1Event(this, skill);
        }


        /// ##### Other Functions #####
        public int LandingAirJumpAmountCharge()
        {
            var value = 1;

            // Add Calculate
            foreach (var effect in _effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Add))
            {
                switch (effect.GetEffectType())
                {
                    case EffectType.BonusJump:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                    case EffectType.Bondage:
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            // Multi Calculate
            foreach (var effect in _effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Multi))
            {
                switch (effect.GetEffectType())
                {
                    case EffectType.BonusJump:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                    case EffectType.Bondage:
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }
            
            return value;
        }
    }
}