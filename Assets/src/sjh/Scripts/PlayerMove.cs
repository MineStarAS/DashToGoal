using System;
using System.Linq;
using UnityEngine;
using src.kr.kro.minestar.player;
using Unity.VisualScripting;
using System.Linq.Expressions;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;

namespace src.sjh.Scripts
{
    public class PlayerMove : MonoBehaviour
    {
        /// ##### Field #####
        private Player m_Player;

        private float m_fMaxSpeed = 8.0f; // 플레이어 이동속도
        private float moveForce = 0.05f; // 플레이어 이동에 가해지는 힘
        private float jumpForce = 13.0f; // 플레이어 점프 힘
        private bool m_isJump; // 점프키를 눌렀는가

        private const int DefaultAirJumpAmount = 1; // 공중 점프 가능한 횟수.
        private int _airJumpAmount = DefaultAirJumpAmount; // 공중 점프 가능 횟수
        private int m_iGroundjump; // 땅에 있을때 점프할 수 있는 
        private Rigidbody2D _body; // 플레이어 물리
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
                    _body.drag = 2.0f; // 플레이어 낙하 속도
                }
            }

            if (transform.position.y < -15) transform.position = new Vector3(0, 0, 0);
        }
        
        /// ##### Calculate Functions #####
        
        private float GetMoveForce()
        {
            var value = m_fMaxSpeed;
            var effects = m_Player.GetEffects();

            if (effects == null || effects.Count == 0) return value;

            // Add Calculate
            foreach (var effect in effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Add))
            {
                switch (effect.GetEffectType())
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
            foreach (var effect in effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Multi))
            {
                switch (effect.GetEffectType())
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
            var effects = m_Player.GetEffects();

            if (effects == null || effects.Count == 0) return value;
            
            foreach (var effect in effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Add))
            {
                switch (effect.GetEffectType())
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
            
            foreach (var effect in effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Multi))
            {
                switch (effect.GetEffectType())
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
            var effects = m_Player.GetEffects();

            // Add Calculate
            foreach (var effect in effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Add))
            {
                switch (effect.GetEffectType())
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
            foreach (var effect in effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Multi))
            {
                switch (effect.GetEffectType())
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
            return effect.GetValueCalculator() switch
            {
                ValueCalculator.Add => value + effect.GetCalculatorValue(),
                ValueCalculator.Multi => value * effect.GetCalculatorValue(),
                _ => value
            };
        }

        private static int Calculate(int value, Effect effect)
        {
            return effect.GetValueCalculator() switch
            {
                ValueCalculator.Add => value + Convert.ToByte(effect.GetCalculatorValue()),
                ValueCalculator.Multi => value * Convert.ToByte(effect.GetCalculatorValue()),
                _ => value
            };
        }

        /// ##### Movement Functions #####
        public void DoMove()
        {
            float h = 0;
            var maxMoveForce = GetMoveForce();
            // 움직임
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                h = 1;
                _body.drag = 0.0f;
                _spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                h = -1;
                _body.drag = 0.0f; // 저항값
                _spriteRenderer.flipX = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                h = 0;
            }

            if(h == 0)
                _body.velocity = new Vector2(0, _body.velocity.y);
            else
                _body.AddForce(Vector2.right * h * moveForce, ForceMode2D.Impulse);


            if (_body.velocity.x > maxMoveForce)
                _body.velocity = new Vector2(maxMoveForce, _body.velocity.y);
            else if (_body.velocity.x < -maxMoveForce)
                _body.velocity = new Vector2(-maxMoveForce, _body.velocity.y);

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

            new PlayerMoveEvent(m_Player);
        }

        public void DoJump()
        {
            if (!Input.GetKeyDown(KeyCode.C) || _airJumpAmount <= 0) return;
            var jf = GetJumpForce();

            // 점프 횟수 추가
            m_isJump = true;
            if (m_iGroundjump == 0) _airJumpAmount--;
            _body.drag = 0.0f;
            _body.velocity = new Vector2(_body.velocity.x, 0);
            _body.AddForce(Vector2.up * jf, ForceMode2D.Impulse);
            
            new PlayerJumpEvent(m_Player);
        }

        public void AddMovement(float x, float y) => _body.AddForce(new Vector2(x, y));
        
        public void AddMovementFlip(float x, float y) => _body.AddForce(!_spriteRenderer.flipX ? new Vector2(x, y) : new Vector2(-x, y));
       

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

        private void _DoCheckCollider() // 다시 충돌 감지
        {
            if (!this.GetComponent<BoxCollider2D>().enabled)
                this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}