using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    [CreateAssetMenu(menuName = ("Data/Component Data"))]
    public class ComponentData : ItemData
    {

        public Transform prefab;
        public Transform visual;
        public bool isPowerNode;

        public float powerConsumption;

    }
}
