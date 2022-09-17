using JetBrains.Annotations;
using src.kr.kro.minestar.player;
using System;
using System.Collections;
using UnityEngine;

namespace src.kr.kro.minestar.device
{
    public abstract class Device : MonoBehaviour
    {
        [CanBeNull] public Player Player { get; protected set; }

        private void Start()
        {
            (this as IDeviceTimeLimit)?.Init();
            (this as IDeviceRangeDetect)?.Init();
            (this as IDeviceTimer)?.Init();
        }

        public static Device SummonDevice<T>(Vector3 vector3)
        {
            string deviceName = typeof(T).Name;
            GameObject gameObject = Resources.Load<GameObject>($"Device/{deviceName}") 
                                    ?? throw new NullReferenceException($"Cannot find {deviceName}.");
            
            Device device = gameObject.GetComponent<Device>()
            ?? throw new NullReferenceException($"{deviceName} is not device.");

            Instantiate(device, vector3, Quaternion.identity);
            return device;
        }

        public void RemoveDevice()
        {
            Destroy(gameObject);
            StopAllCoroutines();
        }
    }

    internal interface IDeviceFunction
    {
        public void Init()
        {
        }

        protected Device GetDevice() => this as Device ?? throw new InvalidCastException($"{GetType().Name} is not Device.");

        public static int ConvertTime(double time) => time <= 0 ? 0 : Convert.ToInt32(Math.Round(time, 2) * 100);
    }

    internal interface IDeviceTimeLimit : IDeviceFunction
    {
        protected double LimitTime { get; set; }

        protected int CurrentTime { get; set; }

        public new void Init()
        {
            CurrentTime = ConvertTime(LimitTime);
            StartTimer();
        }

        private void StartTimer()
        {
            Device device = GetDevice();
            Coroutine coroutine = null;
            coroutine = device.StartCoroutine(PassesTimer());

            IEnumerator PassesTimer()
            {
                while (0 <= CurrentTime)
                {
                    CurrentTime--;
                    yield return new WaitForSeconds(0.01F);
                }

                device.RemoveDevice();
                device.StopCoroutine(coroutine);
            }
        }
    }

    internal interface IDeviceTimer : IDeviceFunction
    {
        protected float PeriodTime { get; set; }
        protected Coroutine Coroutine { get; set; }

        public new void Init()
        {
            StartTimer();
        }

        private void StartTimer()
        {
            if (PeriodTime <= 0) PeriodTime = 0.01F;
            
            Device device = GetDevice();
            Coroutine = device.StartCoroutine(PassesTimer());

            IEnumerator PassesTimer()
            {
                while (true)
                {
                    PeriodFunction();
                    yield return new WaitForSeconds(PeriodTime);
                }
            }
        }

        public void StopTimer()
        {
            Device device = GetDevice();
            device.StopCoroutine(Coroutine);
        }

        public void PeriodFunction();
    }

    internal interface IDeviceRangeDetect : IDeviceFunction
    {
        protected float DetectRadius { get; set; }

        public Collider2D[] GetDetectedObject()
        {
            Vector3 position = GetDevice().transform.position;
            return Physics2D.OverlapCircleAll(new Vector2(position.x, position.y), DetectRadius);
        }
    }
}