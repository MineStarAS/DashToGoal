using src.kr.kro.minestar.player;
using src.kr.kro.minestar.player.skill;
using UnityEngine;

namespace src.kr.kro.minestar.gameEvent
{
    public abstract class GameEvent
    {
    }

    public class GameEventOperator
    {
        /// ##### Field #####
        private readonly GameSystem _gameSystem;

        /// ##### Constructor #####
        public GameEventOperator(GameSystem gameSystem)
        {
            _gameSystem = gameSystem;
        }


        public void DoEvent(GameEvent gameEvent)
        {
            //Debug.Log(gameEvent.GetType().Name);
            foreach (Player player in _gameSystem.Players)
            {
                PassiveSkill passiveSkill = player.PlayerCharacter.PassiveSkill;
                ActiveSkill activeSkill1 = player.PlayerCharacter.ActiveSkill1;
                ActiveSkill activeSkill2 = player.PlayerCharacter.ActiveSkill2;

                if (passiveSkill is DetectPassiveSkill skill0)
                    if (skill0.DetectEvent == gameEvent.GetType()) passiveSkill.UseSkill();

                if (activeSkill1 is ChargeActiveSkill skill1)
                    if (skill1.DetectEvent == gameEvent.GetType())
                        skill1.DoCharge(gameEvent);

                if (activeSkill2 is not ChargeActiveSkill skill2) continue;
                if (skill2.DetectEvent == gameEvent.GetType())
                    skill2.DoCharge(gameEvent);
            }
        }
    }
}