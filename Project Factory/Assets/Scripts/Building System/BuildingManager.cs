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



        public List<BuildingPartData> parts;
        [HideInInspector] public BuildingPartData currentPart;

        public List<ComponentData> components;
        [HideInInspector] public ComponentData currentComponent;

        private void Awake()
        {
            I = this;
            currentPart = parts[0];
            currentComponent = components[0];
        }

        private void Update()
        {
            //Change Part
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RefreshSelectedObjectType();
                currentPart = parts[0];
                RefreshSelectedObjectType();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentPart = parts[1];
                RefreshSelectedObjectType();
            }
        }


        private void RefreshSelectedObjectType()
        {
            OnSelectedChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
