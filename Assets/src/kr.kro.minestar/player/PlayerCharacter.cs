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
        public PassiveSkill PassiveSkill { get; protected set; }

        public ActiveSkill ActiveSkill1 { get; protected set; }

        public ActiveSkill ActiveSkill2 { get; protected set; }
    }
}