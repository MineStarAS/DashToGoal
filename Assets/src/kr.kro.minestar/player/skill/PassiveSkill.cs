using System;

namespace src.kr.kro.minestar.player.skill
{
    public abstract class PassiveSkill : Skill
    {
        /// ##### Constructor #####
        protected PassiveSkill(Player player) : base(player)
        {
        }

        protected override void SkillFunction() // 추상 메소드 정의 - 손준호
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