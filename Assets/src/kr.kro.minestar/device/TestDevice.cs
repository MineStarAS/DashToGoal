using src.kr.kro.minestar.player;
using System;
using UnityEngine;

namespace src.kr.kro.minestar.device
{
    public class TestDevice: Device
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Player player = col.GetComponent<Player>();
            player.TestFunction("TestDevice");
            player.Movement.SetMovement(player.Movement.Body.velocity.x, 20);
            RemoveDevice();
        }
    }
}