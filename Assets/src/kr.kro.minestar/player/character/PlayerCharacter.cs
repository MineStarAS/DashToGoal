using System.Collections;
using System.Linq;
using src.kr.kro.minestar.player.character;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using System;
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
                PlayerCharacterEnum.SonJunHo => new PcSonJunHo(player),
                _ => new PcMineStar(player),
            };
        }
        /// ##### Field #####
        protected Player Player { get; set; }
        public Skill PassiveSkill { get; protected set; }

        public Skill ActiveSkill1 { get; protected set; }

        public Skill ActiveSkill2 { get; protected set; }
        
        /// ##### Constructor #####
        protected PlayerCharacter(Player player)
        {
            Player = player;
        }
        
        /// ##### Functions #####
        protected void StartTimer()
        {
            UIManager uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
            
            (ActiveSkill1 as ISkillCoolTime)?.SetImageCoolTime(uiManager.imgActive1, uiManager.imgActive1_2, uiManager.tActive1);
            (ActiveSkill2 as ISkillCoolTime)?.SetImageCoolTime(uiManager.imgActive2, uiManager.imgActive2_2, uiManager.tActive2);
            
            Player.StartCoroutine(Timer());

            IEnumerator Timer()
            {
                while (true)
                {
                    (PassiveSkill as ISkillCoolTime)?.DoPassesTime();
                    (ActiveSkill1 as ISkillCoolTime)?.DoPassesTime();
                    (ActiveSkill2 as ISkillCoolTime)?.DoPassesTime();
                    
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