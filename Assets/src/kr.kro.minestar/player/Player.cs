using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using src.kr.kro.minestar.player.effect;
using src.sjh.Scripts;
using UnityEngine;

namespace src.kr.kro.minestar.player
{
    public class Player
    {
        /// ##### Constant Field #####
        private const float MoveForce = 6.0f;

        private const float JumpForce = 13.0f;

        /// ##### Field #####
        private readonly GameSystem _gameSystem;

        private readonly PlayerCharacter _playerCharacter;

        private int _airJumpAmount;
        private readonly List<Effect> _effects;
        [CanBeNull] private string _item;

        /// ##### Constructor #####
        public Player(GameSystem gameSystem, PlayerCharacterEnum playerCharacterEnum)
        {
            _gameSystem = gameSystem;
            _playerCharacter = PlayerCharacter.FromEnum(playerCharacterEnum);

            _airJumpAmount = 1;
            _effects = new List<Effect>();
        }

        /// ##### Get Functions #####
        public PlayerCharacter GetPlayerCharacter() => _playerCharacter;
        public List<Effect> GetEffects => _effects;

        public float GetMoveForce()
        {
            var value = MoveForce;

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
            var value = JumpForce;
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

        ///##### Effect Functions #####
        public void AddEffect(Effect effect)
        {
            _effects.Add(effect);
        }
        
        public void RemoveEffect(Effect effect)
        {
            _effects.Remove(effect);
        }

        /// ##### Functions #####
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

            _airJumpAmount = value;
            return value;
        }

        public void addVector(Vector2 vector)
        {
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
    }
}