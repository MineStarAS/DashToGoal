using src.kr.kro.minestar.player;
using src.kr.kro.minestar.player.skill;

namespace src.kr.kro.minestar.gameEvent
{
    public abstract class PlayerEvent : GameEvent
    {
        public Player Player { get; protected set; }
    }

    public class PlayerMoveEvent : PlayerEvent
    {
        /// ##### Constructor #####
        public PlayerMoveEvent(Player player)
        {
            Player = player;
            
            player.GameSystem.GameEventOperator.DoEvent(this);
        }
    }

    public class PlayerJumpEvent : PlayerEvent
    {
        /// ##### Constructor #####
        public PlayerJumpEvent(Player player)
        {
            Player = player;
            
            player.GameSystem.GameEventOperator.DoEvent(this);
        }
    }

    public class PlayerUseSkillEvent : PlayerEvent
    {
        /// ##### Field #####
        public Skill Skill { get; }
        public SkillSlot SkillSlot { get; }

        /// ##### Constructor #####
        public PlayerUseSkillEvent(Player player, Skill skill)
        {
            Player = player;
            Skill = skill;

            if (Player.PlayerCharacter.PassiveSkill == skill) SkillSlot = SkillSlot.Passive;
            else if (Player.PlayerCharacter.ActiveSkill1 == skill) SkillSlot = SkillSlot.Active1;
            else SkillSlot = SkillSlot.Active2;
            
            player.GameSystem.GameEventOperator.DoEvent(this);
        }
    }
}