using System.Collections;
using System.Linq;
using src.kr.kro.minestar.player.character;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        public Player Player { get; protected set; }
        public PassiveSkill PassiveSkill { get; protected set; }

        public ActiveSkill ActiveSkill1 { get; protected set; }

        public ActiveSkill ActiveSkill2 { get; protected set; }


        /// ##### Constructor #####
        protected PlayerCharacter(Player player)
        {
            Player = player;
        }

        /// ##### Functions #####
        protected void StartTimer()
        {
            UIManager uiManager = Player.UIManager();
            
            ActiveSkill1.SetImageCoolTime(uiManager.imgActive1, uiManager.tActive1);
            ActiveSkill2.SetImageCoolTime(uiManager.imgActive2, uiManager.tActive2);

            Player.StartCoroutine(Timer());

            IEnumerator Timer()
            {
                while (true)
                {
                    foreach (Effect effect in Player.Effects.Values.Where(effect => effect is TimerEffect))
                    {
                        try
                        {
                            ((TimerEffect)effect).DoPassesTime();
                        }
                        catch (Exception)
                        {
                        }
                    }

                    ActiveSkill1.DoPassesTime();
                    ActiveSkill2.DoPassesTime();
                    
                    yield return new WaitForSeconds(0.01F);
                }
                // ReSharper disable once IteratorNeverReturns
            }
        }

        public void StopTimer()
        {
            Player.StopAllCoroutines();
        }
    }
}