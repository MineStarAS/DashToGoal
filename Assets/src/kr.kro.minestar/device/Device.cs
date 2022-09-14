using JetBrains.Annotations;
using src.kr.kro.minestar.player;
using UnityEngine;

namespace src.kr.kro.minestar.device
{
    public abstract class Device: MonoBehaviour
    {
        [CanBeNull] public Player Player { get; protected set; }

        public void RemoveDevice() => Destroy(gameObject);
    }
}