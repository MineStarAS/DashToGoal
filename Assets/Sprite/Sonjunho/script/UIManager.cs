using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_gameUI;

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
