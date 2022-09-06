using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.src.sjh.Scripts
{
    public class SceneChanger : MonoBehaviour
    {
        void OnMouseDown()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}