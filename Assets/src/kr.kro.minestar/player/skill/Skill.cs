using System;
using System.Linq;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class Skill
    {
        /// ##### Field #####
        private Player _player;

        private string _name;
        private string _description;

        /// ##### Getter #####
        public Player GetPlayer() => _player;
        
        public string GetName() => _name;

        public string GetDescription() => _description;


        /// ##### Setter #####
        protected void SetPlayer(Player player) => _player = player;
        
        protected void SetName(string name) => _name = name;

        protected void SetDescription(string description) => _description = description;
        
        /// ##### Functions #####

        public abstract void UseSkill(Player player);

        protected abstract bool CanUseSkill();
    }

    /// #########################
    /// ##### Passive Skill #####
    /// #########################
    public abstract class PassiveSkill : Skill
    {
        /// ##### Field #####
        private Effect[] _effects;

        private Type _detectEvent;

        /// ##### Getter #####
        public Effect[] GetEffects() => _effects.ToArray();

        public Type GetDetectEvent() => _detectEvent;

        /// ##### Setter #####
        protected void SetEffects(Effect[] effects) => _effects = effects;

        protected void SetDetectEvent<T>() => _detectEvent = typeof(T);
    }

    /// ########################
    /// ##### Active Skill #####
    /// ########################
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

        public Type GetDetectEvent() => _detectEvent;

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
}