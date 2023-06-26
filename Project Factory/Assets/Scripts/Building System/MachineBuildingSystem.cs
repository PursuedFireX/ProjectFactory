using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class MachineBuildingSystem : MonoBehaviour
    {
        public static MachineBuildingSystem I { get; private set; }

        private BuildingManager buildingManager;
        private bool canBuild;
        private ComponentGhost ghost;

        public EventHandler OnComponentAdded;

        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
            buildingManager = BuildingManager.I;
            ghost = buildingManager.componentGhost.GetComponent<ComponentGhost>();
        }

        private void Update()
        {
            if (ghost.isSnapped)
            {
                if (ghost.currentSnap.childCount == 0)
                {
                    canBuild = true;
                }
                else canBuild = false;
            }
            else
            {
                canBuild = false;
            }


            if(GameManager.I.CurrentState == GameState.Build && GameManager.I.currentBuildState == BuildState.Component)
            {

                if (InputManager.I.LeftMouseRelease())
                {
                    if (canBuild)
                    {
                        PlaceComponent();
                    }
                }
            }
        }

        public void PlaceComponent()
        {

            PlacedComponent pc = PlacedComponent.Create(ghost.currentSnap.position, buildingManager.currentComponent);
            pc.transform.SetParent(ghost.currentSnap);
            CheckForPlacedComponent();
        }

        private void CheckForPlacedComponent()
        {
            OnComponentAdded?.Invoke(this, EventArgs.Empty);
        }
    }
}
