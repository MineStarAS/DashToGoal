using System.Collections;
using System.Linq;
using src.kr.kro.minestar.player.character;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using Unity.VisualScripting;
using UnityEngine;

namespace src.kr.kro.minestar.player
{
    public enum PlayerCharacterEnum
    {
        MineStar,
        SonJunHo
    }


    public abstract class PlayerCharacter : MonoBehaviour
    {
        /// ##### Static Functions #####
        public static PlayerCharacter FromEnum(Player player, PlayerCharacterEnum playerCharacterEnum)
        {
            return playerCharacterEnum switch
            {
                PlayerCharacterEnum.MineStar => player.AddComponent<PcMineStar>(),
                PlayerCharacterEnum.SonJunHo => player.AddComponent<PcMineStar>(),
                _ => player.AddComponent<PcMineStar>(),
            };
        }

        /// ##### Field #####
        public PassiveSkill PassiveSkill { get; protected set; }

        public ActiveSkill ActiveSkill1 { get; protected set; }

        public ActiveSkill ActiveSkill2 { get; protected set; }

        /// ##### Functions #####
        protected void StartTimer()
        {
            var player = gameObject.GetComponent<Player>();
            
            StartCoroutine(Timer());

            IEnumerator Timer()
            {
                while (true)
                {
                    foreach (var effect in player.Effects.Where(effect => effect is TimerEffect))
                    {
                        ((TimerEffect)effect).DoPassesTime();
                    }

                    ActiveSkill1.DoPassesTime();
                    ActiveSkill2.DoPassesTime();
                    
                    yield return new WaitForSeconds(0.01F);
                }
            }
        }

        public void StopTimer() => StopAllCoroutines();
    }
}