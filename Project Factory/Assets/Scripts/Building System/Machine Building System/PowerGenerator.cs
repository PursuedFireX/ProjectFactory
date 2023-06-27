using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class PowerGenerator : MachineBase
    {

        public float powerProduced;

        public override void Start()
        {
            base.Start();
            //TimeTickSystem.OnTick += TickHandler;
        }

        public override void TickHandler(object sender, TimeTickSystem.OnTickEventArgs e)
        {
            GeneratePower();
        }


        private void GeneratePower()
        {
            if(powerNode != null)
            {
                powerNode.maxPower = 60;
                powerNode.powerTransferRate = 4.5f;
                if(powerNode.storedPower < powerNode.maxPower)
                {
                    powerNode.AddPower(powerProduced);
                }
            }
            
        }


    }
}
