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

        private Timer _timer;

        /// ##### Functions #####
        protected virtual void Init(float startCoolTime, float defaultCoolTime)
        {
            gameObject.AddComponent<Skill>();
            
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
                Debug.Log($"CoolTime: {CurrentCoolTime}");
                CurrentCoolTime -= 1;
                yield return new WaitForSeconds(0.01F);
            }
        }

        protected override bool CanUseSkill() => CurrentCoolTime <= 0;
    }

    public abstract class ChargeActiveSkill : ActiveSkill
    {
        /// ##### Field #####
        private int _startChargeAmount;

        private int _maxChargeAmount; // 최대 충전량
        private int _useChargeAmount; // 1회 사용량
        private int _chargingAmount; // 1회 충전량
        private int _chargedAmount; // 현재 충전량

        private Type _detectEvent; // 충전 트리거 이벤트

        /// ##### Getter #####
        public float GetStartChargeAmount() => _startChargeAmount;

        public float GetMaxChargeAmount() => _maxChargeAmount;

        public float GetUseChargeAmount() => _useChargeAmount;

        public float GetChargingAmount() => _chargingAmount;

        public Type GetDetectEvent() => _detectEvent;

        /// ##### Setter #####
        protected void SetStartChargeAmount(int value) => _startChargeAmount = value;

        protected void SetMaxChargeAmount(int value) => _maxChargeAmount = value;

        protected void SetUseChargeAmount(int value) => _useChargeAmount = value;

        protected void SetChargingAmount(int value) => _chargingAmount = value;

        protected void SetDetectEvent<T>() => _detectEvent = typeof(T);


        /// ##### Functions #####
        protected override void Init(float startCoolTime, float defaultCoolTime)
        {
            base.Init(startCoolTime, defaultCoolTime);
            _chargedAmount = _maxChargeAmount < _startChargeAmount ? _maxChargeAmount : _startChargeAmount;
        }

        public void DoCharge(GameEvent gameEvent)
        {
            if (_detectEvent != gameEvent.GetType()) return;

            if (_chargedAmount + _chargingAmount >= _maxChargeAmount) _chargedAmount = _maxChargeAmount;
            else _chargedAmount += _chargingAmount;
        }

        public void DoCharge(GameEvent gameEvent, int chargeValue)
        {
            if (_detectEvent != gameEvent.GetType()) return;

            if (_chargedAmount + chargeValue >= _maxChargeAmount) _chargedAmount = _maxChargeAmount;
            else _chargedAmount += chargeValue;
        }

        protected override bool CanUseSkill()
        {
            if (!base.CanUseSkill()) return false;
            return _chargedAmount >= _useChargeAmount;
        }
    }
}