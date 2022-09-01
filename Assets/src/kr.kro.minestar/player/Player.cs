using System;
using src.kr.kro.minestar.player.skill;

namespace src.kr.kro.minestar.player
{
    public class Player
    {
        public readonly PlayerCharacter playerCharacter;
        public readonly PassiveSkill passiveSkill;
        public readonly ActiveSkill activeskill1;
        public readonly ActiveSkill activeskill2;

        public Player(PlayerCharacter playerCharacter)
        {
            this.playerCharacter = playerCharacter;
            passiveSkill = PlayerCharacterFunction.PassiveSkill(playerCharacter);
            activeskill1 = PlayerCharacterFunction.ActiveSkill1(playerCharacter);
            activeskill2 = PlayerCharacterFunction.ActiveSkill2(playerCharacter);
        }
    }

    public enum PlayerCharacter
    {
        MineStar,
        SonJunHo
    }

    public static class PlayerCharacterFunction
    {
        public static string Name(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                PlayerCharacter.MineStar => "마인스타",
                PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }

        public static PassiveSkill PassiveSkill(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                PlayerCharacter.MineStar => "마인스타",
                PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }

        public static ActiveSkill ActiveSkill1(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                PlayerCharacter.MineStar => "마인스타",
                PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }

        public static ActiveSkill ActiveSkill2(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                PlayerCharacter.MineStar => "마인스타",
                PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }
    }
}