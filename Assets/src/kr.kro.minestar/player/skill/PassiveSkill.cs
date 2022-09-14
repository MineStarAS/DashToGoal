using System;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class PassiveSkill : Skill
    {
        /// ##### Constructor #####
        protected PassiveSkill(Player player) : base(player)
        {
        }
    }
    
    public abstract class DetectPassiveSkill : PassiveSkill
    {
        /// ##### Constructor #####
        protected DetectPassiveSkill(Player player) : base(player)
        {
        }

        public Type DetectEvent { get; private set; }

        protected void SetDetectEvent<T>() => DetectEvent = typeof(T);
    }
}