using System.Collections.Generic;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player.effect;
using src.kr.kro.minestar.player.skill;
using UnityEngine;

// ReSharper disable ObjectCreationAsStatement

namespace src.kr.kro.minestar.player
{
    public class Player : MonoBehaviour
    {
        /// ##### Field #####
        public GameSystem GameSystem { get; private set; }
        public UIManager EffectsUI { get; private set; }
        public GameObject GameManager;

        [SerializeField] private PlayerCharacterEnum playerCharacterEnum;
        public PlayerCharacter PlayerCharacter { get; private set; }
        public Dictionary<string, Effect> Effects { get; private set; }

        [SerializeField] public float maxSpeed;
        [SerializeField] public float moveForce;
        [SerializeField] public float jumpForce;
        public Movement Movement { get; private set; }
        public bool m_IsGoal;
        public bool IsGoal { get => m_IsGoal; set => m_IsGoal = value; }

        /// ##### Unity Functions #####
        private void Start()
        {
            m_IsGoal = false;
            Effects = new Dictionary<string, Effect>();
            GameManager = GameObject.Find("GameManager");
            GameSystem = GameManager.GetComponent<GameSystem>();
            EffectsUI = GameManager.GetComponent<UIManager>();
            Movement = new Movement(this);
            PlayerCharacter = PlayerCharacter.FromEnum(this, playerCharacterEnum);

            GameSystem.RegisterPlayer(this);
        }

        private void Update()
        {
            if (m_IsGoal) return;
            Movement.DoJump(); // 점프
            Movement.DoMove(); // 좌우 이동
            DoUseSkill();
            //DoEffect();
        }

        private void FixedUpdate()
        {
            Movement.FixedCheck();
        }

        private void OnTriggerEnter2D(Collider2D other) => Movement.OnTriggerEnter2D(other);

        private void OnTriggerStay2D(Collider2D other) => Movement.OnTriggerStay2D(other);

        private void OnTriggerExit2D(Collider2D other) => Movement.OnTriggerExit2D(other);

        private void DoCheckCollider() // 다시 충돌 감지
        {
            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            if (!boxCollider2D.enabled) boxCollider2D.enabled = true;
        }

        ///##### Effect Functions #####
        public void AddEffect(Effect effect)
        {
            if (Effects.ContainsKey(effect.Name)) Effects[effect.Name].RemoveEffect();
            Effects.Add(effect.Name, effect);
            //EffectsUI.func_DoEffect(effect);
        }

        public void RemoveEffect(Effect effect) => Effects.Remove(effect.Name);


        // ReSharper disable Unity.PerformanceAnalysis
        ///###### Do Functions #####
        private void DoUseSkill()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                DoUseActiveSkill1();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                DoUseActiveSkill2();
            }
        }

        private void DoUseActiveSkill1()
        {
            ActiveSkill skill = PlayerCharacter.ActiveSkill1;
            if (skill.UseSkill()) new PlayerUseActiveSkill1Event(this, skill);
        }

        private void DoUseActiveSkill2()
        {
            ActiveSkill skill = PlayerCharacter.ActiveSkill2;
            if (skill.UseSkill()) new PlayerUseActiveSkill2Event(this, skill);
        }
    }
}