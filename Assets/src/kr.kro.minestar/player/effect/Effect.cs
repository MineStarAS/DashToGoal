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

    public enum Calculator
    {
        Add, // 덧셈
        Multi, // 곱셈
        NotOp, // 연산자가 아님
    }

    public abstract class Effect
    {
        /// ##### Field #####
        public Player Player { get; protected set; }

        public EffectType EffectType { get; protected set; }

        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public Calculator Calculator { get; protected set; }
        public float CalculatorValue { get; protected set; }

        /// ##### Functions #####
        public void AddEffect() => Player.AddEffect(this);

        public void RemoveEffect() => Player.RemoveEffect(this);
    }

    public abstract class TimerEffect : Effect
    {
        /// ##### Field #####
        private int _maxTime;

        private int _currentTime;

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
        }

        public void DoPassesTime()
        {
            if (_currentTime-- <= 0) RemoveEffect();
        }
        
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