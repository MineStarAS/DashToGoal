using System;
using System.Linq;
using UnityEngine;
using src.kr.kro.minestar.player;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using System.Collections.Generic;

namespace src.sjh.Scripts
{
    public class Movement
    {
        /// ##### Field #####
        public Player Player { get; private set; }

        public float Flip { get; private set; }
        public float MaxSpeed { get; private set; } // 플레이어 이동속도
        public float MoveForce { get; private set; } // 플레이어 이동에 가해지는 힘
        public float JumpForce { get; private set; } // 플레이어 점프 힘
        public bool IsJump { get; private set; } // 점프키를 눌렀는가
        
        private bool m_isSkill = false; // 스킬 사용
        public bool isSkill { get => m_isSkill; set => m_isSkill = value; }

        private const int DefaultAirJumpAmount = 1; // 공중 점프 가능한 횟수.
        private int AirJumpAmount = DefaultAirJumpAmount; // 공중 점프 가능 횟수
        private int GroundJumpAmount; // 땅에 있을때 점프할 수 있는 회쇼ㅜ
        public Rigidbody2D Body; // 플레이어 물리
        private SpriteRenderer SpriteRenderer; // 스프라이트 정보

        /// ##### Unity Functions #####
        public Movement(Player player)
        {
            Player = player;
            Body = player.GetComponent<Rigidbody2D>();
            SpriteRenderer = player.GetComponent<SpriteRenderer>();

            MaxSpeed = player.maxSpeed <= 0 ? 8.0f : player.maxSpeed;
            MoveForce = player.moveForce <= 0 ? 0.05f : player.moveForce;
            JumpForce = player.jumpForce <= 0 ? 13.0f : player.jumpForce;
        }

        public void FixedCheck()
        {
            if (Body != null)
            {
                if (Body.velocity.y < -10.0f)
                {
                    SetDrag(2F);
                }
            }

            if (Player.transform.position.y < -15) Player.transform.position = new Vector3(0, 0, 0);
        }

        /// ##### Calculate Functions #####
        private float GetMoveForce()
        {
            float value = MaxSpeed;
            Dictionary<string, Effect>.ValueCollection effects = Player.Effects.Values;

            if (effects.Count == 0) return value;

            // Add Calculate
            foreach (Effect effect in effects.Where(effect => effect.Calculator == Calculator.Add))
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
                    case EffectType.CoolTimeReduction:
                    case EffectType.CoolTimeIncrease:
                    default:
                        continue;
                }
            }

