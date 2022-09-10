using UnityEngine;
using src.kr.kro.minestar.player;
using Unity.VisualScripting;
using System.Linq.Expressions;

namespace src.sjh.Scripts
{
    public class PlayerMove : MonoBehaviour
    {
        /// ##### Default Field #####
        [SerializeField] private const byte DefaultAirJumpAmount = 1; // 공중 점프 가능한 횟수.
        Player m_Player;
        /// ##### Field #####
        [SerializeField] private float moveForce;    // 플레이어 이동에 가해지는 힘
        [SerializeField] private float jumpForce;    // 플레이어 점프 힘
        [SerializeField] private float gizmoSize;

        private bool m_isJump; // 점프키를 눌렀는가
        private float m_fMaxSpeed; // 플레이어 이동속도
        private byte _airJumpAmount; // 공중 점프 가능 횟수
        [SerializeField] private int m_iGroundjump; // 땅에 있을때 점프할 수 있는 
        private Rigidbody2D _body; // 플레이어 물리
        private SpriteRenderer _spriteRenderer; // 스프라이트 정보

        /// ##### Unity Functions #####
        private void Start() // 변수 초기화
        {
            m_Player = GetComponent<Player>();
            m_fMaxSpeed = 5.0f;
            moveForce = 0.05f;
            jumpForce = 13.0f;

            _airJumpAmount = DefaultAirJumpAmount;

            _body = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {  
            _DoJump(); // 점프
            _DoMove(); // 좌우 이동
            _DoUseSkill(); // 스킬 사용
        }

        private void FixedUpdate()
        {
            if(_body.velocity.y < -10.0f)
            {
                _body.drag = 2.0f; // 플레이어 낙하 속도
            }

            if (this.transform.position.y < -15) this.transform.position = new Vector3(0, 0, 0);
        }

        private void _DoUseSkill()
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                m_Player.DoUseActiveSkill1();
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                m_Player.DoUseActiveSkill2();
            }
        }

        /// ##### Movement Functions #####
        private void _DoMove()
        {
            // 움직임
            float h = 0; // 좌우 방향
            if (Input.GetKey(KeyCode.RightArrow))
            {
                h = 1;
                _body.drag = 0.0f;
                _spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                h = -1;
                _body.drag = 0.0f; // 저항값
                _spriteRenderer.flipX = true;
            }
            _body.AddForce(Vector2.right * h * moveForce, ForceMode2D.Impulse);
            

            if (_body.velocity.x > m_fMaxSpeed)
                _body.velocity = new Vector2(m_fMaxSpeed, _body.velocity.y);
            else if (_body.velocity.x < -m_fMaxSpeed)
                _body.velocity = new Vector2(-m_fMaxSpeed, _body.velocity.y);


            // 멈추기
            if (m_iGroundjump == 0 || m_isJump == true) return; // 플레이어가 공중에 있으면 실행 못하게
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                _body.drag = 30.0f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                _body.drag = 30.0f;
            }
        }

        private void _DoJump()
        {
            if (_airJumpAmount <= 0) return;
            // 점프 횟수 추가
            if (Input.GetKeyDown(KeyCode.C) & _airJumpAmount != 0)
            {
                m_isJump = true;
                if (m_iGroundjump == 0) _airJumpAmount--;
                _body.drag = 0.0f;
                _body.velocity = new Vector2(_body.velocity.x,0);
                _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // 무한 점프 막기
            if (other.gameObject.layer == 6 && _body.velocity.y <= 0) // 점프 후 착지했다면
            {
                m_isJump = false;
                _airJumpAmount = DefaultAirJumpAmount;
                _body.drag = 0;
                _body.velocity = new Vector2(_body.velocity.x, -1);
                m_iGroundjump = 1;
                if (!Input.GetKey(KeyCode.RightArrow))
                {
                    _body.drag = 30.0f;
                }
                else if (!Input.GetKey(KeyCode.LeftArrow))
                {
                    _body.drag = 30.0f;
                }
            }

            if (!m_isJump && other.gameObject.layer == 6 && _body.velocity.y >= 0) // 땅에 있다면
            {
                m_iGroundjump = 1;
                _airJumpAmount = DefaultAirJumpAmount;
                _body.velocity = new Vector2(_body.velocity.x, -1);
                if (!Input.GetKey(KeyCode.RightArrow))
                {
                    _body.drag = 30.0f;
                }
                else if (!Input.GetKey(KeyCode.LeftArrow))
                {
                    _body.drag = 30.0f;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) // 타일의 경계선을 나가도 실행이 됨.
        {
            if (other.gameObject.layer == 6)
            {
                m_iGroundjump = 0;
                _body.drag = 0.0f;
                this.GetComponent<BoxCollider2D>().enabled = false;
                Invoke("_DoCheckCollider", 0.05f); // 다시 체크.
            }
        }

        void _DoCheckCollider() // 다시 충돌 감지
        {
            if (!this.GetComponent<BoxCollider2D>().enabled)
                this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}