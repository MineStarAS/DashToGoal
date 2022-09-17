using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [SerializeField] CreatePlayer scrCreatePlayer;
    [SerializeField] Text m_textTimer;
    [SerializeField] Button m_Restart;
    [SerializeField] Button m_Esc;

    float m_fSec = 0;
    int m_iMin = 0;
    int m_ihour = 0;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            scrCreatePlayer.IsPlaying = false;
            m_Restart.gameObject.SetActive(true);
            m_Esc.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (scrCreatePlayer.IsPlaying) func_Time();
        if (Input.GetKeyDown(KeyCode.K)) m_iMin += 12;
    }

    private void func_Time()
    {
        m_fSec += Time.deltaTime;
        if (m_fSec >= 60)
        {
            m_fSec -= 60;
            m_iMin++;
        }
        else if(m_iMin >= 60)
        {
            m_iMin -= 60;
            m_ihour++;
        }

        if (m_iMin == 0 && m_ihour == 0)
            m_textTimer.text = string.Format("{0:N3}", m_fSec);
        else if (m_iMin >= 1 && m_ihour == 0)
            m_textTimer.text = string.Format("{0:} : {1:N3}", m_iMin, m_fSec);
        else if (m_iMin >= 0 && m_ihour >= 1)
            m_textTimer.text = string.Format("{0:} : {1:} : {2:N3}", m_ihour, m_iMin, m_fSec);
    }
}
