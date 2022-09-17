using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace src.kr.kro.minestar.player
{
    public class Movement
    {
        /// ##### Constant Field #####
        private const float Drag = 30;

        /// ##### Field #####
        public Player Player { get; }

        public float MaxSpeed { get; } // 플레이어 이동속도
        public float MoveForce { get; } // 플레이어 이동에 가해지는 힘
        public float JumpForce { get; } // 플레이어 점프 힘

        public bool IsGround { get; private set; }

        private const int DefaultAirJumpAmount = 1; // 공중 점프 가능한 횟수.
        private int AirJumpAmount = DefaultAirJumpAmount; // 공중 점프 가능 횟수

        public Rigidbody2D Body; // 플레이어 물리
        private SpriteRenderer SpriteRenderer; // 스프라이트 정보

        /// ##### Unity Functions #####
        public Movement(Player player)
        {
            Player = player;
            Body = player.GetComponent<Rigidbody2D>();
            SpriteRenderer = player.GetComponent<SpriteRenderer>();

            MaxSpeed = player.maxSpeed <= 0 ? 8.0f : player.maxSpeed;
            MoveForce = player.moveForce <= 0 ? 0.1f : player.moveForce;
            JumpForce = player.jumpForce <= 0 ? 13.0f : player.jumpForce;
        }

        public void FixedCheck()
        {
            if (Body != null && Body.velocity.y < -10.0f) SetDrag(2F);
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

        private int AirJumpAmountCharge()
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
                Calculator.Add => value + Convert.ToInt32(effect.CalculatorValue),
                Calculator.Multi => value * Convert.ToInt32(effect.CalculatorValue),
                _ => value
            };
        }

        /// ##### Movement Functions #####
        public void DoMove()
        {
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) ||
                !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                if (IsGround) SetDrag(Drag);
                return;
            }

            float maxMoveForce = GetMoveForce();

            SetDrag(0);
            SpriteRenderer.flipX = !Input.GetKey(KeyCode.RightArrow);
            if (maxMoveForce < Math.Abs(Body.velocity.x) && (0 < Body.velocity.x) == !SpriteRenderer.flipX) return;


            if (IsGround)
            {
                if ((0 < Body.velocity.x) == SpriteRenderer.flipX) SetMovementXFlip(maxMoveForce / 2);
                else if (Math.Abs(Body.velocity.x) < maxMoveForce / 2) SetMovementXFlip(maxMoveForce / 2);
            }

            AddMovementFlip(MoveForce, 0);

            new PlayerMoveEvent(Player);
        }

        public void DoJump()
        {
            if (!Input.GetKeyDown(KeyCode.C) || AirJumpAmount <= 0) return;
            float jumpForce = GetJumpForce();
            if (!IsGround) AirJumpAmount--;
            IsGround = false;
            SetDrag(0);
            SetMovementY(0);
            SetMovementY(jumpForce);
            new PlayerJumpEvent(Player);
        }


        public void SetMovement(float x, float y) => Body.velocity = new Vector2(x, y);
        public void SetMovementFlip(float x, float y) => Body.velocity = !SpriteRenderer.flipX ? new Vector2(x, y) : new Vector2(-x, y);

        public void SetMovementX(float x) => Body.velocity = new Vector2(x, Body.velocity.y);
        public void SetMovementXFlip(float x) => Body.velocity = !SpriteRenderer.flipX ? new Vector2(x, Body.velocity.y) : new Vector2(-x, Body.velocity.y);
        public void SetMovementY(float y) => Body.velocity = new Vector2(Body.velocity.x, y);

        public void AddMovement(float x, float y) => Body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
        public void AddMovementFlip(float x, float y) => Body.AddForce(!SpriteRenderer.flipX ? new Vector2(x, y) : new Vector2(-x, y), ForceMode2D.Impulse);

        public void SetDrag(float value) => Body.drag = value;


        public void OnTriggerEnter2D(Collider2D other)
        {
            try
            {
                if (other.GetComponent<PlatformEffector2D>() != null)
                    if (Body.transform.position.y - other.transform.position.y <= -0.05)
                        return;
                IsGround = true;
                SetDrag(Drag);
                AirJumpAmount = AirJumpAmountCharge();
            }
            catch (NullReferenceException)
            {
            }
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if (other.GetComponent<PlatformEffector2D>() != null)
                if (Body.transform.position.y - other.transform.position.y <= -0.05)
                    return;
            IsGround = true;
        }

        public void OnTriggerExit2D(Collider2D other) // 타일의 경계선을 나가도 실행이 됨.
        {
            IsGround = false;
            SetDrag(0);
        }
    }
}