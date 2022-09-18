using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace src.kr.kro.minestar.player
{
    public class Movement : MonoBehaviour
    {
        /// ##### Constant Field #####
        private const float Drag = 30;

        /// ##### Field #####
        private Player _player;

        [SerializeField] private float maxSpeed;
        [SerializeField] private float moveForce;
        [SerializeField] private float jumpForce;

        private bool _isGround;
        
        private int _airJumpAmount; // 공중 점프 가능 횟수

        private Rigidbody2D _body; // 플레이어 물리
        private SpriteRenderer _spriteRenderer; // 스프라이트 정보

        /// ##### Unity Functions #####
        private void Start()
        {
            _player = GetComponent<Player>();
            _body = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _isGround = false;
            _airJumpAmount = 1;

            if (maxSpeed <= 0) maxSpeed = 8.0F;
            if (moveForce <= 0) moveForce = 0.1F;
            if (jumpForce <= 0) jumpForce = 13.0F;
        }

        public void FixedCheck()
        {
            if (_body != null && _body.velocity.y < -10.0f) SetDrag(2F);
            if (_player != null && _player.transform.position.y < -15) _player.transform.position = new Vector3(0, 0, 0);
        }

        /// ##### Calculate Functions #####
        private float GetMoveForce()
        {
            float value = maxSpeed;
            Dictionary<string, Effect>.ValueCollection effects = _player.Effects.Values;

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
            float value = jumpForce;
            Dictionary<string, Effect>.ValueCollection effects = _player.Effects.Values;

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
            int value = 1;
            Dictionary<string, Effect>.ValueCollection effects = _player.Effects.Values;

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
                if (_isGround) SetDrag(Drag);
                return;
            }

            float maxMoveForce = GetMoveForce();

            SetDrag(0);
            _spriteRenderer.flipX = !Input.GetKey(KeyCode.RightArrow);
            if (maxMoveForce < Math.Abs(_body.velocity.x) && (0 < _body.velocity.x) == !_spriteRenderer.flipX) return;


            if (_isGround)
            {
                if ((0 < _body.velocity.x) == _spriteRenderer.flipX) SetMovementXFlip(maxMoveForce / 2);
                else if (Math.Abs(_body.velocity.x) < maxMoveForce / 2) SetMovementXFlip(maxMoveForce / 2);
            }

            AddMovementFlip(moveForce, 0);

            new PlayerMoveEvent(_player);
        }

        public void DoJump()
        {
            if (!Input.GetKeyDown(KeyCode.C) || _airJumpAmount <= 0) return;
            float jumpForce = GetJumpForce();
            if (!_isGround) _airJumpAmount--;
            _isGround = false;
            SetDrag(0);
            SetMovementY(0);
            SetMovementY(jumpForce);
            new PlayerJumpEvent(_player);
        }


        public void SetMovement(float x, float y) => _body.velocity = new Vector2(x, y);
        public void SetMovementFlip(float x, float y) => _body.velocity = !_spriteRenderer.flipX ? new Vector2(x, y) : new Vector2(-x, y);

        public void SetMovementX(float x) => _body.velocity = new Vector2(x, _body.velocity.y);
        public void SetMovementXFlip(float x) => _body.velocity = !_spriteRenderer.flipX ? new Vector2(x, _body.velocity.y) : new Vector2(-x, _body.velocity.y);
        public void SetMovementY(float y) => _body.velocity = new Vector2(_body.velocity.x, y);

        public void AddMovement(float x, float y) => _body.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
        public void AddMovementFlip(float x, float y) => _body.AddForce(!_spriteRenderer.flipX ? new Vector2(x, y) : new Vector2(-x, y), ForceMode2D.Impulse);

        public void SetDrag(float value) => _body.drag = value;


        public void OnTriggerEnter2D(Collider2D other)
        {
            try
            {
                if (other.tag == "Finish") _player.IsGoal = true;
                if (other.GetComponent<PlatformEffector2D>() != null)
                    if (_body.transform.position.y - other.transform.position.y <= -0.05)
                        return;
                _isGround = true;
                SetDrag(Drag);
                _airJumpAmount = AirJumpAmountCharge();
            }
            catch (NullReferenceException)
            {
            }
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if (other.GetComponent<PlatformEffector2D>() != null)
                if (_body.transform.position.y - other.transform.position.y <= -0.05)
                    return;
            _isGround = true;
        }

        public void OnTriggerExit2D(Collider2D other) // 타일의 경계선을 나가도 실행이 됨.
        {
            _isGround = false;
            SetDrag(0);
        }
    }
}