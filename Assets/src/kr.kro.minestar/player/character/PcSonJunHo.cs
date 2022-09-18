using src.kr.kro.minestar.device;
using src.kr.kro.minestar.player.skill;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable All

namespace src.kr.kro.minestar.player.character
{
    public class PcSonJunHo : PlayerCharacter
    {
        public PcSonJunHo(Player player) : base(player)
        {
            PassiveSkill = new PsSpeedy(player);
            ActiveSkill1 = new AsDash(player);
            ActiveSkill2 = new AsSummonDevice(player);
            StartTimer();
        }
    }

    public class AsSummonDevice : Skill, ISkillCoolTime
    {
        private double _defaultCoolTime;
        private int _currentCoolTime;

        public AsSummonDevice(Player player) : base(player)
        {
            Player = player;
            Name = "Summon Device";
            Description = "Summon!!!\n" +
                          "De----vice---!!!!";
            _defaultCoolTime = 3;
            _currentCoolTime = 3;
        }

        protected override void SkillFunction() => DeviceObject.SummonDevice<TestDevice>(Player.transform.position);
        

        double ISkillCoolTime.DefaultCoolTime
        {
            get => _defaultCoolTime;
            set => _defaultCoolTime = value;
        }

        int ISkillCoolTime.CurrentCoolTime
        {
            get => _currentCoolTime;
            set => _currentCoolTime = value;
        }

        public Image SkillImage1 { get; set; }
        public Image SkillImage2 { get; set; }
        public Text CoolTimeText { get; set; }
    }
}