using src.kr.kro.minestar.player.effect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_gameUI;
    [SerializeField] private Image[] m_imgEffects;
    [SerializeField] private Image[] m_imgFillEffects;
    [SerializeField] Image m_imgActive1;
    [SerializeField] Image m_imgActive1_2;
    [SerializeField] Image m_imgActive2;
    [SerializeField] Image m_imgActive2_2;
    [SerializeField] Text m_tActive1;
    [SerializeField] Text m_tActive2;
    List<string> m_EffectUIList;

    public Image imgActive1 { get => m_imgActive1; set => m_imgActive1 = value; }
    public Image imgActive1_2 { get => m_imgActive1_2; set => m_imgActive1_2 = value; }
    public Image imgActive2 { get => m_imgActive2; set => m_imgActive2 = value; }
    public Image imgActive2_2 { get => m_imgActive2_2; set => m_imgActive2_2 = value; }
    public Text tActive1 { get => m_tActive1; set => m_tActive1 = value; }
    public Text tActive2 { get => m_tActive2; set => m_tActive2 = value; }
      
    private void Start()
    {
        if (this.name != "GameManager") return;
        m_EffectUIList = new List<string>();
        m_EffectUIList.Add("Null");
    }
    public void func_ClickESC()
    {
        m_gameUI[2].gameObject.SetActive(true);
        m_gameUI[3].gameObject.SetActive(false);
    }

    public void func_ClickEXIT()
    {
        m_gameUI[2].gameObject.SetActive(false);
        m_gameUI[3].gameObject.SetActive(true);
    }

    public void func_ClickSkillUIOne(bool isOn)
    {
        Debug.Log(isOn);
        if (isOn) m_gameUI[0].gameObject.SetActive(true);
        else m_gameUI[0].gameObject.SetActive(false);
    }

    public void func_ClickSkillUITwo(bool isOn)
    {
        if (isOn) m_gameUI[1].gameObject.SetActive(true);
        else m_gameUI[1].gameObject.SetActive(false);
    }

    public void func_DoEffect(Effect effect)
    {
        if (m_EffectUIList.Contains(effect.Name)) return;

        // 첫 Effect 셋팅
        if (m_EffectUIList[0] == "Null")
        {
            m_EffectUIList.Remove("Null");
            m_EffectUIList.Add(effect.Name);
            m_imgEffects[0].gameObject.SetActive(true);
            return;
        }

        // 첫 번쨰 이후 Effect 셋팅 
        for (int i = 0; i < m_EffectUIList.Count; i++)
        {
            if(i == m_EffectUIList.Count -1)
            {
                // 스프라이트를 argName에 맞게 수정.
                m_EffectUIList.Add(effect.Name);
                m_imgEffects[i + 1].gameObject.SetActive(true);
                break;
            }
        }
    }

    public Image func_GetImage(Effect effect)
    {
        int inum = 0;
        // 첫 Effect 셋팅
        if (m_EffectUIList[0] != "Null")
        {
            for (; inum < m_EffectUIList.Count; inum++)
            {
                if (m_EffectUIList[inum] == effect.Name) break;
                if (inum == m_EffectUIList.Count - 1) inum++;
            }
        }
        return m_imgFillEffects[inum];
    }

    public void func_EffectEnd(Effect effect)
    {
        Debug.Log("끝 -- " + effect.Name);
        int ListIndex = m_EffectUIList.IndexOf(effect.Name);
        m_imgEffects[m_EffectUIList.Count - 1].gameObject.SetActive(false);
        if (m_EffectUIList.Count != 1) m_EffectUIList.RemoveAt(ListIndex);
        else
        {
            m_EffectUIList.RemoveAt(ListIndex);
            m_EffectUIList.Add("Null");
        }
    }
}
