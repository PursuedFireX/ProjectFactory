using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class PlacedObject : MonoBehaviour
    {
        public static PlacedObject Create(Vector3 worldPos, Vector2Int origin, BuildingPartData.Dir dir, BuildingPartData type)
        {
            Transform buildTransform = Instantiate(type.prefab, worldPos, Quaternion.Euler(0, type.GetRotationAngle(dir), 0)).transform;

            PlacedObject placedObject = buildTransform.GetComponent<PlacedObject>();
            placedObject.type = type;
            placedObject.origin = origin;
            placedObject.dir = dir;

            return placedObject;
        }

        public static PlacedObject Create(Vector3 worldPos, Vector2Int origin, BuildingPartData.Dir dir, BuildingPartData type, GridBuildingSystem.Edge edge)
        {
            Transform buildTransform = Instantiate(type.prefab, worldPos, Quaternion.Euler(0, type.GetRotationAngle(dir), 0)).transform;

            PlacedObject placedObject = buildTransform.GetComponent<PlacedObject>();
            placedObject.type = type;
            placedObject.origin = origin;
            placedObject.dir = dir;
            placedObject.edge = edge;

            return placedObject;
        }

        private BuildingPartData type;
        private Vector2Int origin;
        private BuildingPartData.Dir dir;
        private GridBuildingSystem.Edge edge;

        public List<Vector2Int> gridPositions()
        {
            return type.GetGridPositionList(origin, dir);
        }

        public BuildingPartData.PartType GetPartType()
        {
            return type.partType;
        }

        public GridBuildingSystem.Edge GetEdge()
        {
            return edge;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}