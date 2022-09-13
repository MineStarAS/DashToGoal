using System;
using System.Collections;
using System.Threading;
using src.kr.kro.minestar.gameEvent;
using UnityEngine;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class ActiveSkill : Skill
    {
        /// ##### Field #####
        public int DefaultCoolTime { get; private set; }

        public int CurrentCoolTime { get; private set; }

        /// ##### Functions #####
        protected virtual void Init(float startCoolTime, float defaultCoolTime)
        {
            gameObject.AddComponent<Skill>();
            Player = gameObject.GetComponent<Player>();

            var startAmount = Convert.ToInt32(Math.Round(startCoolTime, 2) * 100);
            var defaultAmount = Convert.ToInt32(Math.Round(defaultCoolTime, 2) * 100);

            DefaultCoolTime = defaultAmount;
            CurrentCoolTime = startAmount;

            StartTimer(startAmount);
        }

        protected void StartTimer(int coolTime)
        {
            CurrentCoolTime = coolTime;
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            while (CurrentCoolTime >= 0)
            {
                CurrentCoolTime -= 1;
                yield return new WaitForSeconds(0.01F);
            }
            Debug.Log($"{Name} activate");
        }

        protected override bool CanUseSkill() => CurrentCoolTime <= 0;
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


        /// ##### Functions #####
        protected override void Init(float startCoolTime, float defaultCoolTime)
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