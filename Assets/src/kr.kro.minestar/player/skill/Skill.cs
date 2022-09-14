using src.kr.kro.minestar.player.effect;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class Skill
    {
        /// ##### Field #####
        public Player Player { get; protected set; }

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public int DefaultCoolTime { get; private set; }

        public int CurrentCoolTime { get; private set; }

        public Image SkillImage;
        public Text CoolTimeText;

        /// ##### Constructor #####
        protected Skill(Player player)
        {
            Player = player;
        }

        protected virtual void Init(double startCoolTime, double defaultCoolTime)
        {
            DefaultCoolTime = Convert.ToInt32(Math.Round(defaultCoolTime, 2) * 100);
            CurrentCoolTime = Convert.ToInt32(Math.Round(startCoolTime, 2) * 100);
        }
        
        /// ##### CoolTime Functions #####
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
                float value = DefaultCoolTime - (DefaultCoolTime - CurrentCoolTime);
                SkillImage.fillAmount = value / DefaultCoolTime;
            }
            catch (NullReferenceException)
            {
            }
        }

        private static double ConvertSecond(int time) => Math.Round(time / 100.0, 1);

        /// ##### Use Skill Functions #####

        public abstract bool UseSkill();

        protected void UsedSkill()
        {
            int value = DefaultCoolTime;
            Dictionary<string, Effect>.ValueCollection effects = Player.Effects.Values;

            if (effects.Count == 0)
            {
                CurrentCoolTime = value;
                return;
            }

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

            CurrentCoolTime = value;
        }

        protected virtual bool CanUseSkill() => CurrentCoolTime <= 0;


        private static float Calculate(float value, Effect effect)
        {
            return effect.Calculator switch
            {
                Calculator.Add => value + effect.CalculatorValue,
                Calculator.Multi => value * effect.CalculatorValue,
                _ => value
            };
        }

        private static int Calculate(int value, Effect effect)
        {
            return effect.Calculator switch
            {
                Calculator.Add => value + Convert.ToByte(effect.CalculatorValue),
                Calculator.Multi => value * Convert.ToByte(effect.CalculatorValue),
                _ => value
            };
        }
    }
}