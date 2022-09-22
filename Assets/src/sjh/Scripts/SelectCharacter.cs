using UnityEngine;

namespace src.sjh.Scripts
{
    public class SelectCharacter : MonoBehaviour
    {
        [SerializeField] private GameObject gameManager;
        [SerializeField] private GameObject prefabPlayer;
        private CharacterManager _characterManager;

        private void Start()
        {
            _characterManager = gameManager.GetComponent<CharacterManager>();
        }

        public void OnClickPlayer()
        {
            _characterManager.SetPlayer(prefabPlayer);
        }
    }
}