using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Timer = System.Threading.Timer;

namespace src.kr.kro.minestar.player.effect
{
    public enum EffectType
    {
        ///Beneficial Effect
        FastMovement, // 증속
        BonusJump, // 추가 공중 점프
        SuperJump, // 점프력 증가

        ///Harmful Effect
        SlowMovement, // 감속
        JumpFatigue, // 점프감소
        Bondage, // 속박
        Disorder, // 혼란, 좌우 반전
    }

    public enum ValueCalculator
    {
        Add, // 덧셈
        Multi, // 곱셈
        NotOp, // 연산자가 아님
    }

    public abstract class Effect
    {
        /// ##### Field #####
        private Player _player;

        private EffectType _effectType;

        private string _name;
        private string _description;

        private ValueCalculator _valueCalculator;
        private float _calculatorValue;

        /// ##### Getter #####
        public Player GetPlayer() => _player;

        public EffectType GetEffectType() => _effectType;

        public string GetName() => _name;

        public string GetDescription() => _description;

        public ValueCalculator GetValueCalculator() => _valueCalculator;

        public float GetCalculatorValue() => _calculatorValue;

        /// ##### Setter #####
        protected void SetPlayer(Player player) => _player = player;

        protected void SetEffectType(EffectType effectType) => _effectType = effectType;

        protected void SetName(string name) => _name = name;

        protected void SetDescription(string description) => _description = description;

        protected void SetValueCalculator(ValueCalculator valueCalculator) => _valueCalculator = valueCalculator;

        protected void SetCalculatorValue(float value) => _calculatorValue = value;

        /// ##### Functions #####
        protected void AddEffect() => _player.AddEffect(this);

        protected void RemoveEffect() => _player.RemoveEffect(this);
    }

    public abstract class TimerEffect : Effect
    {
        /// ##### Field #####
        private int _maxTime;

        private int _currentTime;
        private Timer _timer;

        /// ##### Getter #####
        public double GetMaxTime() => Math.Round(Convert.ToDouble(_maxTime) / 100, 2);

        public double GetCurrentTime() => Math.Round(Convert.ToDouble(_currentTime) / 100);

        public double GetTimePercent() => Convert.ToDouble(_currentTime) / _maxTime;

        /// ##### Setter #####
        protected void SetTime(double time)
        {
            var value = Convert.ToInt32(Math.Round(time, 2) * 100);

            _maxTime = value;
            _currentTime = value;

            EnableTimer();
        }

        private IEnumerator EnableTimer()
        {
            while (_currentTime >= 0)
            {
                yield return new WaitForSeconds(0.01F);
                _currentTime -= 1;
            }

            RemoveEffect();
        }

        /// ##### Functions #####
        public void AddTime(double time)
        {
            if (time <= 0) return;
                
            var value = Convert.ToInt32(Math.Round(time, 2) * 100);

            _currentTime += value;
        }
        
        public void RemoveTime(double time)
        {
            if (time <= 0) return;
                
            var value = Convert.ToInt32(Math.Round(time, 2) * 100);

            _currentTime -= value;
        }
        
        public void MultiplyTime(double multiple)
        {
            if (multiple <= 0) return;

            _currentTime = Convert.ToInt32(_currentTime * multiple);
        }
    }
}