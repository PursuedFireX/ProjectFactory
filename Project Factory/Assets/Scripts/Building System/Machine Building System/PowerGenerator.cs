using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class PowerGenerator : PlacedObject
    {
        private void Start()
        {
            TimeTickSystem.OnTick += GeneratePower;
        }

        private void GeneratePower(object sender, TimeTickSystem.OnTickEventArgs e)
        {
            Console.SendDebug("Tick: " + TimeTickSystem.GetTick());
            GameManager.I.currentPower += 30;
        }
    }
}
