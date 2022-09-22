using src.kr.kro.minestar.player.character;
using System.Collections.Generic;
using src.kr.kro.minestar.player.effect;
using UnityEditor.SceneManagement;
using UnityEngine;

// ReSharper disable ObjectCreationAsStatement

namespace src.kr.kro.minestar.player
{
    public class Player : MonoBehaviour
    {
        /// ##### Field #####
        public GameSystem GameSystem { get; private set; }
        public Effects Effects { get; private set; }

        [SerializeField] private PlayerCharacterEnum character;
        public PlayerCharacter PlayerCharacter { get; private set; }
        public Movement Movement { get; private set; }

        /// ##### Unity Functions #####
        private void Start()
        {
            GameSystem = gameObject.GetComponent<GameSystem>();
            Effects = gameObject.AddComponent<Effects>();

            PlayerCharacter = PlayerCharacter.FromEnum(this, character);
            Movement = gameObject.AddComponent<Movement>();

            GameSystem.RegisterPlayer(this);
        }

        private void Update()
        {
            DoUseSkill();
        }

        private void OnTriggerEnter2D(Collider2D other) => Movement.OnTriggerEnter2D(other);

        private void OnTriggerStay2D(Collider2D other) => Movement.OnTriggerStay2D(other);

        private void OnTriggerExit2D(Collider2D other) => Movement.OnTriggerExit2D(other);
        
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