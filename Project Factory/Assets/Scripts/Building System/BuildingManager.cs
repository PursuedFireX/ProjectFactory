using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager I { get; private set; }

        public event EventHandler OnSelectedChanged;

        public GameObject componentGhost;
        public GameObject buildingGhost;

        public BuildingPartData startingPart;
        [HideInInspector] public BuildingPartData currentPart;
        public ComponentData startingComponent;
        [HideInInspector] public ComponentData currentComponent;

        private void Awake()
        {
            I = this;
            currentPart = startingPart;
            currentComponent = startingComponent;
        }

        public void UpdateSelectedPart(ItemData item)
        {
            if (item.GetType() == typeof(BuildingPartData))
            {
                currentPart = (BuildingPartData)item;
            }
            else if (item.GetType() == typeof(ComponentData))
            {
                currentComponent = (ComponentData)item;
            }

            RefreshSelectedObjectType();
        }

        private void RefreshSelectedObjectType()
        {
            OnSelectedChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
