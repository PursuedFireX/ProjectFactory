using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class MachineBase : PlacedObject
    {
        public Transform[] componentSlots;
        public List<ComponentData> components;
        public PowerNode powerNode;
        public bool powered;
        public float powerConsumption;

        public float storedPower;

        public virtual void Start()
        {
            components = new List<ComponentData>();
            MachineBuildingSystem.I.OnComponentAdded += Instance_OnComponentAdded;
            TimeTickSystem.OnTick += TickHandler;
        }

        private void Instance_OnComponentAdded(object sender, System.EventArgs e)
        {
            for (int i = 0; i < componentSlots.Length; i++)
            {
                if (componentSlots[i].childCount > 0)
                {
                    if (!components.Contains(componentSlots[i].GetComponentInChildren<PlacedComponent>().data))
                    {
                        components.Add(componentSlots[i].GetComponentInChildren<PlacedComponent>().data);
                        
                        Console.SendDebug(components[i].name);
                        if (powerNode == null)
                        {
                            if (components[i].isPowerNode)
                            {
                                powerNode = componentSlots[i].GetComponentInChildren<PowerNode>();
                            }
                        }

                        componentSlots[i].GetChild(0).transform.SetParent(componentSlots[i].parent);
                    }
                }
            } 

        }

        public virtual void TickHandler(object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if(powerNode != null)
            {
                storedPower = powerNode.storedPower;
                if (storedPower > powerConsumption)
                {
                    powerNode.TakePower(powerConsumption);
                    if (storedPower > 0)
                        powered = true;
                }
            }
        }


    }
}
