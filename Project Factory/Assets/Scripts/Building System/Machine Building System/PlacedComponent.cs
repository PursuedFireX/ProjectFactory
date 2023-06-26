using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class PlacedComponent : MonoBehaviour
    {

        public ComponentData data;

        public static PlacedComponent Create(Vector3 worldPos, ComponentData data)
        {
            Transform buildTransform = Instantiate(data.prefab, worldPos, Quaternion.identity).transform;

            PlacedComponent placedComponent = buildTransform.GetComponent<PlacedComponent>();
            placedComponent.data = data;

            return placedComponent;
        }
    }
}
