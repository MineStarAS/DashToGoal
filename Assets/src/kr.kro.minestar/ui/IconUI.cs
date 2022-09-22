using src.kr.kro.minestar.ui;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace src.kr.kro.minestar.ui
{
    public abstract class IconUI
    { 
        public IconUIManager iconUIManager;
        public Image IconImage;
        Image FillImage { get; set; }
        RectTransform IconTransform;

        protected IconUI()
        {
            FillImage.color = Color.black;
            FillImage.type = Image.Type.Filled;
            FillImage.fillAmount = 0;
        }

        void FillPercent(float value) => FillImage.fillAmount = value;
        
        public void UpdateUI()
        {
            if (this is SkillIconUI skillIconUI)
            {
                if (skillIconUI.Skill is not ISkillCoolTime) return;
                FillPercent((skillIconUI as ISkillCoolTime).GetCoolTimePercent());
            }
            else if (this is EffectIconUI effectIconUI)
            {
                if (effectIconUI.Effect is not IEffectLimitTimer) return;
                FillPercent((effectIconUI as IEffectLimitTimer).GetTimePercent());
            }
        }
    }

    public class SkillIconUI : IconUI
    {
        public Skill Skill;

        public SkillIconUI(Skill skill)
        {
            Skill = skill;
            IconImage = iconUIManager.AddComponent<Image>();
            IconImage.sprite = Resources.Load<Sprite>($"Skill/${skill.GetType().Name}");
        }
        
        
    }

    public class EffectIconUI : IconUI
    {
        public Effect Effect;
        
        public EffectIconUI(Effect effect)
        {
            Effect = effect;
            IconImage = iconUIManager.AddComponent<Image>();
            IconImage.sprite = Resources.Load<Sprite>($"Effect/${effect.GetType().Name}");
        }

    }
}