using src.kr.kro.minestar.player.effect;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class Skill
    {
        /// ##### Field #####
        public Player Player { get; protected set; }

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        /// ##### Constructor #####
        protected Skill(Player player)
        {
            Player = player;
            (this as ISkillCoolTime)?.Init();
        }

        /// ##### Functions #####
        public virtual bool UseSkill() // virtual 키워드 추가 - 손준호
        {
            if (!CanUseSkill()) return false;
            SkillFunction();
            return true;
        }

        protected abstract void SkillFunction(); // private -> protected - 손준호


        protected virtual bool CanUseSkill() // protected virtual 키워드 추가 - 손준호
        {
            if (!(this as ISkillCoolTime)?.CanUseSkill() ?? false) return false;

            return true;
        }
    }

    internal interface ISkillFunction
    {
        public void Init()
        {
        
        }

        public virtual bool CanUseSkill() => true;

        protected Skill GetSkill() => this as Skill ?? throw new InvalidCastException($"{GetType().Name} is not Skill.");

        public static int ConvertTime(double time) => time <= 0 ? 0 : Convert.ToInt32(Math.Round(time, 2) * 100);

        protected static double ConvertSecond(int time) => Math.Round(time / 100.0, 1);

        protected static float Calculate(float value, Effect effect)
        {
            return effect.Calculator switch
            {
                Calculator.Add => value + effect.CalculatorValue,
                Calculator.Multi => value * effect.CalculatorValue,
                _ => value
            };
        }

        protected static int Calculate(int value, Effect effect)
        {
            return effect.Calculator switch
            {
                Calculator.Add => value + Convert.ToByte(effect.CalculatorValue),
                Calculator.Multi => value * Convert.ToByte(effect.CalculatorValue),
                _ => value
            };
        }
    }

    internal interface ISkillCoolTime : ISkillFunction
    {
        protected double StartCoolTime { get; } // protected -> public - 손준호
        protected double DefaultCoolTime { get; } // protected -> public - 손준호

        protected int CurrentCoolTime { get; set; } // protected -> public - 손준호

        public Image SkillImage { get; set; }
        public Text CoolTimeText { get; set; }

        protected int GetCoolTime()
        {
            int value = ConvertTime(DefaultCoolTime);
            Dictionary<string, Effect>.ValueCollection effects = GetSkill().Player.Effects.Values;

            if (effects.Count == 0) return value;

            // Add Calculate
            foreach (Effect effect in effects.Where(effect => effect.Calculator == Calculator.Add))
            {
                switch (effect.EffectType)
                {
                    case EffectType.CoolTimeReduction:
                    case EffectType.CoolTimeIncrease:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                    case EffectType.Bondage:
                    case EffectType.BonusJump:
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            // Multi Calculate
            foreach (Effect effect in effects.Where(effect => effect.Calculator == Calculator.Multi))
            {
                switch (effect.EffectType)
                {
                    case EffectType.CoolTimeReduction:
                    case EffectType.CoolTimeIncrease:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                    case EffectType.Bondage:
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
        
        public void DoPassesTime()
        {
            if (CurrentCoolTime <= 0)
            {
                CoolTimeText.gameObject.SetActive(false);
                return;
            }

            CoolTimeText.gameObject.SetActive(true);
            CoolTimeText.text = ConvertSecond(CurrentCoolTime).ToString();
            CurrentCoolTime--;
            SetCoolTimePercent();
        }
        
        public void SetImageCoolTime(Image image, Text text)
        {
            SkillImage = image;
            CoolTimeText = text;
        }

        private void SetCoolTimePercent()
        {
            try
            {
                double value = DefaultCoolTime - (DefaultCoolTime - CurrentCoolTime);
                SkillImage.fillAmount = Convert.ToSingle(value / DefaultCoolTime);
            }
            catch (NullReferenceException)
            {
            }
        }

        public new bool CanUseSkill() => CurrentCoolTime <= 0;
    }
    
    internal interface ISkillDetectEvent : ISkillFunction
    {
        protected Type DetectEvent { get; set; }

        protected void DetectedEvent();
    }
    
    internal interface ISkillRangeDetect<T> : ISkillFunction
    {
        protected float DetectRadius { get; set; }

        protected Collider2D[] GetDetectedObject()
        {
            Vector3 position = GetSkill().Player.transform.position;
            return Physics2D.OverlapCircleAll(new Vector2(position.x, position.y), DetectRadius);
        }

        public new bool CanUseSkill() =>  GetDetectedObject().Select(collider => collider.GetComponent<T>()).Any(component => component != null);
        
    }
}