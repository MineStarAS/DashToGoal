using JetBrains.Annotations;
using src.kr.kro.minestar.player;
using System;
using System.Collections;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

namespace src.kr.kro.minestar.device
{
    public abstract class Device : MonoBehaviour
    {
        [CanBeNull] public Player Player { get; protected set; }

        public void SetPosition(float x, float y)
        {
            Transform transform = GetComponent<Transform>();
            Vector3 vector3 = new(x, y, transform.position.z);
            transform.position = vector3;
        }

        public void SetPosition(float x, float y, float offsetX, float offsetY)
        {
            Transform transform = GetComponent<Transform>();
            Vector3 vector3 = new(x + offsetX, y + offsetY, transform.position.z);
            transform.position = vector3;
        }

        public void SetPosition(Vector3 position)
        {
            Transform transform = GetComponent<Transform>();
            Vector3 vector3 = new(position.x, position.y, position.z);
            transform.position = vector3;
        }

        public void SetPosition(Vector3 position, float offsetX, float offsetY)
        {
            Transform transform = GetComponent<Transform>();
            Vector3 vector3 = new(position.x + offsetX, position.y + offsetY, position.z);
            transform.position = vector3;
        }

        public virtual void RemoveDevice() => Destroy(gameObject);
    }

    public abstract class TimeLimitDevice : Device
    {
        [SerializeField] private double limitTime;

        private int _currentTime;

        private void Start()
        {
            _currentTime = limitTime <= 0 ? 0 : Convert.ToInt32(Math.Round(limitTime, 2) * 100);
            StartTimer();
        }

        private void StartTimer()
        {
            Coroutine coroutine = null;
            coroutine = StartCoroutine(PassesTimer());

            IEnumerator PassesTimer()
            {
                while (0 <= _currentTime)
                {
                    _currentTime--;
                    yield return new WaitForSeconds(0.01F);
                }

                RemoveDevice();
                StopCoroutine(coroutine);
            }
        }
    }
}