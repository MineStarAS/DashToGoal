using src.kr.kro.minestar.player;

namespace src.kr.kro.minestar.gameEvent.player
{
    public abstract class PlayerEvent : GameEvent
    {
        public readonly Player player;
    }
}