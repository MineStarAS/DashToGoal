using UnityEngine;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class Skill: MonoBehaviour
    {
        /// ##### Field #####
        private Player _player;

        private string _name;
        private string _description;

        /// ##### Getter #####
        public Player GetPlayer() => _player;
        
        public string GetName() => _name;

        public string GetDescription() => _description;


        /// ##### Setter #####
        protected void SetPlayer(Player player) => _player = player;
        
        protected void SetName(string name) => _name = name;

        protected void SetDescription(string description) => _description = description;
        
        /// ##### Functions #####

        public abstract bool UseSkill(Player player);

        protected abstract bool CanUseSkill();
    }
}