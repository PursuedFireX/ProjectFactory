using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class ComponentGhost : MonoBehaviour
    {
        private Transform visual;
        private ComponentData component;
        [SerializeField] private Material ghostMat;
        [HideInInspector] public bool isSnapped;
        [SerializeField] private LayerMask snapLayer;
        [HideInInspector] public Transform currentSnap;
        [SerializeField] private float snapDistance;


        private void Start()
        {
            RefreshVisual();
            BuildingManager.I.OnSelectedChanged += Instance_OnSelectedChanged;
            GameManager.I.OnBuildStateChange += Instance_OnSelectedChanged;
            GameManager.I.OnGameStateChange += Instance_OnSelectedChanged;

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("SnapPoint"))
            {
                isSnapped = true;
                currentSnap = other.transform;
            }
        }


        private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
        {
            RefreshVisual();
        }


        private void LateUpdate()
        {
            if (GameManager.I.currentBuildState == BuildState.Component && GameManager.I.CurrentState == GameState.Build)
            {
                if (!isSnapped)
                {
                    Vector3 targetPosition = Mouse3D.GetMouseWorldPosition();
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
                }
                else if(isSnapped)
                {
                    Vector3 targetPosition = currentSnap.position;
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

                    if((Mouse3D.GetMouseWorldPosition() - currentSnap.position).sqrMagnitude > snapDistance * snapDistance)
                    {
                        isSnapped = false;
                        currentSnap = null;
                    }

                }
            }
        }

        private void RefreshVisual()
        {
            if (GameManager.I.currentBuildState != BuildState.Component || GameManager.I.CurrentState != GameState.Build)
            {
                if (visual != null)
                {
                    Destroy(visual.gameObject);
                    visual = null;
                }
            }
            else
            {
                if (visual != null)
                {
                    Destroy(visual.gameObject);
                    visual = null;
                }

                ComponentData currentComponent = BuildingManager.I.currentComponent;

                if (currentComponent != null)
                {
                    visual = Instantiate(currentComponent.visual, Vector3.zero, Quaternion.identity);
                    visual.parent = transform;
                    visual.localPosition = Vector3.zero;
                    visual.localEulerAngles = Vector3.zero;
                    visual.GetComponentInChildren<Renderer>().material = ghostMat;
                    SetLayerRecursive(visual.gameObject, 2);
                }
            }
        }

        private void SetLayerRecursive(GameObject targetGameObject, int layer)
        {
            targetGameObject.layer = layer;
            foreach (Transform child in targetGameObject.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
                targetGameObject.transform.localScale = Vector3.one;
            }
        }
    }
}
