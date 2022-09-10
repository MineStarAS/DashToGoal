using System.Diagnostics.CodeAnalysis;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using UnityEngine;
// ReSharper disable All

namespace src.kr.kro.minestar.player.character
{
    public class PcMineStar : PlayerCharacter
    {
        
        [SuppressMessage("ReSharper", "Unity.IncorrectMonoBehaviourInstantiation")]
        public PcMineStar(Player player)
        {
            PassiveSkill = new PsSpeedy(player);
            ActiveSkill1 = Skill.Instantiate<ActiveSkill>(player.GetComponent<AsDash>());
            ActiveSkill2 = new AsSuperJump(20, 10);
        }
    }
    public class PsSpeedy : PassiveSkill
    {
        public PsSpeedy(Player player)
        {
            SetPlayer(player);
            SetName("Speedy");
            SetDescription("I'm FAST!!!");
            
            SetEffects(new Effect[]{new Speed(player)});
            SetDetectEvent<PlayerUseActiveSkill1Event>();
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
            SetName("Dash");
            SetDescription("I will dash\n" +
                           "to the goal");
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
        public AsSuperJump(float startCoolTime, float defaultCoolTime)
        {
            SetName("SuperJump");
            SetDescription("I will jump\n" +
                           "to the goal");

            SetStartChargeAmount(0);
            SetMaxChargeAmount(10);
            SetUseChargeAmount(10);
            SetChargingAmount(1);
            SetDetectEvent<PlayerJumpEvent>();
        }


        public override bool UseSkill(Player player)
        {
            if (!CanUseSkill()) return false;

            player.GetPlayerMove().AddMovement(0F, 50F);
            
            Debug.Log($"{GetName()} Used");
            return true;
        }
    }
}