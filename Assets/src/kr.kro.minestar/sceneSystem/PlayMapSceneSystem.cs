using src.kr.kro.minestar.ui;
using UnityEngine;

namespace src.kr.kro.minestar.sceneSystem
{
    public class PlayMapSceneSystem: MonoBehaviour
    {
        private void Start()
        {
            GameObject player = Resources.Load<GameObject>($"Player/{CharacterSelectSceneSystem.ClickedButton.PlayerCharacterEnum}");
            Instantiate(player).transform.position = Vector3.zero;

            GameObject gameSystemGameObject = new GameObject("GameSystem");
            GameSystem gameSystem = gameSystemGameObject.AddComponent<GameSystem>();

            GameObject iconUIManagerGameObject = new GameObject("IconUIManager");
            iconUIManagerGameObject.transform.SetParent(GameObject.Find("UI_Canvas").transform);
            iconUIManagerGameObject.AddComponent<IconUIManager>();
        }
    }
}