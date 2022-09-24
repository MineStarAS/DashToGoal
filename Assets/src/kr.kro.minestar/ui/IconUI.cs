using src.kr.kro.minestar.player;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using UnityEngine;
using UnityEngine.UI;

namespace src.kr.kro.minestar.ui
{
    public abstract class IconUI : MonoBehaviour
    { 
        public IconUIManager IconUIManager { get; set; }
        public Image IconImage { get; protected set; }
        public Image FillImage { get; private set; }
        public RectTransform IconTransform { get; private set; }
        public GameObject SkillUI;

        protected void Init()
        {
            SkillUI = GameObject.Find("ActiveSkill2UI");
            FillImage = new GameObject("FillImage").AddComponent<Image>();
            FillImage.color = Color.black;
            FillImage.type = Image.Type.Filled;
            FillImage.fillAmount = 1;
            FillImage.sprite = Resources.Load<Sprite>("Skill/FillImage");
        }

        private void FillPercent(float value) => FillImage.fillAmount = value;
        
        public void UpdateUI(int argNum = 0)
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
                IconImage.rectTransform.position = new Vector2(SkillUI.transform.position.x + 2f + 2f * argNum, SkillUI.transform.position.y);
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
            IconImage.rectTransform.position = new Vector2((Screen.width / 100)-30f + 2f * argNum, -5.73F); ;
            IconImage.rectTransform.localScale = new Vector2(1, 1);
            IconImage.rectTransform.sizeDelta = new Vector2(1.3F, 1.3F);
            IconImage.sprite = Resources.Load<Sprite>($"Skill/${skill.GetType().Name}");

            FillImage.transform.SetParent(IconImage.transform);
            FillImage.transform.position = IconImage.transform.position;
            FillImage.fillOrigin = (int)Image.Origin360.Top;
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
        
        public void Init(Effect effect, Player player, int argNum)
        {
            Init();
            Effect = effect;
            
            IconImage = gameObject.AddComponent<Image>();
            IconImage.rectTransform.position = new Vector2(SkillUI.transform.position.x + 2f + 2f * argNum, SkillUI.transform.position.y);
            IconImage.rectTransform.localScale = new Vector2(1, 1);
            IconImage.rectTransform.sizeDelta = new Vector2(1.3F, 1.3F);
            IconImage.sprite = Resources.Load<Sprite>("Effect/" + effect.GetType().Name);

            FillImage.transform.SetParent(IconImage.transform);
            FillImage.transform.position = IconImage.transform.position;
            FillImage.fillOrigin = (int)Image.Origin360.Top;
            FillImage.rectTransform.sizeDelta = new Vector2(1.3F, 1.3F);
            FillImage.rectTransform.localScale = new Vector2(1, 1);

            Color color = FillImage.color;
            color.a = 0.5f;
            FillImage.color = color;
        }
    }
}