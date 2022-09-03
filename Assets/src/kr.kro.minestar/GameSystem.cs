using System.Collections.Generic;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player;

namespace src.kr.kro.minestar
{
    public class GameSystem
    {
        /// ##### Field #####
        public GameEventOperator GameEventOperator;

        public string PlayMap;
        
        public HashSet<Player> Players = new();

        /// ##### Constructor #####
        public GameSystem()
        {
            GameEventOperator = new GameEventOperator(this);
        }
    }
}