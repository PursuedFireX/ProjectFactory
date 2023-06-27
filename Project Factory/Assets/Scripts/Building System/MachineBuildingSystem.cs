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

        public PowerNode currentSelectedNode;
        private bool isConnecting;
        private LineRenderer lr;

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
            if (GameManager.I.CurrentState == GameState.Free)
            {
                if (InputManager.I.LeftMouseRelease())
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.GetComponent<PowerNode>() != null)
                        {

                            if (!isConnecting)
                            {
                                currentSelectedNode = hit.transform.GetComponent<PowerNode>();
                                lr = currentSelectedNode.transform.gameObject.AddComponent<LineRenderer>();
                                lr.startWidth = .1f;
                                lr.endWidth = .1f;
                                lr.material = AssetManager.I.powerLineMat;
                                lr.SetPosition(0, currentSelectedNode.transform.position);
                                lr.SetPosition(1, currentSelectedNode.transform.position);
                                isConnecting = true;
                            }
                            else if(isConnecting)
                            {
                                currentSelectedNode.connectedNodes.Add(hit.transform.GetComponent<PowerNode>());
                                hit.transform.GetComponent<PowerNode>().connectedNodes.Add(currentSelectedNode);
                                lr.SetPosition(1, hit.transform.position);
                                currentSelectedNode = null;
                                isConnecting = false;
                            }
                        }
                        else
                        {
                            
                        }

                    }
                }

                if (InputManager.I.RightMouseRelease())
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.GetComponent<PowerNode>() != null)
                        {
                            Console.SendDebug(hit.transform.GetComponent<PowerNode>().storedPower.ToString());
                        }
                        else
                        {

                        }

                    }
                }

            }

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
