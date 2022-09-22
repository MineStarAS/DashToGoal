using src.kr.kro.minestar.ui;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

namespace src.kr.kro.minestar.ui
{
    public abstract class IconUI : MonoBehaviour
    { 
        public IconUIManager IconUIManager { get; set; }
        public Image IconImage { get; protected set; }
        public Image FillImage { get; private set; }
        public RectTransform IconTransform { get; private set; }

        protected void Init()
        {
            FillImage = new GameObject("FillImage").AddComponent<Image>();
            FillImage.color = Color.black;
            FillImage.type = Image.Type.Filled;
            FillImage.fillAmount = 0;
            FillImage.sprite = Resources.Load<Sprite>("Skill/FillImage");
        }

        private void FillPercent(float value) => FillImage.fillAmount = value;
        
        public void UpdateUI()
        {
            if (this is SkillIconUI skillIconUI)
            {
                if (skillIconUI.Skill is not ISkillCoolTime) return;
                FillPercent(((ISkillCoolTime)skillIconUI.Skill).GetCoolTimePercent());
            }
            else if (this is EffectIconUI effectIconUI)
            {
                if (effectIconUI.Effect is not IEffectLimitTimer) return;
                FillPercent(((IEffectLimitTimer)effectIconUI.Effect).GetTimePercent());
            }
        }
    }

    public class SkillIconUI : IconUI
    {
        public Skill Skill { get; set; }

        public void Init(Skill skill, int argNum)
        {
            Init();
            
            Skill = skill;
            
            IconImage = gameObject.AddComponent<Image>();
            IconImage.rectTransform.position = new Vector2(-11f + 2f * argNum, -5.73F); ;
            IconImage.rectTransform.localScale = new Vector2(1, 1);
            IconImage.rectTransform.sizeDelta = new Vector2(1.3F, 1.3F);
            IconImage.sprite = Resources.Load<Sprite>($"Skill/${skill.GetType().Name}");

            FillImage.transform.SetParent(IconImage.transform);
            FillImage.transform.position = IconImage.transform.position;
            FillImage.rectTransform.sizeDelta = new Vector2(1.3F, 1.3F);
            FillImage.rectTransform.localScale = new Vector2(1, 1);

            Color color = FillImage.color;
            color.a = 0.5f;
            FillImage.color = color;
        }
    }

    public class EffectIconUI : IconUI
    {
        public Effect Effect { get; set; }
        
        public EffectIconUI Init(Effect effect)
        {
            Init();
            
            Effect = effect;
            
            IconImage = gameObject.AddComponent<Image>();
            IconImage.sprite = Resources.Load<Sprite>($"Effect/${effect.GetType().Name}");
            
            return this;
        }

    }
}