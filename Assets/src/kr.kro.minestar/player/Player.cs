using System;
using src.kr.kro.minestar.player.skill;

namespace src.kr.kro.minestar.player
{
    public class Player
    {
        private readonly PlayerCharacter _playerCharacter;
        private readonly PassiveSkill _passiveSkill;
        private readonly ActiveSkill _activeskill1;
        private readonly ActiveSkill _activeskill2;
        
        private readonly 

        public Player(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
            _passiveSkill = PlayerCharacterFunction.PassiveSkill(playerCharacter);
            _activeskill1 = PlayerCharacterFunction.ActiveSkill1(playerCharacter);
            _activeskill2 = PlayerCharacterFunction.ActiveSkill2(playerCharacter);
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
                // PlayerCharacter.MineStar => "마인스타",
                // PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }

        public static ActiveSkill ActiveSkill1(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                // PlayerCharacter.MineStar => "마인스타",
                // PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }

        public static ActiveSkill ActiveSkill2(PlayerCharacter playerCharacter)
        {
            return playerCharacter switch
            {
                // PlayerCharacter.MineStar => "마인스타",
                // PlayerCharacter.SonJunHo => "손준호",
                _ => throw new ArgumentOutOfRangeException(nameof(playerCharacter), playerCharacter, null)
            };
        }
    }
}