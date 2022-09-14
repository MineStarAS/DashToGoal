using System;
using System.Linq;
using UnityEngine;
using src.kr.kro.minestar.player;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;

namespace src.sjh.Scripts
{
    public class PlayerMove : MonoBehaviour
    {
        /// ##### Field #####
        private Player m_Player;

        float m_dir = 0;
        private float m_fMaxSpeed = 8.0f; // 플레이어 이동속도
        private float moveForce = 0.05f; // 플레이어 이동에 가해지는 힘
        private float jumpForce = 13.0f; // 플레이어 점프 힘
        private bool m_isJump; // 점프키를 눌렀는가
        private bool m_isSkill = false; // 스킬 사용
        public bool isSkill { get => m_isSkill; set => m_isSkill = value; }

        private const int DefaultAirJumpAmount = 1; // 공중 점프 가능한 횟수.
        private int _airJumpAmount = DefaultAirJumpAmount; // 공중 점프 가능 횟수
        private int m_iGroundjump; // 땅에 있을때 점프할 수 있는 
        public Rigidbody2D _body; // 플레이어 물리
        private SpriteRenderer _spriteRenderer; // 스프라이트 정보

        /// ##### Unity Functions #####
        private void Start() // 변수 초기화
        {
            m_Player = GetComponent<Player>();
            _body = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void FixedCheck()
        {
            if (_body != null)
            {
                if (_body.velocity.y < -10.0f)
                {
                    SetDrag(2F);
                }
            }

            if (transform.position.y < -15) transform.position = new Vector3(0, 0, 0);
        }

        /// ##### Calculate Functions #####
        private float GetMoveForce()
        {
            var value = m_fMaxSpeed;
            var effects = m_Player.Effects;

            if (effects == null || effects.Count == 0) return value;

            // Add Calculate
            foreach (var effect in effects.Where(effect => effect.Calculator == Calculator.Add))
            {
                switch (effect.EffectType)
                {
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.Bondage:
                        return 0F;
                    case EffectType.BonusJump:
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            // Multi Calculate
            foreach (var effect in effects.Where(effect => effect.Calculator == Calculator.Multi))
            {
                switch (effect.EffectType)
                {
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.Bondage:
                        return 0F;
                    case EffectType.BonusJump:
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            return value;
        }

        private float GetJumpForce()
        {
            var value = jumpForce;
            var effects = m_Player.Effects;

            if (effects == null || effects.Count == 0) return value;

            foreach (var effect in effects.Where(effect => effect.Calculator == Calculator.Add))
            {
                switch (effect.EffectType)
                {
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.Bondage:
                        return 0F;
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                    case EffectType.BonusJump:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            foreach (var effect in effects.Where(effect => effect.Calculator == Calculator.Multi))
            {
                switch (effect.EffectType)
                {
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.Bondage:
                        return 0F;
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                    case EffectType.BonusJump:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            return value;
        }

        public int LandingAirJumpAmountCharge()
        {
            var value = DefaultAirJumpAmount;
            var effects = m_Player.Effects;

            // Add Calculate
            foreach (var effect in effects.Where(effect => effect.Calculator == Calculator.Add))
            {
                switch (effect.EffectType)
                {
                    case EffectType.BonusJump:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                    case EffectType.Bondage:
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            // Multi Calculate
            foreach (var effect in effects.Where(effect => effect.Calculator == Calculator.Multi))
            {
                switch (effect.EffectType)
                {
                    case EffectType.BonusJump:
                        value = Calculate(value, effect);
                        continue;
                    case EffectType.FastMovement:
                    case EffectType.SlowMovement:
                    case EffectType.Bondage:
                    case EffectType.SuperJump:
                    case EffectType.JumpFatigue:
                    case EffectType.Disorder:
                    default:
                        continue;
                }
            }

            return value;
        }

        private static float Calculate(float value, Effect effect)
        {
            return effect.Calculator switch
            {
                Calculator.Add => value + effect.CalculatorValue,
                Calculator.Multi => value * effect.CalculatorValue,
                _ => value
            };
        }

        private static int Calculate(int value, Effect effect)
        {
            return effect.Calculator switch
            {
                Calculator.Add => value + Convert.ToByte(effect.CalculatorValue),
                Calculator.Multi => value * Convert.ToByte(effect.CalculatorValue),
                _ => value
            };
        }

        /// ##### Movement Functions #####
        public void DoMove()
        {
            var maxMoveForce = GetMoveForce();

            // 움직임
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                m_dir = 1;
                _body.drag = 0.0f;
                _spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                m_dir = -1;
                _body.drag = 0.0f; // 저항값
                _spriteRenderer.flipX = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                m_dir = -2;
            }

            if (m_dir == -2) // 좌우키 동시 입력
            {
                if(_body.velocity.x > 0.1f) _body.AddForce(Vector2.right * -1 * moveForce, ForceMode2D.Impulse);
                else if (_body.velocity.x < -0.1f) _body.AddForce(Vector2.right * 1 * moveForce, ForceMode2D.Impulse);
                else _body.velocity = new Vector2(0, _body.velocity.y);

                return;
            }


            if (_body.velocity.x > maxMoveForce) // 최고 속도보다 클 때
            {
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    if (m_iGroundjump == 0 || m_isJump == true) return; // 플레이어가 공중에 있으면 실행 못하게
                    SetDrag(30F);
                }
                if (!Input.GetKey(KeyCode.LeftArrow)) return;
                _body.AddForce(Vector2.right * m_dir * moveForce, ForceMode2D.Impulse);
            }
            if (_body.velocity.x < -maxMoveForce) // 최고 속도보다 작을 때
            {
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    if (m_iGroundjump == 0 || m_isJump == true) return; // 플레이어가 공중에 있으면 실행 못하게
                    SetDrag(30F);
                }
                if (!Input.GetKey(KeyCode.RightArrow)) return;
                _body.AddForce(Vector2.right * m_dir * moveForce, ForceMode2D.Impulse);
            }
            else                                    // 현재 속도가 최고 속도보다 작지도 크지도 않을 때
            {
                if (m_dir == -1 && Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    if (m_iGroundjump == 0 || m_isJump == true) return; // 플레이어가 공중에 있으면 실행 못하게
                    SetDrag(30F);
                }
                else if (m_dir == 1 && Input.GetKeyUp(KeyCode.RightArrow))
                {
                    if (m_iGroundjump == 0 || m_isJump == true) return; // 플레이어가 공중에 있으면 실행 못하게
                    SetDrag(30F);
                }

                if (_body.drag != 30 && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
                {
                    _body.AddForce(Vector2.right * m_dir * moveForce, ForceMode2D.Impulse);
                }
            }

            new PlayerMoveEvent(m_Player);
        }

        public void DoJump()
        {
            if (!Input.GetKeyDown(KeyCode.C) || _airJumpAmount <= 0) return;
            var jf = GetJumpForce();

            // 점프 횟수 추가
            m_dir = 0;
            m_isJump = true;
            if (m_iGroundjump == 0) _airJumpAmount--;
            _body.drag = 0.0f;
            _body.velocity = new Vector2(_body.velocity.x, 0);
            _body.AddForce(Vector2.up * jf, ForceMode2D.Impulse);
            new PlayerJumpEvent(m_Player);
        }
        public void AddMovement(float x, float y) => _body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);

        public void AddMovementFlip(float x, float y) => _body.AddForce(!_spriteRenderer.flipX ? new Vector2(x, y) : new Vector2(-x, y), ForceMode2D.Impulse);
        public void SetMovementFlip(float x, float y) => _body.velocity = new Vector2(x,y);
        public void SetDrag(float value) => _body.drag = value;


        private void OnTriggerEnter2D(Collider2D other)
        {
            try
            {
                //m_isJump = false;
                //_airJumpAmount = DefaultAirJumpAmount;
                //_body.drag = 0;
                //_body.velocity = new Vector2(_body.velocity.x, -1);
                //m_iGroundjump = 1;

                //if (!Input.GetKey(KeyCode.RightArrow))
                // 무한 점프 막기
                if (other.gameObject.layer == 6 && _body.velocity.y <= 0) // 점프 후 착지했다면
                {
                    m_isSkill = false;
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

                if (!m_isSkill && !m_isJump && other.gameObject.layer == 6 && _body.velocity.y >= 0) // 땅에 있다면
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
            catch (NullReferenceException)
            {
            }
        }

        private void OnTriggerExit2D(Collider2D other) // 타일의 경계선을 나가도 실행이 됨.
        {
            if (other.gameObject.layer == 6)
            {
                m_iGroundjump = 0;
                _body.drag = 0.0f;
                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("_DoCheckCollider", 0.05f); // 다시 체크.
            }
        }

        private void _DoCheckCollider() // 다시 충돌 감지
        {
            if (!this.GetComponent<BoxCollider2D>().enabled)
                this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}