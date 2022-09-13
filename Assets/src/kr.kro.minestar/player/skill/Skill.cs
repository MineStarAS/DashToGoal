using UnityEngine;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class Skill: MonoBehaviour
    {
        /// ##### Field #####
        public Player Player { get; protected set; }
        
        public string Name { get; protected set; }
        
        public string Description { get; protected set; }
        
        /// ##### Functions #####

        public abstract bool UseSkill(Player player);

        protected abstract bool CanUseSkill();
    }
}