using src.kr.kro.minestar.gameEvent;

namespace src.kr.kro.minestar.player.skill
{
    public class ActiveSkillDash : ActiveSkill
    {
        public ActiveSkillDash()
        {
            SetName("Dash");
            SetDescription("I will dash\n" +
                           "to the goal");
            
            SetStartCoolTime(20F);
            SetDefaultCoolTime(10F);
        }


        public override void UseSkill(Player player)
        {
            if (!CanUseSkill()) return;
            // player.addVector();
        }
    }

    public class ActiveSkillSuperJump : ChargeActiveSkill
    {
        public ActiveSkillSuperJump()
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


        public override void UseSkill(Player player)
        {
            if (!CanUseSkill()) return;
        }
    }
}