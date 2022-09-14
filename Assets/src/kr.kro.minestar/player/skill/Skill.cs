using UnityEngine;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class Skill
    {
        /// ##### Field #####
        public Player Player { get; protected set; }
        
        public string Name { get; protected set; }
        
        public string Description { get; protected set; }

        /// ##### Constructor #####
        protected Skill(Player player)
        {
            Player = player;
        }

        /// ##### Functions #####

        public abstract bool UseSkill(Player player);

        protected abstract bool CanUseSkill();
    }
}