using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otherPlayer : MonoBehaviour
{
    Rigidbody2D rigid;
    bool isStop = false;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("func_Stop", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStop) return;
            rigid.velocity = new Vector2(1 * 20f, 0);
    }

    void func_Stop()
    {
        isStop = true;
        rigid.velocity = new Vector2(0, 0);
    }
}