            // Multi Calculate
            foreach (Effect effect in effects.Where(effect => effect.Calculator == Calculator.Multi))
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
                    case EffectType.CoolTimeReduction:
                    case EffectType.CoolTimeIncrease:
                    default:
                        continue;
                }
            }

            return value;
        }

        private float GetJumpForce()
        {
            float value = JumpForce;
            Dictionary<string, Effect>.ValueCollection effects = Player.Effects.Values;

            if (effects.Count == 0) return value;

            foreach (Effect effect in effects.Where(effect => effect.Calculator == Calculator.Add))
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
                    case EffectType.CoolTimeReduction:
                    case EffectType.CoolTimeIncrease:
                    default:
                        continue;
                }
            }

            foreach (Effect effect in effects.Where(effect => effect.Calculator == Calculator.Multi))
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
                    case EffectType.CoolTimeReduction:
                    case EffectType.CoolTimeIncrease:
                    default:
                        continue;
                }
            }

            return value;
        }

        public int LandingAirJumpAmountCharge()
        {
            int value = DefaultAirJumpAmount;
            Dictionary<string, Effect>.ValueCollection effects = Player.Effects.Values;

            // Add Calculate
            foreach (Effect effect in effects.Where(effect => effect.Calculator == Calculator.Add))
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
                    case EffectType.CoolTimeReduction:
                    case EffectType.CoolTimeIncrease:
                    default:
                        continue;
                }
            }

            // Multi Calculate
            foreach (Effect effect in effects.Where(effect => effect.Calculator == Calculator.Multi))
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
                    case EffectType.CoolTimeReduction:
                    case EffectType.CoolTimeIncrease:
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
             float maxMoveForce = GetMoveForce();
            // 움직임
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                Flip = 1;
                Body.drag = 0.0f;
                SpriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                Flip = -1;
                Body.drag = 0.0f; // 저항값
                SpriteRenderer.flipX = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                Flip = -2;
            }

            if (Flip == -2) // 좌우키 동시 입력
            {
                if(Body.velocity.x > 0.1f) Body.AddForce(Vector2.right * -1 * MoveForce, ForceMode2D.Impulse);
                else if (Body.velocity.x < -0.1f) Body.AddForce(Vector2.right * 1 * MoveForce, ForceMode2D.Impulse);
                else Body.velocity = new Vector2(0, Body.velocity.y);

                return;
            }


            if (Body.velocity.x > maxMoveForce) // 최고 속도보다 클 때
            {
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    if (GroundJumpAmount == 0 || IsJump == true) return; // 플레이어가 공중에 있으면 실행 못하게
                    SetDrag(30F);
                }
                if (!Input.GetKey(KeyCode.LeftArrow)) return;
                Body.AddForce(Vector2.right * Flip * MoveForce, ForceMode2D.Impulse);
            }
            if (Body.velocity.x < -maxMoveForce) // 최고 속도보다 작을 때
            {
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    if (GroundJumpAmount == 0 || IsJump == true) return; // 플레이어가 공중에 있으면 실행 못하게
                    SetDrag(30F);
                }
                if (!Input.GetKey(KeyCode.RightArrow)) return;
                Body.AddForce(Vector2.right * Flip * MoveForce, ForceMode2D.Impulse);
            }
            else                                    // 현재 속도가 최고 속도보다 작지도 크지도 않을 때
            {
                if (Flip == -1 && Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    if (GroundJumpAmount == 0 || IsJump) return; // 플레이어가 공중에 있으면 실행 못하게
                    SetDrag(30F);
                }
                else if (Flip == 1 && Input.GetKeyUp(KeyCode.RightArrow))
                {
                    if (GroundJumpAmount == 0 || IsJump) return; // 플레이어가 공중에 있으면 실행 못하게
                    SetDrag(30F);
                }

                if (Body.drag != 30 && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
                {
                    Body.AddForce(Vector2.right * Flip * MoveForce, ForceMode2D.Impulse);
                }
            }

            new PlayerMoveEvent(Player);
        }

        public void DoJump()
        {
            if (!Input.GetKeyDown(KeyCode.C) || AirJumpAmount <= 0) return;
            float jf = GetJumpForce();

            // 점프 횟수 추가
            IsJump = true;
            if (GroundJumpAmount == 0) AirJumpAmount--;
            Body.drag = 0.0f;
            Body.velocity = new Vector2(Body.velocity.x, 0);
            Body.AddForce(Vector2.up * jf, ForceMode2D.Impulse);
            new PlayerJumpEvent(Player);
        }


        public void SetMovement(float x, float y) => Body.velocity = new Vector2(x, y);
        public void SetMovementFlip(float x, float y) => Body.velocity = !SpriteRenderer.flipX ? new Vector2(x, y) : new Vector2(-x, y);

        public void AddMovement(float x, float y) => Body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
        public void AddMovementFlip(float x, float y) => Body.AddForce(!SpriteRenderer.flipX ? new Vector2(x, y) : new Vector2(-x, y), ForceMode2D.Impulse);
        
        public void SetDrag(float value) => Body.drag = value;


        public void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(Body.velocity.y);
            try
            {
                if (other.gameObject.layer == 6 && (Body.velocity.y <= 0 || Body.drag == 2)) // 점프 후 착지했다면
                {
                    Debug.Log("Hello");
                    m_isSkill = false;
                    IsJump = false;
                    AirJumpAmount = DefaultAirJumpAmount;
                    Body.drag = 0;
                    Body.velocity = new Vector2(Body.velocity.x, -1);
                    GroundJumpAmount = 1;
                    if (!Input.GetKey(KeyCode.RightArrow) || !Input.GetKey(KeyCode.LeftArrow))
                    {
                        Body.drag = 30.0f;
                    }
                }

                if (!m_isSkill && !IsJump && other.gameObject.layer == 6 && Body.velocity.y >= 0) // 땅에 있다면
                {
                    GroundJumpAmount = 1;
                    AirJumpAmount = DefaultAirJumpAmount;
                    Body.velocity = new Vector2(Body.velocity.x, -1);
                    if (!Input.GetKey(KeyCode.RightArrow) || !Input.GetKey(KeyCode.LeftArrow))
                    {
                        Body.drag = 30.0f;
                    }
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        public void OnTriggerExit2D(Collider2D other) // 타일의 경계선을 나가도 실행이 됨.
        {
            if (other.gameObject.layer != 6) return;
            
            GroundJumpAmount = 0;
            Body.drag = 0.0f;
            Player.GetComponent<BoxCollider2D>().enabled = false;
            Player.Invoke("DoCheckCollider", 0.05f); // 다시 체크.
        }
    }
}