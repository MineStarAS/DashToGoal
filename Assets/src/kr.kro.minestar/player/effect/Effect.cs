using System.Timers;

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
        private ValueCalculator _valueCalculator;

        private float _calculatorValue;

        /// ##### Getter #####
        public Player GetPlayer() => _player;
        
        public EffectType GetEffectType() => _effectType;

        public ValueCalculator GetValueCalculator() => _valueCalculator;
        
        public float GetCalculatorValue() => _calculatorValue;

        /// ##### Setter #####
        protected void SetPlayer(Player player) => _player = player;
        
        protected void SetEffectType(EffectType effectType) => _effectType = effectType;

        protected void SetValueCalculator(ValueCalculator valueCalculator) => _valueCalculator = valueCalculator;
        
        protected void SetCalculatorValue(float value) => _calculatorValue = value;

        /// ##### Functions #####
        protected void AddEffect() => _player.AddEffect(this);
        
    }

    public class TimerEffect : Effect
    {
        private float _time;
        private Timer _timer; //async

        public TimerEffect ()
        {
            
        }
    }
}