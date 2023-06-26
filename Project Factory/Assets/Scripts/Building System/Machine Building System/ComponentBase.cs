using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class ComponentBase : PlacedObject
    {
        public Transform[] componentSlots;
        public List<ComponentData> components;
        public float powerUsage;
        public bool powered;
        private bool hasPowerNode;
        public PowerNode powerNode;

        //public ResourceData[] inputResources;
        //public ResourceData output;

        private void Start()
        {
            components = new List<ComponentData>();
            MachineBuildingSystem.I.OnComponentAdded += Instance_OnComponentAdded;
        }

        private void Update()
        {
            if (GameManager.I.currentPower > powerUsage)
            {
                powered = true;
            }
        }

        private void Instance_OnComponentAdded(object sender, System.EventArgs e)
        {
            for (int i = 0; i < componentSlots.Length; i++)
            {
                if(componentSlots[i].childCount > 0)
                {
                    if(!components.Contains(componentSlots[i].GetComponentInChildren<PlacedComponent>().data))
                    {
                        components.Add(componentSlots[i].GetComponentInChildren<PlacedComponent>().data);
                        if(!hasPowerNode)
                        {
                            if(components[i].isPowerNode)
                            {
                                powerNode = componentSlots[i].GetComponentInChildren<PowerNode>();
                            }
                        }
                    }
                }
            }
            Console.SendDebug("Component added");
        }

    }
}
