using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class BuildingGhost : MonoBehaviour
    {
        private Transform visual;
        private BuildingPart part;
        [SerializeField] private Material ghostMat;


        private void Start()
        {
            RefreshVisual();
            GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
            GameManager.I.OnBuildStateChange += Instance_OnBuildStateChange;

        }


        private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
        {
            RefreshVisual();
        }

        private void Instance_OnBuildStateChange(object sender, System.EventArgs e)
        {
            RefreshVisual();
        }

        private void LateUpdate()
        {

            if (!GridBuildingSystem.Instance.isDragging)
            {
                Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
                targetPosition.y = 0f;
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

                transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
            }
            
        }

        private void RefreshVisual()
        {
            if (GameManager.I.currentBuildState == BuildState.Component)
            {
                Destroy(visual.gameObject);
                visual = null;
            }
            else
            {
                if (visual != null)
                {
                    Destroy(visual.gameObject);
                    visual = null;
                }

                BuildingPart currentPart = GridBuildingSystem.Instance.GetCurrentPart();

                if (currentPart != null)
                {
                    visual = Instantiate(currentPart.visual, Vector3.zero, Quaternion.identity);
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
