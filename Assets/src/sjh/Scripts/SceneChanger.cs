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
            if (CurrentScene.name == "Title")
                SceneManager.LoadScene("CharacterSelect");
            else if (CurrentScene.name == "CharacterSelect")
                if (name == "Btn_test")
                    SceneManager.LoadScene("test");
                else
                    SceneManager.LoadScene("map1");
            else if (CurrentScene.name == "test")
                if (name == "Btn_Menu")
                    SceneManager.LoadScene("Title");
                else
                    SceneManager.LoadScene("CharacterSelect");
            else if (CurrentScene.name == "Map1")
                SceneManager.LoadScene("Title");
        }
    }
}