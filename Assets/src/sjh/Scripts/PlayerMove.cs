using UnityEngine;

namespace src.sjh.Scripts
{
    public class PlayerMove : MonoBehaviour
    {
        /// ##### Default Field #####
        private const byte DefaultAirJumpAmount = 1;

        /// ##### Field #####
        [SerializeField] private float moveForce;

        [SerializeField] private float jumpForce;

        [SerializeField] private byte airJumpAmount; // 공중점프 가능 횟수

        private Rigidbody2D _body; // 플레이어 물리
        private SpriteRenderer _spriteRenderer;

        /// ##### Unity Functions #####
        private void Start() // 변수 초기화
        {
            moveForce = 6.0f;
            jumpForce = 13.0f;

            airJumpAmount = DefaultAirJumpAmount;

            _body = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!IsAir()) airJumpAmount = DefaultAirJumpAmount;
            
            if (Input.GetKeyDown(KeyCode.Space)) _DoJump(); // 점프

            if (Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.LeftArrow)) _DoMove(); // 좌우 이동
        }

        private void FixedUpdate()
        {
            // 플렛폼 타입 알아내기
            Debug.DrawRay(_body.position, Vector3.down, new Color(0, 1, 0));
            var rayHit = Physics2D.Raycast(_body.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            var rayHitCollider = rayHit.collider;
            if (rayHitCollider != null) Debug.Log(rayHitCollider.tag);
        }

        /// ##### Movement Functions #####
        private void _DoMove()
        {
            //Vector3 moveDirection = Vector3.zero; // 플레이어 이동 방향

            var fSpeed = 0.0f;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //moveDirection = Vector3.right;
                fSpeed = moveForce;
                _spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                //moveDirection = Vector3.left;
                fSpeed = -moveForce;
                _spriteRenderer.flipX = true;
            }

            _body.velocity = new Vector2(fSpeed, _body.velocity.y);

            // 새로운 위치 = 현재 위치 + 이동방향 * 속도
            //transform.position += moveDirection * m_fspeed * Time.deltaTime;
        }

        private void _DoJump()
        {
            if (!IsAir())
            {
                if (airJumpAmount <= 0) return;
                airJumpAmount--;
            }

            _body.velocity = Vector2.zero;
            var jumpVelocity = new Vector2(0, jumpForce); // 점프 속력
            _body.AddForce(jumpVelocity, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // 무한 점프 막기
            if (other.gameObject.layer == 6 && _body.velocity.y < 0)
            {
                // m_isLandign = true;
            }
        }

        /// ##### Check Functions #####
        bool IsAir()
        {
            var y = _body.velocity.y;
            return -0.1 <= y && y <= 0.1;
        }
    }
}