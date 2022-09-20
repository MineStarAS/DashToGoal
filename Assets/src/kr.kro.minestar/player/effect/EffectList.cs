namespace src.kr.kro.minestar.player.effect
{
    public class Speed : Effect, IEffectLimitTimer
    {
        private double _currentTime;

        public Speed(Player player) : base(player)
        {
            EffectType = EffectType.MoveSpeed;

            Name = "Speed";
            Description = "FAST SPEED!!!";

            Value = 0.2F;
            _currentTime = 5;
        }

        double IEffectLimitTimer.LimitTime { get; set; }
        double IEffectLimitTimer.CurrentTime { get => _currentTime; set => _currentTime = value; }
    }
}