using System;
using src.kr.kro.minestar.gameEvent;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class ActiveSkill : Skill
    {
        /// ##### Field #####
        private float _startCoolTime;

        private float _defaultCoolTime;
        private float _coolTime;

        /// ##### Getter #####
        public float GetStartCoolTime() => _startCoolTime;

        public float GetDefaultCoolTime() => _defaultCoolTime;

        /// ##### Setter #####
        protected void SetStartCoolTime(float value) => _startCoolTime = value;

        protected void SetDefaultCoolTime(float value) => _defaultCoolTime = value;

        /// ##### Functions #####
        protected ActiveSkill()
        {
            _coolTime = _startCoolTime;
        }

        public void StartTimer()
        {
            _coolTime -= 0.1F;
        }

        protected override bool CanUseSkill() => _coolTime <= 0.05F;
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

        protected Type GetDetectEvent() => _detectEvent;

        /// ##### Setter #####
        protected void SetStartChargeAmount(int value) => _startChargeAmount = value;

        protected void SetMaxChargeAmount(int value) => _maxChargeAmount = value;
        
        protected void SetUseChargeAmount(int value) => _useChargeAmount = value;
        
        protected void SetChargingAmount(int value) => _chargingAmount = value;

        protected void SetDetectEvent<T>() => _detectEvent = typeof(T);
        

        /// ##### Functions #####
        protected ChargeActiveSkill()
        {
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

    public class TestSkill : ChargeActiveSkill
    {
        public TestSkill()
        {
            SetStartCoolTime(20F);
            SetDefaultCoolTime(10F);

            SetStartChargeAmount(0);
            SetMaxChargeAmount(20);
            SetUseChargeAmount(5);
            SetChargingAmount(1);
            SetDetectEvent<PlayerJumpEvent>();
        }


        public override void UseSkill()
        {
        }
    }
}