using src.kr.kro.minestar.ui;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

namespace src.kr.kro.minestar.ui
{
    public abstract class IconUI
    { 
        public IconUIManager iconUIManager;
        public Image IconImage;
        public Image FillImage { get; set; }
        RectTransform IconTransform;

        protected IconUI()
        {
            FillImage = new GameObject("FillImage").AddComponent<Image>();
            FillImage.color = Color.black;
            FillImage.type = Image.Type.Filled;
            FillImage.fillAmount = 1;
            FillImage.sprite = Resources.Load<Sprite>("Skill/FillImage");
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

        public SkillIconUI(Skill skill, IconUIManager iconUIManager, int argNum)
        {
            Skill = skill;
            IconImage = new GameObject("Icon").AddComponent<Image>();
            IconImage.rectTransform.position = new Vector2(-11f + 2f * argNum, -4.4f); ;
            IconImage.transform.SetParent(iconUIManager._canvas.transform);
            IconImage.rectTransform.localScale = new Vector2(1, 1);
            IconImage.sprite = Resources.Load<Sprite>($"Skill/${skill.GetType().Name}");

            FillImage.transform.SetParent(IconImage.transform);
            FillImage.transform.position = IconImage.transform.position;
            FillImage.rectTransform.localScale = new Vector2(1, 1);

            Color color = FillImage.color;
            color.a = 0.5f;
            FillImage.GetComponent<Image>().color = color;
            //IconImage = iconUIManager.AddComponent<Image>();
        }
    }

    public class EffectIconUI : IconUI
    {
        public Effect Effect;
        
        public EffectIconUI(Effect effect, IconUIManager iconUIManager)
        {
            Effect = effect;
            IconImage = new GameObject("Icon").AddComponent<Image>();

            //IconImage = iconUIManager.AddComponent<Image>();
            IconImage.sprite = Resources.Load<Sprite>($"Effect/${effect.GetType().Name}");
        }

    }
}