using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class System_SceneManager : MonoBehaviour
{
    public void OnMouseDown()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
