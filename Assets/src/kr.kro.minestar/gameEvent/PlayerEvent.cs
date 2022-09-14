using src.kr.kro.minestar.player;
using src.kr.kro.minestar.player.skill;

namespace src.kr.kro.minestar.gameEvent
{
    public abstract class PlayerEvent : GameEvent
    {
        /// ##### Field #####
        private Player _player;

        /// ##### Getter #####
        public Player GetPlayer() => _player;

        /// ##### Setter #####
        protected Player SetPlayer(Player player) => _player = player;
    }

    public class PlayerMoveEvent : PlayerEvent
    {
        /// ##### Constructor #####
        public PlayerMoveEvent(Player player)
        {
            SetPlayer(player);
            
            player.GameSystem.GameEventOperator.DoEvent(this);
        }
    }

    public class PlayerJumpEvent : PlayerEvent
    {
        /// ##### Constructor #####
        public PlayerJumpEvent(Player player)
        {
            SetPlayer(player);
            
            player.GameSystem.GameEventOperator.DoEvent(this);
        }
    }

    public class PlayerUsePassiveSkillEvent : PlayerEvent
    {
        /// ##### Field #####
        private readonly PassiveSkill _skill;

        /// ##### Getter #####
        public PassiveSkill GetPassiveSkill() => _skill;

        /// ##### Constructor #####
        public PlayerUsePassiveSkillEvent(Player player, PassiveSkill skill)
        {
            SetPlayer(player);
            _skill = skill;
            
            player.GameSystem.GameEventOperator.DoEvent(this);
        }
    }

    public class PlayerUseActiveSkill1Event : PlayerEvent
    {
        /// ##### Field #####
        private readonly ActiveSkill _skill;

        /// ##### Getter #####
        public ActiveSkill GetActiveSkill() => _skill;

        /// ##### Constructor #####
        public PlayerUseActiveSkill1Event(Player player, ActiveSkill skill)
        {
            SetPlayer(player);
            _skill = skill;
            
            player.GameSystem.GameEventOperator.DoEvent(this);
        }
    }

    public class PlayerUseActiveSkill2Event : PlayerEvent
    {
        /// ##### Field #####
        private readonly ActiveSkill _skill;

        /// ##### Getter #####
        public ActiveSkill GetActiveSkill() => _skill;

        /// ##### Constructor #####
        public PlayerUseActiveSkill2Event(Player player, ActiveSkill skill)
        {
            SetPlayer(player);
            _skill = skill;
            
            player.GameSystem.GameEventOperator.DoEvent(this);
        }
    }
}