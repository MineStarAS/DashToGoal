using src.kr.kro.minestar.player;

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
        }
    }
}