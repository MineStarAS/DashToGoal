using System.Diagnostics;

namespace src.kr.kro.minestar.player.effect
{
    public class Speed : Effect, IEffectLimitTimer
    {
        private double _LimitTime;
        private double _currentTime;

        public Speed(Player player) : base(player)
        {
            EffectType = EffectType.MoveSpeed;

            Name = "Speed";
            Description = "FAST SPEED!!!";

            Value = 0.2F;
            _LimitTime = 5;
            _currentTime = 5;
        }

        double IEffectLimitTimer.LimitTime { get => _LimitTime; set => _LimitTime = value; }
        double IEffectLimitTimer.CurrentTime { get => _currentTime; set => _currentTime = value; }
    }
}