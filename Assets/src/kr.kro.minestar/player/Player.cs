using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using src.sjh.Scripts;
using UnityEngine;

namespace src.kr.kro.minestar.player
{
    public class Player : MonoBehaviour
    {
        /// ##### Constant Field #####
        private const float MoveForce = 6F;
        private const float JumpForce = 13F;
        private const float GizmoSize = 5F;
        private const byte DefaultAirJumpAmount = 2;

        /// ##### Field #####
        private readonly GameSystem _gameSystem;

        private readonly PlayerCharacter _playerCharacter;

        private readonly Rigidbody2D _body; // 플레이어 물리
        private SpriteRenderer _spriteRenderer; // 스프라이트 정보

        private int _airJumpAmount; // 공중 점프 가능 횟수
        private Transform _target; // 감지된 물체
        private bool m_isLanding; // 땅에 닿았는가

        private readonly List<Effect> _effects = new ();
        [CanBeNull] private string _item;

        private PlayerMove _playerMove;

        /// ##### Constructor #####
        public Player(GameSystem gameSystem, PlayerCharacterEnum playerCharacterEnum)
        {
            _gameSystem = gameSystem;
            _playerCharacter = PlayerCharacter.FromEnum(this, playerCharacterEnum);

            m_isLanding = false;

            _airJumpAmount = DefaultAirJumpAmount;
            // _effects = new List<Effect>();

            // _body = GetComponent<Rigidbody2D>();
            // _spriteRenderer = GetComponent<SpriteRenderer>();

            _body = new Rigidbody2D();
            _spriteRenderer = new SpriteRenderer();
        }

        /// ##### Unity Functions #####
        private void Update()
        {
            DoJump(); // 점프
            DoMove(); // 좌우 이동
            DoDetect(); // 범위내 오브젝트 감지
        }

        // private void FixedUpdate()
        // {
        //     // 플렛폼 타입 알아내기
        //     Debug.DrawRay(_body.position, Vector3.down, new Color(0, 1, 0));
        //     var rayHit = Physics2D.Raycast(_body.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
        //     var rayHitCollider = rayHit.collider;
        //     
        //     //if (rayHitCollider != null) Debug.Log(rayHitCollider.tag);
        //     if (_body.velocity.y < -10.0f && !m_isLanding) _body.drag = 2.0f;
        // }

        /// ##### Get Functions #####
        public PlayerCharacter GetPlayerCharacter() => _playerCharacter;

        public List<Effect> GetEffects => _effects;

        public float GetMoveForce()
        {
            var value = MoveForce;

            if (_effects == null || _effects.Count == 0) return value;

            // Add Calculate
            foreach (var effect in _effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Add))
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
            foreach (var effect in _effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Multi))
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

        public float GetJumpForce()
        {
            var value = JumpForce;

            if (_effects == null || _effects.Count == 0) return value;
                
            foreach (var effect in _effects)
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

        public GameSystem GetGameSystem() => _gameSystem;

        public PassiveSkill GetPassiveSkill() => _playerCharacter.GetPassiveSkill();
        public ActiveSkill GetGetActiveSkill1() => _playerCharacter.GetActiveSkill1();
        public ActiveSkill GetGetActiveSkill2() => _playerCharacter.GetActiveSkill2();

        ///##### Effect Functions #####
        public void AddEffect(Effect effect)
        {
            _effects.Add(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            _effects.Remove(effect);
        }

        /// ##### Calculate Functions #####
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

        ///###### Do Functions #####
        private void DoJump()
        {
            var value = GetJumpForce();

            if (_airJumpAmount <= 0) return;
            // 점프 횟수 추가
            if (!(Input.GetKeyDown(KeyCode.Space) & _airJumpAmount != 0)) return;
            _airJumpAmount--;

            _body.velocity = Vector2.zero;
            var jumpVelocity = new Vector2(0, value); // 점프 속력
            _body.AddForce(jumpVelocity, ForceMode2D.Impulse);
            m_isLanding = false;

            var playerEvent = new PlayerJumpEvent(this, value);
        }
        
        private void DoMove()
        {
            var value = GetMoveForce();
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                value *= -1;
                _spriteRenderer.flipX = true;
            }

            _body.velocity = new Vector2(value, _body.velocity.y);
        }
        
        private void DoDetect() // 플레이어 혹은 물체 감지 - 나중에 스킬로 옮기면됨
        {
            if (!Input.GetKeyDown(KeyCode.V)) return;
            var cols = Physics2D.OverlapCircleAll(transform.position, GizmoSize);
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


        private void DoUseActiveSkill1()
        {
            var skill = GetGetActiveSkill1();
            skill.UseSkill(this);

            var playerEvent = new PlayerUseActiveSkill1Event(this, skill);
        }

        private void DoUseActiveSkill2()
        {
            var skill = GetGetActiveSkill2();
            skill.UseSkill(this);

            var playerEvent = new PlayerUseActiveSkill1Event(this, skill);
        }


        /// ##### Other Functions #####
        public int LandingAirJumpAmountCharge()
        {
            var value = 1;

            // Add Calculate
            foreach (var effect in _effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Add))
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
            foreach (var effect in _effects.Where(effect => effect.GetValueCalculator() == ValueCalculator.Multi))
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

            _airJumpAmount = value;
            return value;
        }

        public void AddVector(Vector2 vector) => _body.AddForce(vector);

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
            Gizmos.DrawWireSphere(transform.position, GizmoSize);
        }
    }
}