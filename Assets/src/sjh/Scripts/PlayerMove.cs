using UnityEngine;

namespace src.sjh.Scripts
{
    public class PlayerMove : MonoBehaviour
    {
        /// ##### Default Field #####
        [SerializeField] private const byte DefaultAirJumpAmount = 1; // 공중 점프 가능한 횟수.

        /// ##### Field #####
        [SerializeField] private float moveForce;    // 플레이어 이동에 가해지는 힘
        [SerializeField] private float jumpForce;    // 플레이어 점프 힘
        [SerializeField] private float gizmoSize;

        private float m_fMoveSpeed; // 플레이어 이동속도
        private byte _airJumpAmount; // 공중 점프 가능 횟수
        private int m_iGroundjump; // 땅에 있을때 점프할 수 있는 
        private Rigidbody2D _body; // 플레이어 물리
        private SpriteRenderer _spriteRenderer; // 스프라이트 정보

        /// ##### Unity Functions #####
        private void Start() // 변수 초기화
        {
            m_fMoveSpeed = 0.0f;
            moveForce = 6.0f;
            jumpForce = 13.0f;

            _airJumpAmount = DefaultAirJumpAmount;

            _body = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {  
            _DoJump(); // 점프
            _DoMove(); // 좌우 이동
        }

        private void FixedUpdate()
        {
            if(_body.velocity.y < -10.0f)
            {
                _body.drag = 2.0f; // 플레이어 낙하 속도
            }
        }

        /// ##### Movement Functions #####
        private void _DoMove()
        {
            m_fMoveSpeed = 0.0f;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                m_fMoveSpeed = moveForce;
                _spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                m_fMoveSpeed = -moveForce;
                _spriteRenderer.flipX = true;
            }

            _body.velocity = new Vector2(m_fMoveSpeed, _body.velocity.y);
        }

        private void _DoJump()
        {
            if (_airJumpAmount <= 0) return;
            // 점프 횟수 추가
            if (Input.GetKeyDown(KeyCode.Space) & _airJumpAmount != 0)
            {
                if (m_iGroundjump == 0) _airJumpAmount--;
                _body.drag = 0.0f;
                _body.velocity = Vector2.zero;
                var jumpVelocity = new Vector2(0, jumpForce); // 점프 속력
                _body.AddForce(jumpVelocity, ForceMode2D.Impulse);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // 무한 점프 막기
            if (other.gameObject.layer == 6 && _body.velocity.y <= 0)
            {
                _airJumpAmount = DefaultAirJumpAmount;
                _body.drag = 0;
                _body.velocity = new Vector2(m_fMoveSpeed, 0);
                m_iGroundjump = 1;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == 6)
            {
                m_iGroundjump = 0;
            }
        }
    }
}