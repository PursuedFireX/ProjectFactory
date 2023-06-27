using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class PowerNode : PlacedComponent
    {
        public float maxPower;
        public float storedPower;
        public float powerTransferRate;

        public bool connected;
        public List<PowerNode> connectedNodes;

        private void Start()
        {
            connectedNodes = new List<PowerNode>();
            TimeTickSystem.OnTick += HandleTick;
        }

        public void AddPower(float amount)
        {
            storedPower += amount;

            if (storedPower > maxPower)
                storedPower = maxPower;
        }

        public void TakePower(float amount)
        {
            storedPower -= amount;

            if (storedPower <= 0)
                storedPower = 0;
        }

        public void TakePower(float amount, out float power)
        {
            storedPower -= amount;
            power = amount;

            if (storedPower <= 0)
                storedPower = 0;

            
        }

        private void HandleTick(object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if (connectedNodes.Count > 0)
            {
                SendPower();
            }
        }

        private void SendPower()
        {

            foreach (PowerNode node in connectedNodes)
            {
                if (node.storedPower < storedPower)
                {
                    if (node.storedPower < node.maxPower)
                    {
                        node.AddPower(powerTransferRate);
                        TakePower(powerTransferRate);
                    }
                }
            }

        }
    }
}
