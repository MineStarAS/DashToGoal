using System;
using System.Collections;
using System.Threading;
using src.kr.kro.minestar.gameEvent;
using UnityEngine;
using UnityEngine.UI;
namespace src.kr.kro.minestar.player.skill
{ 
    public abstract class ActiveSkill : Skill
    {
        /// ##### Field #####

        Image m_ActiveImage;
        Text m_CooltimeText;

        public void SetImageCooltime(Image argImage, Text argText)
        {
            m_ActiveImage = argImage;
            m_CooltimeText = argText;
        }
        public int DefaultCoolTime { get; private set; }

        public int CurrentCoolTime { get; private set; }
        
        /// ##### Constructor #####
        protected ActiveSkill(Player player) : base(player)
        {
        }

        /// ##### Functions #####
        protected virtual void Init(double startCoolTime, double defaultCoolTime)
        {
            DefaultCoolTime = Convert.ToInt32(Math.Round(defaultCoolTime, 2) * 100);
            CurrentCoolTime = Convert.ToInt32(Math.Round(startCoolTime, 2) * 100);
        }

        public void DoPassesTime()
        {
            if (CurrentCoolTime <= 0)
            {
                m_CooltimeText.gameObject.SetActive(false);
                return;
            }
            m_CooltimeText.gameObject.SetActive(true);
            m_CooltimeText.text = CurrentCoolTime.ToString();
            CurrentCoolTime--;
            Set_FillAmount(DefaultCoolTime - (DefaultCoolTime - CurrentCoolTime));
        }

        protected void UsedSkill() => CurrentCoolTime = DefaultCoolTime;
        
        protected override bool CanUseSkill() => CurrentCoolTime <= 0;

        private void Set_FillAmount(float _value) => m_ActiveImage.fillAmount = _value / DefaultCoolTime;
    }

    public abstract class ChargeActiveSkill : ActiveSkill
    {
        /// ##### Field #####
        public int StartChargeAmount { get; protected set; } // 시작 충전량

        public int MaxChargeAmount { get; protected set; } // 최대 충전량
        public int UseChargeAmount { get; protected set; } // 1회 사용량
        public int ChargingAmount { get; protected set; } // 1회 충전량
        public int ChargedAmount { get; protected set; } // 현재 충전량

        public Type DetectEvent { get; private set; } // 충전 트리거 이벤트

        protected void SetDetectEvent<T>() => DetectEvent = typeof(T);
        
        /// ##### Constructor #####
        protected ChargeActiveSkill(Player player) : base(player)
        {
        }


        /// ##### Functions #####
        protected override void Init(double startCoolTime, double defaultCoolTime)
        {
            base.Init(startCoolTime, defaultCoolTime);
            ChargedAmount = MaxChargeAmount < StartChargeAmount ? MaxChargeAmount : StartChargeAmount;
        }

        public void DoCharge(GameEvent gameEvent)
        {
            if (DetectEvent != gameEvent.GetType()) return;

            if (ChargedAmount + ChargingAmount >= MaxChargeAmount) ChargedAmount = MaxChargeAmount;
            else ChargedAmount += ChargingAmount;
        }

        public void DoCharge(GameEvent gameEvent, int chargeValue)
        {
            if (DetectEvent != gameEvent.GetType()) return;

            if (ChargedAmount + chargeValue >= MaxChargeAmount) ChargedAmount = MaxChargeAmount;
            else ChargedAmount += chargeValue;
        }

        protected override bool CanUseSkill()
        {
            if (!base.CanUseSkill()) return false;
            return ChargedAmount >= UseChargeAmount;
        }
    }
}