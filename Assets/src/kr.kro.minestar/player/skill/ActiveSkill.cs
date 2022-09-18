using System;
using src.kr.kro.minestar.gameEvent;
using UnityEngine.UI;

namespace src.kr.kro.minestar.player.skill
{ 
    public abstract class ActiveSkill : Skill
    {
        /// ##### Field #####
        
        /// ##### Constructor #####
        protected ActiveSkill(Player player) : base(player)
        {
            
        }
        public void SetImageCoolTime(Image argImage, Image argImage2, Text argText) // 메소드 정의 - 손준호
        {

        }

        public void DoPassesTime() // 추상 메소드 정의 - 손준호
        {

        }

        protected override void SkillFunction() // 추상 메소드 정의 - 손준호
        {

        }

        protected void UsedSkill() // 추상 메소드 정의 - 손준호
        {

        }

        protected void Init(double startCoolTime, double defaultCoolTime) // 추상 메소드 정의 - 손준호
        {
            
        }
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
        protected virtual new void Init(double startCoolTime, double defaultCoolTime) // virtual new 추가 - 손준호
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