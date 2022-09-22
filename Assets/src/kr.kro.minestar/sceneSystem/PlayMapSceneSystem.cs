using UnityEngine;

namespace src.kr.kro.minestar.sceneSystem
{
    public class PlayMapSceneSystem: MonoBehaviour
    {
        private void Start()
        {
            GameObject player = Resources.Load<GameObject>($"Player/{CharacterSelectSceneSystem.ClickedButton.PlayerCharacterEnum}");
            Instantiate(player).transform.position = Vector3.zero;

            GameObject newGameObject = new GameObject("GameSystem");
            GameSystem gameSystem = newGameObject.AddComponent<GameSystem>();
            
        }
    }
}