using System;
using System.Linq;
using src.kr.kro.minestar.player.effect;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class PassiveSkill : Skill
    {
        /// ##### Field #####
        private Effect[] _effects;

        private Type _detectEvent;

        /// ##### Getter #####
        public Effect[] GetEffects() => _effects.ToArray();

        public Type GetDetectEvent() => _detectEvent;

        /// ##### Setter #####
        public void SetEffects(Effect[] effects) => _effects = effects;

        protected void SetDetectEvent<T>() => _detectEvent = typeof(T);
    }
}