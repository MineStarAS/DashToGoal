using src.kr.kro.minestar.utility;
using System;

namespace src.kr.kro.minestar.player.effect
{
    public enum EffectType
    {
        MoveSpeed,
        JumpForce,
        BonusAirJump,
        CoolTime,
        Bondage,
        Disorder,
    }

    public abstract class Effect
    {
        /// ##### Field #####
        public Player Player { get; protected set; }

        public EffectType EffectType { get; protected set; }

        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public float Value { get; protected set; }

        protected Effect(Player player)
        {
            Player = player;
            (this as IEffectLimitTimer)?.Init();
        }

        /// ##### Functions #####
        public void AddEffect() => Player.Effects.AddEffect(this);

        public void RemoveEffect() => Player.Effects.RemoveEffect(this);

        /// - 손준호 작업
        // Image m_EffectFillImage;
        // private bool m_IsEffectEnd;
        // public bool IsEffectEnd { get => m_IsEffectEnd; set => m_IsEffectEnd = value; }
        // public void SetImage(Image argImage) => m_EffectFillImage = argImage;
    }

    internal interface IEffectFunction
    {
        public virtual void Init()
        {
        }

        public virtual bool IsActivate() => true;

        protected Effect GetEffect => this as Effect ?? throw new InvalidCastException($"{GetType().Name} is not Effect.");
    }

    internal interface IEffectLimitTimer : IEffectFunction
    {
        public double LimitTime { get; protected set; }
        public double CurrentTime { get; protected set; }

        public new void Init()
        {
            LimitTime = CurrentTime;
        }

        public double GetTimePercent() => CurrentTime / LimitTime;

        public void DoPassesTime()
        {
            CurrentTime -= 0.01;
            if (CurrentTime <= 0) GetEffect.RemoveEffect();
        }
    }
}