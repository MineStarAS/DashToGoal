using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using System;
using UnityEngine.UI;

// ReSharper disable All

namespace src.kr.kro.minestar.player.character
{
    public class PcMineStar : PlayerCharacter
    {
        public PcMineStar(Player player) : base(player)
        {
            PassiveSkill = new PsSpeedy(player);
            ActiveSkill1 = new AsDash(player);
            ActiveSkill2 = new AsSuperJump(player);
            StartTimer();
        }
    }

    public class PsSpeedy : Skill, ISkillDetectEvent
    {
        private Type DetectEvent { get; set; }
        Type ISkillDetectEvent.DetectEvent { get => DetectEvent; set => DetectEvent = value; }

        public PsSpeedy(Player player) : base(player)
        {
            Player = player;
            Name = "Speedy";
            Description = "I'm FAST!!!";
            DetectEvent = typeof(PlayerUseSkillEvent);
        }

        public void DetectedEvent()
        {
            
        }

        protected override void SkillFunction() => new Speed(Player).AddEffect();
    }

    public class AsDash : Skill, ISkillCoolTime
    {
        private double _defaultCoolTime;
        private int _currentCoolTime;

        public AsDash(Player player) : base(player)
        {
            Player = player;
            Name = "Dash";
            Description = "I will dash\n" +
                          "to the goal";
            
            _defaultCoolTime = 5;
        }

        protected override void SkillFunction()
        {
            Player.Movement.SetMovement(0F, 0F);
            Player.Movement.SetDrag(0);
            Player.Movement.AddMovementFlip(30F, 20F);
        }

        double ISkillCoolTime.DefaultCoolTime { get => _defaultCoolTime; set => _defaultCoolTime = value; }
        int ISkillCoolTime.CurrentCoolTime { get => _currentCoolTime; set => _currentCoolTime = value; }

        public Image SkillImage1 { get; set; }
        public Image SkillImage2 { get; set; }
        public Text CoolTimeText { get; set; }
    }

    public class AsSuperJump : Skill, ISkillCoolTime, ISkillCharge, ISkillDetectEvent
    {
        private double _defaultCoolTime;
        private int _currentCoolTime;

        private int _currentCharge;
        private int _chargeMax;
        private int _chargeUsage;
        private Type _detectEvent;

        public AsSuperJump(Player player) : base(player)
        {
            Player = player;
            Name = "SuperJump";
            Description = "I will jump\n" +
                          "to the goal";

            _defaultCoolTime = 5;
            _currentCoolTime = 5;

            _chargeMax = 3;
            _chargeUsage = 3;

            _detectEvent = typeof(PlayerJumpEvent);
        }

        protected override void SkillFunction() => Player.Movement.AddMovement(0F, 20F);
        
        double ISkillCoolTime.DefaultCoolTime { get => _defaultCoolTime; set => _defaultCoolTime = value; }
        int ISkillCoolTime.CurrentCoolTime { get => _currentCoolTime; set => _currentCoolTime = value; }
        public Image SkillImage1 { get; set; }
        public Image SkillImage2 { get; set; }
        public Text CoolTimeText { get; set; }
        int ISkillCharge.CurrentCharge { get => _currentCharge; set => _currentCharge = value; }
        int ISkillCharge.ChargeMax { get => _chargeMax; set => _chargeMax = value; }
        int ISkillCharge.ChargeUsage { get => _chargeUsage; set => _chargeUsage = value; }
        Type ISkillDetectEvent.DetectEvent { get => _detectEvent; set => _detectEvent = value; }

        public void DetectedEvent()
        {
            (this as ISkillCharge).DoCharge(1);   
            // DoCharge(1);
        }
    }
}