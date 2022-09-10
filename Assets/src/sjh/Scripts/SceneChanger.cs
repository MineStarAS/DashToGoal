using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace src.sjh.Scripts
{
    public class SceneChanger : MonoBehaviour
    {
        public void func_ClickBtn()
        {
            Scene CurrentScene = SceneManager.GetActiveScene();
            if(CurrentScene.name == "Title")
                SceneManager.LoadScene("CharacterSelect");
            else if (CurrentScene.name == "CharacterSelect")
                SceneManager.LoadScene("Map1");
        }
    }
}