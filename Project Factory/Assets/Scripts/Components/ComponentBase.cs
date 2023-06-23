using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class ComponentBase : PlacedObject
    {
        public Transform[] componentSlots;
        public ComponentData[] components;
        public float powerUsage;
        public bool powered;

        public ResourceData[] inputResources;
        public ResourceData output;

        private void Update()
        {
            if (GameManager.I.currentPower > powerUsage)
            {
                powered = true;
            }
        }
    }
}
