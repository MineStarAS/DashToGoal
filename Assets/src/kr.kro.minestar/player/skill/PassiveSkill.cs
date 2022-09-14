using System;
using System.Linq;
using src.kr.kro.minestar.player.effect;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class PassiveSkill : Skill
    {
        /// ##### Constructor #####
        protected PassiveSkill(Player player) : base(player)
        {
        }
        /// ##### Field #####
        public Effect[] Effects
        {
            get => Effects.ToArray();
            protected set { }
        }

        public Type DetectEvent { get; private set; }

        protected void SetDetectEvent<T>() => DetectEvent = typeof(T);
    }
}