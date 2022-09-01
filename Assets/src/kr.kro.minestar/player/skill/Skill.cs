namespace src.kr.kro.minestar.player.skill
{
    public abstract class Skill
    {
        public abstract void UseSkill();

        protected abstract bool CanUseSkill();
    }
}