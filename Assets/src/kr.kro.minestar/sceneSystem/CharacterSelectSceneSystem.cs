using src.kr.kro.minestar.player.character;
using src.kr.kro.minestar.utility;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace src.kr.kro.minestar.sceneSystem
{
    public class CharacterSelectSceneSystem : MonoBehaviour
    {
        private Canvas Canvas { get; set; }
        
        public static CharacterButton ClickedButton { get; set; } 

        private void Start()
        {
            Canvas = FindObjectOfType<Canvas>(); 

            CreateCharacterButton();
        }

        private void CreateCharacterButton()
        {
            Rect pixelRect = Canvas.pixelRect;
            float canvasX = pixelRect.width; 
            float canvasY = pixelRect.height;
            
            foreach (PlayerCharacterEnum playerCharacterEnum in Enum.GetValues(typeof(PlayerCharacterEnum)))
            {
                GameObject buttonGameObject = new($"{playerCharacterEnum}Button");
                buttonGameObject.transform.SetParent(Canvas.transform);
                CharacterButton button = buttonGameObject.AddComponent<CharacterButton>();
                button.SetCharacter(this, playerCharacterEnum);
            }
        }
    }

    public class CharacterButton : MonoBehaviour
    {
        private const float StartX = 50F;
        private const float StartY = 300F;
        
        private const float Width = 100F;
        private const float Height = 100F;
        private const float OffsetX = 10F;
        private const float OffsetY = 10F;
        private const float Scale = 2F;

        private CharacterSelectSceneSystem CharacterSelectSceneSystem { get; set; }
        public PlayerCharacterEnum PlayerCharacterEnum { get; private set; }

        private RectTransform RectTransform { get; set; }
        private Image Image { get; set; }
        private Button Button { get; set; }

        public void SetCharacter(CharacterSelectSceneSystem system, PlayerCharacterEnum playerCharacterEnum)
        {
            CharacterSelectSceneSystem = system;
            PlayerCharacterEnum = playerCharacterEnum;
            int order = Convert.ToInt32(playerCharacterEnum);
            float x = (Width * Scale + OffsetX) * order + StartX;
            float y = StartY;
            
            RectTransform = gameObject.AddComponent<RectTransform>();

            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.zero;
            RectTransform.pivot = Vector2.up;

            RectTransform.localScale = new Vector3(Scale, Scale, 1);
            RectTransform.anchoredPosition = new Vector2(x, y);


            Image = gameObject.AddComponent<Image>();
            Image.sprite = Resources.Load<Sprite>($"Player/PlayerImage/{playerCharacterEnum}");


            Button = gameObject.AddComponent<Button>();
            Button.transition = Selectable.Transition.ColorTint;
            Button.targetGraphic = Image;
            Button.onClick.AddListener(Test);
        }

        private void Test()
        {
            CharacterSelectSceneSystem.ClickedButton = this;
            SceneManager.LoadScene("map1");
        }
    }
}