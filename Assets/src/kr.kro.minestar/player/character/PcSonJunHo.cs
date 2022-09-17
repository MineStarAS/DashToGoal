using src.kr.kro.minestar.device;
using src.kr.kro.minestar.player.skill;
using Unity.VisualScripting;
using UnityEngine;

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

    public class AsSummonDevice : ActiveSkill
    {
        private GameObject device;
        public AsSummonDevice(Player player) : base(player)
        {
            Player = player;
            Name = "Summon Device";
            Description = "Summon!!!\n" +
                          "De----vice---!!!!";

            device = Resources.Load<GameObject>("Device/TestDevice");

            Init(3F, 3F);
        }


        public override bool UseSkill()
        {
            if (!CanUseSkill()) return false;
            Device.SummonDevice<TestDevice>(Player.transform.position);
            UsedSkill();
            return true;
        }
    }
}