using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using UnityEngine;

// ReSharper disable All

namespace src.kr.kro.minestar.player.character
{
    public class PcMineStar : PlayerCharacter
    {
        private void Start()
        {
            PassiveSkill = gameObject.AddComponent<PsSpeedy>();
            ActiveSkill1 = gameObject.AddComponent<AsDash>();
            ActiveSkill2 = gameObject.AddComponent<AsSuperJump>();
        }
    }

    public class PsSpeedy : PassiveSkill
    {
        private void Start()
        {
            Player = gameObject.GetComponent<Player>();
            Name = "Speedy";
            Description = "I'm FAST!!!";

            Effects = new Effect[] { new Speed(Player) };
            SetDetectEvent<PlayerUseActiveSkill1Event>();
            Debug.Log("PsSpeedy Start");
        }

        public override bool UseSkill(Player player)
        {
            new PlayerUsePassiveSkillEvent(player, this);
            return true;
        }

        protected override bool CanUseSkill() => true;
    }

    public class AsDash : ActiveSkill
    {
        private void Start()
        {
            Name = "Dash";
            Description = "I will dash\n" +
                          "to the goal";
            Init(20F, 10F);
        }


        public override bool UseSkill(Player player)
        {
            if (!CanUseSkill()) return false;
            player.GetPlayerMove().AddMovementFlip(5F, 2F);

            StartTimer(DefaultCoolTime);
            return true;
        }
    }

    public class AsSuperJump : ChargeActiveSkill
    {
        private void Start()
        {
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


        public override bool UseSkill(Player player)
        {
            if (!CanUseSkill()) return false;

            player.GetPlayerMove().AddMovement(0F, 50F);

            Debug.Log($"{Name} Used");
            return true;
        }
    }
}