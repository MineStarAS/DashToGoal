using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;

namespace src.kr.kro.minestar.player
{
    public class Player
    {
        /// ##### Constant Field #####
        private const float MoveForce = 6.0f;
        private const float JumpForce = 13.0f;
        
        /// ##### Field #####
        private readonly PlayerCharacter _playerCharacter;
        
        private readonly PassiveSkill _passiveSkill;
        private readonly ActiveSkill _activeSkill1;
        private readonly ActiveSkill _activeSkill2;

        private int _airJumpAmount;
        private readonly List<Effect> _effects; 
        [CanBeNull] private string _item;

        /// ##### Constructor #####
        public Player(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
            _passiveSkill = PlayerCharacterFunction.PassiveSkill(playerCharacter);
            _activeSkill1 = PlayerCharacterFunction.ActiveSkill1(playerCharacter);
            _activeSkill2 = PlayerCharacterFunction.ActiveSkill2(playerCharacter);

            _airJumpAmount = 1;
            _effects = new List<Effect>();
        }

        /// ##### Get Functions #####
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
        
        /// ##### Functions #####
        

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

    public enum PlayerCharacter
    {
        MineStar,
        SonJunHo
    }

    public static class PlayerCharacterFunction
    {
        public static string Name(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                PlayerCharacter.MineStar => "마인스타",
                PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }

        public static PassiveSkill PassiveSkill(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                // PlayerCharacter.MineStar => "마인스타",
                // PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }

        public static ActiveSkill ActiveSkill1(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                // PlayerCharacter.MineStar => "마인스타",
                // PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }

        public static ActiveSkill ActiveSkill2(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                // PlayerCharacter.MineStar => "마인스타",
                // PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }
    }
}