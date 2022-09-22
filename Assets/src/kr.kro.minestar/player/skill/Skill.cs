using src.kr.kro.minestar.gameEvent;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace src.kr.kro.minestar.player.skill
{
    public enum SkillSlot { Passive, Active1, Active2 }

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
            (this as ISkillTimer)?.Init();
        }

        /// ##### Functions #####
        public void UseSkill()
        {
            if (!CanUseSkills()) return;
            // ReSharper disable once ObjectCreationAsStatement
            new PlayerUseSkillEvent(Player, this);
            SkillFunction();
            UsedSkills();
        }

        protected abstract void SkillFunction();

        private bool CanUseSkills()
        {
            switch (true)
            {
                case true when !(this as ISkillCoolTime)?.CanUseSkill() ?? false:
                case true when !(this as ISkillRangeDetect<Object>)?.CanUseSkill() ?? false:
                case true when !(this as ISkillCharge)?.CanUseSkill() ?? false:
                    return false;
                default:
                    return true;
            }
        }

        private void UsedSkills()
        {
            (this as ISkillCoolTime)?.UsedSkill();
            (this as ISkillCharge)?.UsedSkill();
        }
    }

    internal interface ISkillFunction
    {
        public virtual void Init()
        {
        }

        public virtual bool CanUseSkill() => true;

        public virtual void UsedSkill()
        {
        }

        protected Skill GetSkill => this as Skill ?? throw new InvalidCastException($"{GetType().Name} is not Skill.");
    }

    internal interface ISkillCoolTime : ISkillFunction
    {
        public double DefaultCoolTime { get; protected set; }

        public double CurrentCoolTime { get; protected set; }

        public Image SkillImage1 { get; set; }

        public Image SkillImage2 { get; set; }
        public Text CoolTimeText { get; set; }

        protected int GetCoolTime()
        {
            double value = DefaultCoolTime;

            value += value * GetSkill.Player.Effects.ValueCoolTime;

            return Convert.ToInt32(value);
        }

        public void DoPassesTime()
        {
            if (CurrentCoolTime <= 0)
            {
                CoolTimeText.gameObject.SetActive(false);
                return;
            }

            CoolTimeText.gameObject.SetActive(true);
            CoolTimeText.text = Math.Round(CurrentCoolTime).ToString(CultureInfo.InvariantCulture);
            CurrentCoolTime -= 0.01;
            SetCoolTimePercent();
        }

        public void SetImageCoolTime(Image image1, Image image2, Text text)
        {
            SkillImage1 = image1;
            SkillImage2 = image2;
            CoolTimeText = text;
        }

        private void SetCoolTimePercent()
        {
            try
            {
                float value = Convert.ToSingle((DefaultCoolTime - (DefaultCoolTime - CurrentCoolTime)) / DefaultCoolTime);
                SkillImage1.fillAmount = value;
                SkillImage2.fillAmount = value;
            }
            catch (NullReferenceException)
            {
            }
        }

        public new bool CanUseSkill() => CurrentCoolTime <= 0;

        public new void UsedSkill() => CurrentCoolTime = GetCoolTime();
    }

    internal interface ISkillTimer : ISkillFunction
    {
        protected float PeriodTime { get; set; }
        protected Coroutine Coroutine { get; set; }

        public new void Init() => StartTimer();


        private void StartTimer()
        {
            if (PeriodTime <= 0) PeriodTime = 0.01F;

            Skill skill = GetSkill;
            Coroutine = skill.Player.StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                PeriodFunction();
                yield return new WaitForSeconds(PeriodTime);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public void StopTimer()
        {
            Skill skill = GetSkill;
            skill.Player.StopCoroutine(Coroutine);
        }

        public void PeriodFunction();
    }

    internal interface ISkillDetectEvent : ISkillFunction
    {
        public Type DetectEvent { get; protected set; }

        public void DetectedEvent(GameEvent gameEvent);
    }

    internal interface ISkillRangeDetect<out T> : ISkillFunction
    {
        public float DetectRadius { get; protected set; }

        protected Collider2D[] GetDetectedObject()
        {
            Vector3 position = GetSkill.Player.transform.position;
            // ReSharper disable once Unity.PreferNonAllocApi
            return Physics2D.OverlapCircleAll(new Vector2(position.x, position.y), DetectRadius);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public new bool CanUseSkill() => GetDetectedObject().Select(collider => collider.GetComponent<T>()).Any(component => component != null);
    }

    internal interface ISkillCharge : ISkillFunction
    {
        public int CurrentCharge { get; protected set; }

        public int ChargeMax { get; protected set; }

        public int ChargeUsage { get; protected set; }

        public bool DoCharge(int value)
        {
            if (CurrentCharge == ChargeMax) return false;

            if (ChargeMax <= CurrentCharge + value)
            {
                CurrentCharge = ChargeMax;
                return false;
            }

            if (CurrentCharge + value <= 0)
            {
                CurrentCharge = 0;
                return true;
            }

            CurrentCharge += value;
            return true;
        }

        public new void UsedSkill()
        {
            if (CurrentCharge - ChargeUsage <= 0) CurrentCharge = 0;
            else CurrentCharge -= ChargeUsage;
        }

        public float GetChargePercent() => Convert.ToSingle(CurrentCharge) / ChargeMax;

        public new bool CanUseSkill() => ChargeUsage <= CurrentCharge;
    }

    internal interface ISkillStack : ISkillFunction
    {
        public int CurrentStack { get; protected set; }

        public int StackMax { get; protected set; }

        public bool DoStack(int value)
        {
            if (CurrentStack == StackMax) return false;

            if (StackMax <= CurrentStack + value)
            {
                CurrentStack = StackMax;
                return false;
            }

            if (CurrentStack + value <= 0)
            {
                CurrentStack = 0;
                return true;
            }

            CurrentStack += value;
            return true;
        }
    }
}