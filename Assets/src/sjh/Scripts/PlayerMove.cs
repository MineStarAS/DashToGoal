using UnityEngine;

namespace src.sjh.Scripts
{
    public class PlayerMove : MonoBehaviour
    {
        /// ##### Default Field #####
        [SerializeField] private const byte DefaultAirJumpAmount = 2; // 기본 점프 가능 횟수 1 = 1단, 2 = 2단 점프 가능
        private bool m_isLanding; // 땅에 닿았는가

        /// ##### Field #####
        [SerializeField] private float moveForce;    // 플레이어 이동 힘
        [SerializeField] private float jumpForce;    // 플레이어 점프 힘
        [SerializeField] private float gizmoSize;

        private byte _airJumpAmount; // 공중 점프 가능 횟수
        private Rigidbody2D _body; // 플레이어 물리
        private SpriteRenderer _spriteRenderer; // 스프라이트 정보
        private Transform _target; // 감지된 물체


        /// ##### Unity Functions #####
        private void Start() // 변수 초기화
        {
            m_isLanding = false;
            moveForce = 6.0f;
            jumpForce = 13.0f;
            gizmoSize = 5.0f;

            _airJumpAmount = DefaultAirJumpAmount;

            _body = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {  
            _DoJump(); // 점프
            _DoMove(); // 좌우 이동
            _DoDetect(); // 범위내 오브젝트 감지
        }

        private void FixedUpdate()
        {
            // 플렛폼 타입 알아내기
            Debug.DrawRay(_body.position, Vector3.down, new Color(0, 1, 0));
            var rayHit = Physics2D.Raycast(_body.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            var rayHitCollider = rayHit.collider;
            //if (rayHitCollider != null) Debug.Log(rayHitCollider.tag);
            if(_body.velocity.y < -10.0f && !m_isLanding)
            {
                _body.drag = 2.0f;
            }
        }

        /// ##### Movement Functions #####
        private void _DoMove()
        {
            var fSpeed = 0.0f;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                fSpeed = moveForce;
                _spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                fSpeed = -moveForce;
                _spriteRenderer.flipX = true;
            }

            _body.velocity = new Vector2(fSpeed, _body.velocity.y);
        }

        private void _DoJump()
        {
            if (_airJumpAmount <= 0) return;
            // 점프 횟수 추가
            if (!(Input.GetKeyDown(KeyCode.Space) & _airJumpAmount != 0)) return;
            _airJumpAmount--;

            _body.velocity = Vector2.zero;
            var jumpVelocity = new Vector2(0, jumpForce); // 점프 속력
            _body.AddForce(jumpVelocity, ForceMode2D.Impulse);
            m_isLanding = false;

        }

        private void _DoDetect() // 플레이어 혹은 물체 감지 - 나중에 스킬로 옮기면됨
        {
            if (!Input.GetKeyDown(KeyCode.V)) return;
            var cols = Physics2D.OverlapCircleAll(transform.position, gizmoSize);
            if (cols.Length > 0)
            {
                foreach (var t in cols)
                {
                    if (!t.CompareTag("Enemy")) continue;
                    Debug.Log("Physics Enemy : Target found");
                    _target = t.gameObject.transform;
                }
            }
            else
            {
                Debug.Log("Physics Enemy : Target lost");
                _target = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // 무한 점프 막기
            if (other.gameObject.layer != 6 || !(_body.velocity.y <= 0)) return;
            m_isLanding = true;
            _airJumpAmount = DefaultAirJumpAmount;
            _body.drag = 0;
        }

        private void OnDrawGizmos() // 감지 거리 그리기
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, gizmoSize);
        }
    }
}