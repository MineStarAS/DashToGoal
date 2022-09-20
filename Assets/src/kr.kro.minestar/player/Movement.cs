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
        private Player Player { get; set; }

        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveForce;
        [SerializeField] private float jumpForce;

        private bool IsGround { get; set; }
        
        private int AirJumpAmount { get; set; }

        private Rigidbody2D Body{ get; set; }
        
        private SpriteRenderer SpriteRenderer{ get; set; }

        /// ##### Unity Functions #####
        private void Start()
        {
            Player = GetComponent<Player>();
            Body = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();

            IsGround = false;
            AirJumpAmount = 1;

            if (moveSpeed <= 0) moveSpeed = 8.0F;
            if (moveForce <= 0) moveForce = 0.1F;
            if (jumpForce <= 0) jumpForce = 13.0F;
        }

        private void Update()
        {
            DoJump();
            DoMove();
        }

        private void FixedUpdate()
        {
            if (Body != null && Body.velocity.y < -10.0f) SetDrag(2F);
            if (Player != null && Player.transform.position.y < -15) Player.transform.position = new Vector3(0, 0, 0);
        }

        /// ##### Calculate Functions #####
        private float GetMoveForce()
        {
            float value = moveSpeed;
            
            value += value * Player.Effects.ValueMoveSpeed;

            return value;
        }

        private float GetJumpForce()
        {
            float value = jumpForce;

            value += value * Player.Effects.ValueJumpForce;

            return value;
        }

        private int AirJumpAmountCharge()
        {
            int value = 1;
            
            value += Player.Effects.ValueBonusAirJump;
            
            return value;
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

            AddMovementFlip(moveForce, 0);

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
                if (other.tag == "Finish") Player.IsGoal = true;
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