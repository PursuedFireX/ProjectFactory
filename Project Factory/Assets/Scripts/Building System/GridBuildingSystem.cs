using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PFX
{
    public class GridBuildingSystem : MonoBehaviour
    {

        public static GridBuildingSystem Instance { get; private set; }

        public event EventHandler OnSelectedChanged;

        public List<BuildingPart> parts;
        public BuildingPart currentPart;
        private Grid3D<GridObject> grid;
        private BuildingPart.Dir dir = BuildingPart.Dir.Down;
        public bool isDragging;
        public LayerMask buildLayers;

        public int gridWidth;
        public int gridHeight;
        public float cellSize;

        public bool showGrid = false;
        public bool showGridDebug;
        private bool canDragBuild;

        public Vector3 baseGhostScale;
        public GameObject ghostObject;
        public Material ghostMat;
        public Color placeColor;
        public Color cantPlaceColor;
        public Color demolishColor;

        public GameObject gridLines;

        private GridObject startCell;
        private GridObject endCell;

        private Vector3 startPos;

        public BuildState buildState;

        private void Awake()
        {
            Instance = this;
            grid = new Grid3D<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (Grid3D<GridObject> g, int x, int z) => new GridObject(g, x, z, this), showGrid, showGridDebug);
            currentPart = parts[0];

            gridLines.transform.position = new Vector3(grid.GetGridOrigin().x, .01f, grid.GetGridOrigin().z);
            gridLines.transform.localScale = new Vector3(gridWidth / cellSize, 1, gridHeight / cellSize);
            buildState = BuildState.Build;

            baseGhostScale = ghostObject.transform.localScale;

        }

        private void Start()
        {
            GameManager.I.OnGameStateChange += Instance_OnGameStateChange;
            GameManager.I.OnBuildStateChange += Instance_OnBuildStateChange;
        }


        private void Update()
        {
            if(GameManager.I.CurrentState == GameState.Build)
            {
                if (buildState == BuildState.Build || buildState == BuildState.Destroy)
                {
                    HandleGhostMaterialColor();
                    BuildingPart.Dir prevDir = dir;

                    if (InputManager.I.LeftMouseClick())
                    {
                        if (currentPart.partType == BuildingPart.PartType.Foundation)
                        {
                            dir = BuildingPart.Dir.Down;
                        }
                        grid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);
                        if (grid.GetGridObject(x, z) != null)
                        {
                            startCell = grid.GetGridObject(x, z);
                            startPos = GetMouseWorldSnappedPosition();
                            canDragBuild = true;
                            prevDir = dir;
                        }
                        else
                        {
                            canDragBuild = false;
                            Debug.Log("Out of building bounds.");
                        }
                    }




                    if (InputManager.I.LeftMouseClick(true))
                    {
                        if (grid.GetGridObject(GetMouseWorldSnappedPosition()).GetCellPosition() != startCell.GetCellPosition())
                        {
                            isDragging = true;
                        }
                        else
                        {
                            isDragging = false;
                        }

                        if (startCell != null)
                        {
                            HandleBoxSelection();
                        }
                    }

                    if (InputManager.I.LeftMouseRelease())
                    {
                        isDragging = false;
                        ghostObject.transform.localScale = baseGhostScale;

                        grid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);
                        if (grid.GetGridObject(x, z) != null)
                        {
                            if (canDragBuild)
                            {
                                endCell = grid.GetGridObject(x, z);
                                if (buildState == BuildState.Build)
                                    PlacementHandler();

                            }
                        }
                        else
                            Debug.Log("Out of building bounds.");

                        dir = prevDir;

                        if (buildState == BuildState.Destroy)
                        {
                            RaycastHit hit;
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out hit, buildLayers))
                            {
                                PlacedObject po = hit.transform.GetComponentInParent<PlacedObject>();
                                if (po != null)
                                {
                                    Debug.Log(po);

                                    GridObject go = grid.GetGridObject(x, z);
                                    if (go != null)
                                    {
                                        if (po.GetPartType() == BuildingPart.PartType.Foundation)
                                        {
                                            Debug.Log("Floor");
                                            go.ClearPlacedObject();
                                            po.DestroySelf();
                                        }
                                        else if (po.GetPartType() == BuildingPart.PartType.Edge)
                                        {
                                            go.ClearEdgeObject(po.GetEdge());
                                            po.DestroySelf();
                                        }
                                    }

                                }
                            }
                        }

                    }

                    //Rotate
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        dir = BuildingPart.GetNextDir(dir);
                        Debug.Log(dir);
                    }

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
                
            }

            if (!showGrid)
            {
                gridLines.SetActive(false);
            }
            else
            {
                gridLines.SetActive(true);
            }
        }

        private void HandleBoxSelection()
        {
            Vector3 currentMousePos = GetMouseWorldSnappedPosition();
            Vector3 lowerLeft = new Vector3
                (
                    Mathf.Min(startPos.x, currentMousePos.x),
                    0,
                    Mathf.Min(startPos.z, currentMousePos.z)
                );
            Vector3 upperRight = new Vector3
                (
                    Mathf.Max(startPos.x, currentMousePos.x),
                    0,
                    Mathf.Max(startPos.z, currentMousePos.z)
                );

            ghostObject.transform.position = new Vector3(lowerLeft.x, 0, lowerLeft.z);
            Vector3 newScale = upperRight - lowerLeft;
            ghostObject.transform.localScale = new Vector3(newScale.x + 1, 1, newScale.z + 1);

        }

        public Vector3 GetMouseWorldSnappedPosition()
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);

            if (currentPart != null)
            {
                Vector2Int rotationOffset = currentPart.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, -1, rotationOffset.y) * grid.GetCellSize();
                return placedObjectWorldPosition;
            }
            else
            {
                return mousePosition;
            }
        }

        public Quaternion GetPlacedObjectRotation()
        {
            if (currentPart != null)
            {
                return Quaternion.Euler(0, currentPart.GetRotationAngle(dir), 0);
            }
            else
            {
                return Quaternion.identity;
            }
        }

        public BuildingPart GetCurrentPart()
        {
            return currentPart;
        }

        private void PlacementHandler()
        {
            if (startCell.GetCellPosition().x > endCell.GetCellPosition().x && startCell.GetCellPosition().y > endCell.GetCellPosition().y)
            {
                for (int xx = startCell.GetCellPosition().x; xx >= endCell.GetCellPosition().x; xx--)
                {
                    for (int zz = startCell.GetCellPosition().y; zz >= endCell.GetCellPosition().y; zz--)
                    {
                        grid.GetXZ(grid.GetWorldPosition(xx, zz), out int xPos, out int zPos);
                        if (grid.GetGridObject(xPos, zPos) != null)
                        {
                            PlaceObject(xPos, zPos, grid.GetGridObject(xPos, zPos));
                        }
                    }
                }
            }
            else if (startCell.GetCellPosition().x > endCell.GetCellPosition().x)
            {
                for (int xx = startCell.GetCellPosition().x; xx >= endCell.GetCellPosition().x; xx--)
                {
                    for (int zz = startCell.GetCellPosition().y; zz <= endCell.GetCellPosition().y; zz++)
                    {
                        grid.GetXZ(grid.GetWorldPosition(xx, zz), out int xPos, out int zPos);
                        if (grid.GetGridObject(xPos, zPos) != null)
                        {
                            PlaceObject(xPos, zPos, grid.GetGridObject(xPos, zPos));
                        }
                    }
                }
            }
            else if (startCell.GetCellPosition().y > endCell.GetCellPosition().y)
            {
                for (int xx = startCell.GetCellPosition().x; xx <= endCell.GetCellPosition().x; xx++)
                {
                    for (int zz = startCell.GetCellPosition().y; zz >= endCell.GetCellPosition().y; zz--)
                    {
                        grid.GetXZ(grid.GetWorldPosition(xx, zz), out int xPos, out int zPos);
                        if (grid.GetGridObject(xPos, zPos) != null)
                        {
                            PlaceObject(xPos, zPos, grid.GetGridObject(xPos, zPos));
                        }
                    }
                }
            }
            else
            {
                for (int xx = startCell.GetCellPosition().x; xx <= endCell.GetCellPosition().x; xx++)
                {
                    for (int zz = startCell.GetCellPosition().y; zz <= endCell.GetCellPosition().y; zz++)
                    {
                        grid.GetXZ(grid.GetWorldPosition(xx, zz), out int xPos, out int zPos);
                        if (grid.GetGridObject(xPos, zPos) != null)
                        {
                            PlaceObject(xPos, zPos, grid.GetGridObject(xPos, zPos));
                        }
                    }
                }
            }
        }

        private void DemolishHandler()
        {
            #region Mass Destruction
            if (startCell.GetCellPosition().x > endCell.GetCellPosition().x && startCell.GetCellPosition().y > endCell.GetCellPosition().y)
            {
                for (int xx = startCell.GetCellPosition().x; xx >= endCell.GetCellPosition().x; xx--)
                {
                    for (int zz = startCell.GetCellPosition().y; zz >= endCell.GetCellPosition().y; zz--)
                    {
                        grid.GetXZ(grid.GetWorldPosition(xx, zz), out int xPos, out int zPos);
                        if (grid.GetGridObject(xPos, zPos) != null)
                        {
                            DemolishObject(grid.GetGridObject(xPos, zPos));
                        }
                    }
                }
            }
            else if (startCell.GetCellPosition().x > endCell.GetCellPosition().x)
            {
                for (int xx = startCell.GetCellPosition().x; xx >= endCell.GetCellPosition().x; xx--)
                {
                    for (int zz = startCell.GetCellPosition().y; zz <= endCell.GetCellPosition().y; zz++)
                    {
                        grid.GetXZ(grid.GetWorldPosition(xx, zz), out int xPos, out int zPos);
                        if (grid.GetGridObject(xPos, zPos) != null)
                        {
                            DemolishObject(grid.GetGridObject(xPos, zPos));
                        }
                    }
                }
            }
            else if (startCell.GetCellPosition().y > endCell.GetCellPosition().y)
            {
                for (int xx = startCell.GetCellPosition().x; xx <= endCell.GetCellPosition().x; xx++)
                {
                    for (int zz = startCell.GetCellPosition().y; zz >= endCell.GetCellPosition().y; zz--)
                    {
                        grid.GetXZ(grid.GetWorldPosition(xx, zz), out int xPos, out int zPos);
                        if (grid.GetGridObject(xPos, zPos) != null)
                        {
                            DemolishObject(grid.GetGridObject(xPos, zPos));
                        }
                    }
                }
            }
            else
            {
                for (int xx = startCell.GetCellPosition().x; xx <= endCell.GetCellPosition().x; xx++)
                {
                    for (int zz = startCell.GetCellPosition().y; zz <= endCell.GetCellPosition().y; zz++)
                    {
                        grid.GetXZ(grid.GetWorldPosition(xx, zz), out int xPos, out int zPos);
                        if (grid.GetGridObject(xPos, zPos) != null)
                        {
                            DemolishObject(grid.GetGridObject(xPos, zPos));
                        }
                    }
                }
            }
            #endregion



        }

        private void PlaceObject(int x, int z, GridObject go)
        {

            List<Vector2Int> gridPositionList = currentPart.GetGridPositionList(new Vector2Int(x, z), dir);

            bool canBuild = true;
            #region CanBuild check
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (grid.GetGridObject(gridPosition.x, gridPosition.y) != null)
                {
                    if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuid())
                    {
                        canBuild = false;
                        break;
                    }
                }
                else
                {
                    Debug.Log("Not enough space.");
                }
            }
            #endregion

            if (canBuild)
            {
                Vector2Int rotationOffset = currentPart.GetRotationOffset(dir);
                Vector3 placePosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();


                PlacedObject po = PlacedObject.Create(placePosition, new Vector2Int(x, z), dir, currentPart);
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(po);
                }
            }
            else
            {
                Debug.Log("Can't build here.");
            }
            
        }


        private void DemolishObject(GridObject gridObject)
        {
            if (gridObject != null)
            {
                PlacedObject placedObject = gridObject.GetPlacedObject();

                if (placedObject != null)
                {
                    placedObject.DestroySelf();
                    List<Vector2Int> gridPositionList = placedObject.gridPositions();

                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }
        }

        public BuildingPart.Dir GetDir()
        {
            return dir;
        }

        private void RefreshSelectedObjectType()
        {
            OnSelectedChanged?.Invoke(this, EventArgs.Empty);
        }

        private void HandleGhostMaterialColor()
        {
            grid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);
            GridObject currentCell = grid.GetGridObject(x, z);
            if (currentCell != null)
            {
                if (buildState == BuildState.Build)
                {
                    if (currentPart.partType == BuildingPart.PartType.Foundation)
                    {
                        if (currentCell.GetPlacedObject() == null)
                            ghostMat.color = placeColor;
                        else
                            ghostMat.color = cantPlaceColor;
                    }
                }
                else if(buildState == BuildState.Destroy)
                {
                    ghostMat.color = demolishColor;
                }
            }
        }

        private void Instance_OnGameStateChange(object sender, System.EventArgs e)
        {
            if (GameManager.I.CurrentState == GameState.Build)
            {
                showGrid = true;
            }
            else if (GameManager.I.CurrentState == GameState.Typing || GameManager.I.CurrentState == GameState.Paused)
            {
                if (showGrid)
                    showGrid = true;
                else if (!showGrid)
                    showGrid = false;
            }
            else
            {
                showGrid = false;
            }
        }

        private void Instance_OnBuildStateChange(object sender, System.EventArgs e)
        {
            if (GameManager.I.currentBuildState == BuildState.Component)
                showGrid = false;
            else
                showGrid = true;

            buildState = GameManager.I.currentBuildState;
        }

        [System.Serializable]
        public class GridObject
        {
            private Grid3D<GridObject> grid;
            private int x;
            private int z;
            private PlacedObject placedObject;
            private PlacedObject placedComponent;

            private PlacedObject topSlot;
            private PlacedObject bottomSlot;
            private PlacedObject leftSlot;
            private PlacedObject rightSlot;

            private GridBuildingSystem gbs;

            public GameObject ghostObj;

            public GridObject(Grid3D<GridObject> grid, int x, int z, GridBuildingSystem gbs)
            {
                this.grid = grid;
                this.x = x;
                this.z = z;
                this.gbs = gbs;
            }

            public Vector2Int GetCellPosition()
            {
                return new Vector2Int(x, z);
            }

            public void SetPlacedObject(PlacedObject p)
            {
                this.placedObject = p;
                grid.TriggetGridObjectChanged(x, z);
            }

            public void SetEdgeObject(PlacedObject edgeObject, Edge edge )
            {
                switch(edge)
                {
                    default:
                    case Edge.Top:
                        topSlot = edgeObject;
                        break;
                    case Edge.Bottom:
                        bottomSlot = edgeObject;
                        break;
                    case Edge.Left:
                        leftSlot = edgeObject;
                        break;
                    case Edge.Right:
                        rightSlot = edgeObject;
                        break;
                }
            }

            public void SetComponentObject(PlacedObject p)
            {
                this.placedComponent = p;
            }


            public PlacedObject GetPlacedObject()
            {

                switch(GridBuildingSystem.Instance.currentPart.partType)
                {
                    case BuildingPart.PartType.Foundation: return placedObject;

                    case BuildingPart.PartType.Edge:
                        switch (gbs.dir)
                        {
                            case BuildingPart.Dir.Up:
                                return topSlot;
                            case BuildingPart.Dir.Down:
                                return bottomSlot;
                            case BuildingPart.Dir.Left:
                                return leftSlot;
                            case BuildingPart.Dir.Right:
                                return rightSlot;
                        }
                        break;

                    case BuildingPart.PartType.Component: return placedComponent;
                }


                return null;
            }

            public PlacedObject GetEdgeObject(Edge edge)
            {
                switch (edge)
                {
                    default:
                    case Edge.Top:
                        return topSlot;
                        
                    case Edge.Bottom:
                        return bottomSlot;
                        
                    case Edge.Left:
                        return leftSlot;
                        
                    case Edge.Right:
                        return rightSlot;
                        
                }
            }

            public void ClearEdgeObject(Edge edge)
            {
                switch (edge)
                {
                    default:
                    case Edge.Top:
                        topSlot = null;
                        break;
                    case Edge.Bottom:
                        bottomSlot = null;
                        break;
                    case Edge.Left:
                        leftSlot = null;
                        break;
                    case Edge.Right:
                        rightSlot = null;
                        break;
                }
            }

            public void ClearPlacedObject()
            {
                placedObject = null;
                grid.TriggetGridObjectChanged(x, z);
            }

            public void ClearComponentObject()
            {
                placedComponent = null;
            }

            public void ClearAllPlacedObjects()
            {
                placedObject = null;
                placedComponent = null;
                topSlot = null;
                bottomSlot = null;
                leftSlot = null;
                rightSlot = null;
            }

            public bool CanBuid()
            {
                switch (GridBuildingSystem.Instance.currentPart.partType)
                {
                    case BuildingPart.PartType.Foundation: return placedObject == null;

                    case BuildingPart.PartType.Edge:
                        switch (gbs.dir)
                        {
                            case BuildingPart.Dir.Up:
                                return topSlot == null;
                            case BuildingPart.Dir.Down:
                                return bottomSlot == null;
                            case BuildingPart.Dir.Left:
                                return leftSlot == null;
                            case BuildingPart.Dir.Right:
                                return rightSlot == null;
                        }
                        break;

                    case BuildingPart.PartType.Component: return placedComponent == null;
                }


                return false;
            }
            
        }

        public enum Edge
        {
            Top,
            Bottom,
            Left,
            Right
        }
    }
}