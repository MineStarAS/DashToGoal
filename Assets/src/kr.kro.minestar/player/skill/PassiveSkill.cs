using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;

namespace src.kr.kro.minestar.player.skill
{
    public class PassiveSkillSpeedy : PassiveSkill
    {
        public PassiveSkillSpeedy(Player player)
        {
            SetPlayer(player);
            SetName("Speedy");
            SetDescription("I'm FAST!!!");
            
            SetEffects(new Effect[]{new Speed(player)});
            SetDetectEvent<PlayerUseActiveSkill1Event>();
        }
        
        public override void UseSkill(Player player)
        {
            var playerEvent = new PlayerUsePassiveSkillEvent(player, this);
        }

        protected override bool CanUseSkill() => true;
    }
}