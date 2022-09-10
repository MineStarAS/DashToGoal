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
            Debug.Log(gameEvent.GetType().Name);
            foreach (var player in _gameSystem.Players)
            {
                var passiveSkill = player.GetPlayerCharacter().GetPassiveSkill();
                var activeSkill1 = player.GetPlayerCharacter().GetActiveSkill1();
                var activeSkill2 = player.GetPlayerCharacter().GetActiveSkill2();


                if (passiveSkill.GetDetectEvent() == gameEvent.GetType()) passiveSkill.UseSkill(player);

                if (activeSkill1 is ChargeActiveSkill skill1)
                    if (skill1.GetDetectEvent() == gameEvent.GetType())
                        skill1.DoCharge(gameEvent);

                if (activeSkill2 is not ChargeActiveSkill skill2) continue;
                if (skill2.GetDetectEvent() == gameEvent.GetType())
                    skill2.DoCharge(gameEvent);
            }
        }
    }
}
