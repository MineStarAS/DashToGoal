using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    PlatformEffector2D m_effector2D;
    public PlatformEffector2D effector2D { get => m_effector2D; set => m_effector2D = value; }
    private void Start()
    {
        effector2D = GetComponent<PlatformEffector2D>();
    }
}