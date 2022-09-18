using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using Unity.VisualScripting;
using UnityEngine;

// ReSharper disable All

namespace src.kr.kro.minestar.player.character
{
    public class PcMineStar : PlayerCharacter
    {
        public PcMineStar(Player player) : base(player)
        {
            PassiveSkill = new PsSpeedy(player);
            ActiveSkill1 = new AsDash(player);
            ActiveSkill2 = new AsSuperJump(player);
            StartTimer();
        }
    }

    public class PsSpeedy : DetectPassiveSkill
    {
        public PsSpeedy(Player player) : base(player)
        {
            Player = player;
            Name = "Speedy";
            Description = "I'm FAST!!!";
            SetDetectEvent<PlayerUseActiveSkill1Event>();
        }

        public override bool UseSkill()
        {
            var effects = new Effect[] { new Speed(Player), new ABC(Player), new ABCD(Player), new ABCDE(Player) };
            
            foreach (var effect in effects)
            {
                Player.AddEffect(effect);
            }

            new PlayerUsePassiveSkillEvent(Player, this);
            return true;
        }

        protected override bool CanUseSkill() => true;
    }

    public class AsDash : ActiveSkill
    {
        public AsDash(Player player) : base(player)
        {
            Player = player;
            Name = "Dash";
            Description = "I will dash\n" +
                          "to the goal";

            Init(1F, 1F);
        }


        public override bool UseSkill()
        {
            if (!CanUseSkill()) return false;
            Player.Movement.SetMovement(0F, 0F);
            Player.Movement.SetDrag(0);
            Player.Movement.AddMovementFlip(30F, 20F);
            // Player.Movement.IsSkill = true;
            UsedSkill();
            return true;
        }
    }

    public class AsSuperJump : ChargeActiveSkill
    {
        public AsSuperJump(Player player) : base(player)
        {
            Player = player;
            Name = "SuperJump";
            Description = "I will jump\n" +
                          "to the goal";

            StartChargeAmount = 0;
            MaxChargeAmount = 10;
            UseChargeAmount = 10;
            ChargingAmount = 1;

            SetDetectEvent<PlayerJumpEvent>();

            Init(20, 10);
        }


        public override bool UseSkill()
        {
            if (!CanUseSkill()) return false;

            Player.Movement.AddMovement(0F, 20F);
            UsedSkill();
            return true;
        }
    }
}