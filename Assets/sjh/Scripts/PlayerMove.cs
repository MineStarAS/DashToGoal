using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float m_fspeed; // 이동 속도
    [SerializeField]
    private float m_fjumpforce; // 점프 파워

    private bool m_isjumping; // 점프 중인가
    private bool m_isLandign; // 착지 했는가


    Rigidbody2D m_rRigid; // 플레이어 물리
    SpriteRenderer m_Spr;
    
    private void Start() // 변수 초기화
    {
        m_fspeed = 6.0f; // 속도 
        m_fjumpforce = 13.0f; // 점프
        m_isjumping = false; // 점프 중인가
        m_isLandign = false; // 착지 중인가

        m_rRigid = GetComponent<Rigidbody2D>();
        m_Spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_isLandign)
        {
            m_isjumping = true;
            m_isLandign = false;
        }
    }

    private void FixedUpdate()
    {
        f_pMove(); // 이동 함수
        f_pJump(); // 점프 함수
    }
    
    private void f_pMove()
    {
        //Vector3 moveDirection = Vector3.zero; // 플레이어 이동 방향

        float fspeed = 0.0f;
        if (Input.GetKey(KeyCode.RightArrow)) 
        {
            //moveDirection = Vector3.right;
            fspeed = m_fspeed;
            m_Spr.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            //moveDirection = Vector3.left;
            fspeed = -m_fspeed;
            m_Spr.flipX = true;
        }

        m_rRigid.velocity = new Vector2(fspeed, m_rRigid.velocity.y);

        // 새로운 위치 = 현재 위치 + 이동방향 * 속도
        //transform.position += moveDirection * m_fspeed * Time.deltaTime;
    }

    private void f_pJump()
    {
        if (!m_isjumping) // 점프 중이면 점프를 취소
            return; 

        m_rRigid.velocity = Vector2.zero;
        Vector2 JumpVelocity = new Vector2(0, m_fjumpforce); // 점프 속력
        m_rRigid.AddForce(JumpVelocity, ForceMode2D.Impulse);

        m_isjumping = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 무한 점프 막기
        if (other.gameObject.layer == 6 && m_rRigid.velocity.y < 0)
        {
            m_isLandign = true;
        }
    }
}
