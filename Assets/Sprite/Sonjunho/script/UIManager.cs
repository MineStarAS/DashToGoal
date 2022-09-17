using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] m_gameUI;
    [SerializeField] Image m_imgActive1;
    [SerializeField] Image m_imgActive1_2;
    [SerializeField] Image m_imgActive2;
    [SerializeField] Image m_imgActive2_2;
    [SerializeField] Text m_tActive1;
    [SerializeField] Text m_tActive2;

    public Image imgActive1 { get => m_imgActive1; set => m_imgActive1 = value; }
    public Image imgActive1_2 { get => m_imgActive1_2; set => m_imgActive1_2 = value; }
    public Image imgActive2 { get => m_imgActive2; set => m_imgActive2 = value; }
    public Image imgActive2_2 { get => m_imgActive2_2; set => m_imgActive2_2 = value; }
    public Text tActive1 { get => m_tActive1; set => m_tActive1 = value; }
    public Text tActive2 { get => m_tActive2; set => m_tActive2 = value; }

    private void Start()
    {
        if (this.name != "GameManager") return;
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
}
