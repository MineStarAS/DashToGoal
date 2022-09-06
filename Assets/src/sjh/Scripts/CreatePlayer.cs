using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    [SerializeField] GameObject m_gPlayer;
    [SerializeField] GameObject m_gCamera;

    private void Start()
    {
        GameObject gObj;
        gObj = Instantiate(m_gPlayer);
        gObj.transform.position = new Vector3(0, 0, 0);
        m_gCamera.GetComponent<CameraController>().func_ChasePlayer();
    }
}
