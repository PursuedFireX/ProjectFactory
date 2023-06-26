using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class Grid3D<TGridObject>
    {
        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }

        private int width;
        private int height;
        private float cellSize;
        private Vector3 gridOrigin;
        private TGridObject[,] gridArray;

        public bool showGrid;
        public bool showDebug;

        public Grid3D(int width, int height, float cellSize, Vector3 gridOrigin, Func<Grid3D<TGridObject>, int, int, TGridObject> createGridObject, bool showGrid = true, bool showDebug = false)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.gridOrigin = gridOrigin;
            this.showGrid = showGrid;
            this.showDebug = showDebug;

            gridArray = new TGridObject[width, height];


            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject(this, x, y);
                }
            }

            if (showDebug)
            {
                TextMesh[,] debugTextArray = new TextMesh[width, height];
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        debugTextArray[x, y] = Tools.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, 1, cellSize) * .5f, 5, Color.white, TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    }
                }
                OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
                {
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
                };

                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            }

            
        }

        public void ShowGrid(bool showGrid)
        {
            this.showGrid = showGrid;
        }

        public Vector3 GetGridOrigin()
        {
            return gridOrigin;
        }

        public Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0, z) * cellSize + gridOrigin;
        }

        public void GetXZ(Vector3 worldPos, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPos - gridOrigin).x / cellSize);
            z = Mathf.FloorToInt((worldPos - gridOrigin).z / cellSize);
        }

        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            }
        }

        public void TriggetGridObjectChanged(int x, int y)
        {
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }

        public void SetGridObject(Vector3 worldPos, TGridObject value)
        {
            int x, y;

            GetXZ(worldPos, out x, out y);
            SetGridObject(x, y, value);
        }

        public float GetCellSize()
        {
            return cellSize;
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
                return default(TGridObject);
        }

        public TGridObject GetGridObject(Vector3 worldPos)
        {
            int x, y;

            GetXZ(worldPos, out x, out y);
            return GetGridObject(x, y);
        }
    }
}