using System.Collections.Generic;
using src.kr.kro.minestar.player.effect;
using UnityEngine;

// ReSharper disable ObjectCreationAsStatement

namespace src.kr.kro.minestar.player
{
    public class Player : MonoBehaviour
    {
        /// ##### Field #####
        public GameSystem GameSystem { get; private set; }

        public UIManager EffectsUI { get; private set; }
        public GameObject GameManager { get; private set; }
        public Dictionary<string, Effect> Effects { get; private set; }

        [SerializeField] private PlayerCharacterEnum character;
        public PlayerCharacter PlayerCharacter { get; private set; }
        public Movement Movement { get; private set; }
        public bool IsGoal { get; set; }

        /// ##### Unity Functions #####
        private void Start()
        {
            IsGoal = false;
            Effects = new Dictionary<string, Effect>();
            GameManager = GameObject.Find("GameManager");
            GameSystem = GameManager.GetComponent<GameSystem>();
            EffectsUI = GameManager.GetComponent<UIManager>();

            PlayerCharacter = PlayerCharacter.FromEnum(this, character);
            Movement = GetComponent<Movement>();

            GameSystem.RegisterPlayer(this);
        }

        private void Update()
        {
            if (IsGoal) return;
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
            EffectsUI.func_DoEffect(effect);
        }

        public void RemoveEffect(Effect effect) => Effects.Remove(effect.Name);


        // ReSharper disable Unity.PerformanceAnalysis
        ///###### Do Functions #####
        private void DoUseSkill()
        {
            if (Input.GetKeyDown(KeyCode.Z)) DoUseActiveSkill1();
            if (Input.GetKeyDown(KeyCode.X)) DoUseActiveSkill2();
        }

        private void DoUseActiveSkill1() => PlayerCharacter.ActiveSkill1.UseSkill();

        private void DoUseActiveSkill2() => PlayerCharacter.ActiveSkill2.UseSkill();
    }
}