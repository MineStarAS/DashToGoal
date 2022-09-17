using src.kr.kro.minestar.player;
using UnityEngine;

namespace src.kr.kro.minestar.device
{
    public class TestDevice : Device, IDeviceTimeLimit, IDeviceTimer, IDeviceRangeDetect
    {
        [SerializeField] private double limitTime;
        [SerializeField] private float periodTime;
        [SerializeField] private float detectRadius;

        double IDeviceTimeLimit.LimitTime { get => limitTime; set => limitTime = value; }

        int IDeviceTimeLimit.CurrentTime { get; set; }

        float IDeviceRangeDetect.DetectRadius { get => detectRadius; set => detectRadius = value; }

        float IDeviceTimer.PeriodTime { get => periodTime; set => periodTime = value; }

        Coroutine IDeviceTimer.Coroutine { get; set; }

        // ReSharper disable Unity.PerformanceAnalysis
        public void PeriodFunction()
        {
            Collider2D[] colliders = ((IDeviceRangeDetect)this).GetDetectedObject();

            foreach (Collider2D collider in colliders)
            {
                Player player = collider.GetComponent<Player>();
                
                if (player == null) continue;
                
                Debug.Log("GOT PLAYER!!!");
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Player player = col.GetComponent<Player>();
            player.Movement.SetMovement(player.Movement.Body.velocity.x, 20);
        }
    }
}