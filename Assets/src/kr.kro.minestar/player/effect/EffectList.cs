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
            
            SetTime(3);
        }
    }

    public class ABC : TimerEffect
    {
        public ABC(Player player)
        {
            Player = player;
            EffectType = EffectType.FastMovement;

            Name = "ABC";
            Description = "FAST SPEED!!!";


            Calculator = Calculator.Multi;
            CalculatorValue = 1.15F;

            SetTime(6);
        }
    }

    public class ABCD : TimerEffect
    {
        public ABCD(Player player)
        {
            Player = player;
            EffectType = EffectType.FastMovement;

            Name = "ABCD";
            Description = "FAST SPEED!!!";


            Calculator = Calculator.Multi;
            CalculatorValue = 1.15F;

            SetTime(12);
        }
    }

    public class ABCDE : TimerEffect
    {
        public ABCDE(Player player)
        {
            Player = player;
            EffectType = EffectType.FastMovement;

            Name = "ABCDE";
            Description = "FAST SPEED!!!";


            Calculator = Calculator.Multi;
            CalculatorValue = 1.15F;

            SetTime(8);
        }
    }
}