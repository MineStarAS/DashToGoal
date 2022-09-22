using System.Collections.Generic;
using src.kr.kro.minestar.gameEvent;
using src.kr.kro.minestar.player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace src.kr.kro.minestar
{
    public class GameSystem : MonoBehaviour
    {
        public GameEventOperator GameEventOperator { get; private set; }

        public HashSet<Player> Players { get; private set; }

        /// ##### Constructor #####
        private void Start()
        {
            GameEventOperator = new GameEventOperator(this);
            Players = new HashSet<Player>();
            StartScheduler();
        }

        public void RegisterPlayer(Player player) => Players.Add(player);

        private void StartScheduler()
        {
            int countDown = 3;
            GameObject titleTextGameObject = new GameObject("titleText");
            titleTextGameObject.transform.SetParent(GameObject.Find("UI_Canvas").transform);

            RectTransform rectTransform = titleTextGameObject.AddComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.localPosition = Vector3.zero;
            rectTransform.sizeDelta = new Vector2(500, 500);
            
            Text titleText = titleTextGameObject.AddComponent<Text>();
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.fontSize = 100;

            
            Coroutine coroutine = StartCoroutine(Timer());

            IEnumerator Timer()
            {
                while (countDown != -2)
                {
                    Debug.Log(countDown);
                    switch (countDown)
                    {
                        case 3:
                        case 2:
                        case 1:
                            SetText(countDown);
                            break;
                        case 0:
                            SetText("START!");
                            break;
                        case -1:
                            RemoveTitle();
                            break;
                    }

                    countDown--;
                    yield return new WaitForSeconds(1F);
                }
            }

            void SetText(object text) => titleText.text = text.ToString();

            void RemoveTitle() => Destroy(titleTextGameObject, 0);
        }
    }
}