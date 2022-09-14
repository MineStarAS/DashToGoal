namespace src.kr.kro.minestar.player.effect
{
    public class Speed: TimerEffect
    {
        public Speed(Player player)
        {
            Player = player;
            EffectType =  EffectType.FastMovement;
            
            Name = "Speed";
            Description = "FAST SPEED!!!";
            
            
            Calculator = Calculator.Multi;
            CalculatorValue = 1.15F;
            
            SetTime(2);
        }
    }
}