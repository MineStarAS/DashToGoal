namespace src.kr.kro.minestar.player.skill.type
{
    public abstract class CoolTimeTypeSkill
    {
        private readonly float startCoolTime;

        public readonly float defaultCoolTime;

        private float _coolTime;

        protected CoolTimeTypeSkill(float startCoolTime, float defaultCoolTime)
        {
            if (startCoolTime < 0)
            {
                this.startCoolTime = 0F;
            }
            else
            {
            
            }
        }
    }
}

