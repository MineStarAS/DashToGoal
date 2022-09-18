using src.kr.kro.minestar.player;
using src.kr.kro.minestar.player.skill;

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
                Skill passiveSkill = player.PlayerCharacter.PassiveSkill;
                Skill activeSkill1 = player.PlayerCharacter.ActiveSkill1;
                Skill activeSkill2 = player.PlayerCharacter.ActiveSkill2;

                if (passiveSkill is ISkillDetectEvent skill0)
                    if (skill0.DetectEvent == gameEvent.GetType())
                        skill0.DetectedEvent(gameEvent);

                if (activeSkill1 is ISkillDetectEvent skill1)
                    if (skill1.DetectEvent == gameEvent.GetType())
                        skill1.DetectedEvent(gameEvent);

                if (activeSkill2 is not ISkillDetectEvent skill2) continue;
                if (skill2.DetectEvent == gameEvent.GetType())
                    skill2.DetectedEvent(gameEvent);
            }
        }
    }
}