using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float m_fspeed; // �̵� �ӵ�
    [SerializeField]
    private float m_fjumpforce; // ���� �Ŀ�

    private bool m_isjumping; // ���� ���ΰ�
    private bool m_isLandign; // ���� �ߴ°�


    Rigidbody2D m_rRigid; // �÷��̾� ����
    SpriteRenderer m_Spr;
    
    private void Start() // ���� �ʱ�ȭ
    {
        m_fspeed = 6.0f; // �ӵ� 
        m_fjumpforce = 13.0f; // ����
        m_isjumping = false; // ���� ���ΰ�
        m_isLandign = false; // ���� ���ΰ�

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
        f_pMove(); // �̵� �Լ�
        f_pJump(); // ���� �Լ�
    }
    
    private void f_pMove()
    {
        //Vector3 moveDirection = Vector3.zero; // �÷��̾� �̵� ����

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

        // ���ο� ��ġ = ���� ��ġ + �̵����� * �ӵ�
        //transform.position += moveDirection * m_fspeed * Time.deltaTime;
    }

    private void f_pJump()
    {
        if (!m_isjumping) // ���� ���̸� ������ ���
            return; 

        m_rRigid.velocity = Vector2.zero;
        Vector2 JumpVelocity = new Vector2(0, m_fjumpforce); // ���� �ӷ�
        m_rRigid.AddForce(JumpVelocity, ForceMode2D.Impulse);

        m_isjumping = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� ���� ����
        if (other.gameObject.layer == 6 && m_rRigid.velocity.y < 0)
        {
            m_isLandign = true;
        }
    }
}
