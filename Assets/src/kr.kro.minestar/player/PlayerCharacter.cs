using src.kr.kro.minestar.player.character;
using src.kr.kro.minestar.player.skill;

namespace src.kr.kro.minestar.player
{
    public enum PlayerCharacterEnum
    {
        MineStar,
        SonJunHo
    }


    public abstract class PlayerCharacter
    {
        /// ##### Static Functions #####
        public static PlayerCharacter FromEnum(Player player, PlayerCharacterEnum playerCharacterEnum)
        {
            return playerCharacterEnum switch
            {
                PlayerCharacterEnum.MineStar => new PcMineStar(player),
                PlayerCharacterEnum.SonJunHo => new PcMineStar(player),
                _ => new PcMineStar(player),
            };
        }

        /// ##### Field #####
        private PassiveSkill _passiveSkill;

        private ActiveSkill _activeSkill1;
        private ActiveSkill _activeSkill2;

        /// ##### Getter #####
        public PassiveSkill GetPassiveSkill() => _passiveSkill;

        public ActiveSkill GetActiveSkill1() => _activeSkill1;

        public ActiveSkill GetActiveSkill2() => _activeSkill2;

        /// ##### Setter #####
        protected void SetPassiveSkill(PassiveSkill passiveSkill) => _passiveSkill = passiveSkill;

        protected void SetActiveSkill1(ActiveSkill activeSkill) => _activeSkill1 = activeSkill;

        protected void SetActiveSkill2(ActiveSkill activeSkill) => _activeSkill2 = activeSkill;
    }
}