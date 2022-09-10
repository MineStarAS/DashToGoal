using System.Collections.Generic;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player;
using UnityEngine;

namespace src.kr.kro.minestar
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField] GameObject m_gCamera;

        /// ##### Constructor #####
        private void Start()
        {
            GameEventOperator = new GameEventOperator(this);
            Players = new HashSet<Player>();
        }
        
        /// ##### Getter #####
        public GameEventOperator GameEventOperator { get; private set; }

        public HashSet<Player> Players { get; private set; }
    }
}