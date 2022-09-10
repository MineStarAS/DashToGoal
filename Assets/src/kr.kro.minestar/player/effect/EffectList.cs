namespace src.kr.kro.minestar.player.effect
{
    public class Speed: TimerEffect
    {
        public Speed(Player player)
        {
            SetPlayer(player);
            SetEffectType(EffectType.FastMovement);
            
            SetName("Speed");
            SetDescription("FAST SPEED!!!");
            
            
            SetValueCalculator(ValueCalculator.Multi);
            SetCalculatorValue(1.15F);
            
            SetTime(10);

            AddEffect();
        }
    }
}