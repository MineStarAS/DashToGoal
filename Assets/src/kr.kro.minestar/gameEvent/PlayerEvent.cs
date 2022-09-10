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
        /// ##### Field #####
        private readonly float _moveForce;

        /// ##### Getter #####
        public float GetMoveForce() => _moveForce;

        /// ##### Constructor #####
        public PlayerMoveEvent(Player player, float moveForce)
        {
            SetPlayer(player);
            _moveForce = moveForce;
            
            player.GetGameSystem().GameEventOperator.DoEvent(this);
        }
    }

    public class PlayerJumpEvent : PlayerEvent
    {
        /// ##### Field #####
        private readonly float _jumpForce;

        /// ##### Getter #####
        public float GetJumpForce() => _jumpForce;

        /// ##### Constructor #####
        public PlayerJumpEvent(Player player, float jumpForce)
        {
            SetPlayer(player);
            _jumpForce = jumpForce;
            
            player.GetGameSystem().GameEventOperator.DoEvent(this);
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
            
            player.GetGameSystem().GameEventOperator.DoEvent(this);
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
            
            player.GetGameSystem().GameEventOperator.DoEvent(this);
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
            
            player.GetGameSystem().GameEventOperator.DoEvent(this);
        }
    }
}