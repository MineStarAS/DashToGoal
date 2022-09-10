using System.Collections.Generic;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player;
using UnityEngine;

namespace src.kr.kro.minestar
{
    public class GameSystem : MonoBehaviour
    {
        /// ##### Field #####
        public readonly GameEventOperator GameEventOperator;

        //public string playMap;
        
        public readonly HashSet<Player> Players;

        /// ##### Constructor #####
        public GameSystem()
        {
            GameEventOperator = new GameEventOperator(this);
            Players = new HashSet<Player>();

            //var player = gameObject.AddComponent<Player>();

            //Players.Add(player);
        }
    }
}