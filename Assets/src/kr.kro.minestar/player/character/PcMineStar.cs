using System;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using UnityEngine;

namespace src.kr.kro.minestar.player.character
{
    public class PcMineStar : PlayerCharacter
    {
        
        public PcMineStar(Player player)
        {
            SetPassiveSkill(new PsSpeedy(player));
            SetActiveSkill1(new AsDash());
            SetActiveSkill2(new AsSuperJump());
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
        public AsDash()
        {
            SetName("Dash");
            SetDescription("I will dash\n" +
                           "to the goal");
            
            SetStartCoolTime(20F);
            SetDefaultCoolTime(10F);
        }


        public override bool UseSkill(Player player)
        {
            if (!CanUseSkill()) return false;
            player.GetPlayerMove().AddMovementFlip(5F, 2F);
            
            player.GetGameSystem().GameEventOperator.DoEvent(new PlayerUseActiveSkill1Event(player, this));
            
            Debug.Log($"{GetName()} Used");
            return true;
        }
    }

    public class AsSuperJump : ChargeActiveSkill
    {
        public AsSuperJump()
        {
            SetName("SuperJump");
            SetDescription("I will jump\n" +
                           "to the goal");
            
            SetStartCoolTime(20F);
            SetDefaultCoolTime(10F);

            SetStartChargeAmount(0);
            SetMaxChargeAmount(10);
            SetUseChargeAmount(10);
            SetChargingAmount(1);
            SetDetectEvent<PlayerJumpEvent>();
        }


        public override bool UseSkill(Player player)
        {
            if (!CanUseSkill()) return false;

            player.GetPlayerMove().AddMovement(0F, 30F);
            
            Debug.Log($"{GetName()} Used");
            return true;
        }
    }
}