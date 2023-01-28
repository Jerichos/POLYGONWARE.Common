using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace POLYGONWARE.Common
{
    [RequireComponent(typeof(BoxCollider))]
    public class Grid : MonoBehaviour
    {
        public static readonly float CellSize = 1;
        
        [SerializeField] private int _width = 3;
        [SerializeField] private int _height = 3;
        [SerializeField] private bool _showGrid;
        [SerializeField] private CellVisual _cellVisualPrefab;
        [SerializeField] private Transform _parent;

        // this gives headaches, maybe better to put cells to a list
        [SerializeField] private Cell[] Cells;

        public static Cell SelectedCell { get; set; }

        [SerializeField] private BoxCollider _boxCollider;

        private void Awake()
        {
            InitializeGrid();
        }

        public bool ShowGrid
        {
            set
            {
                _showGrid = value;
                if (_showGrid)
                {
                    foreach (var cell in Cells)
                    {
                        cell.Visualisation.Show();
                    }
                }
                else
                {
                    foreach (var cell in Cells)
                    {
                        cell.Visualisation.Hide();
                    }
                }
            }
        }
        
        public void InitializeGrid()
        {
            // clear cells
            if(Cells != null && Cells.Length > 0)
                foreach (var cell in Cells)
                {
                    DestroyImmediate(cell.GameObject);
                }
            
            _boxCollider.size = new Vector3(_width, 0.1f, _height);
            _boxCollider.center = new Vector3(_width / 2f, 0, _height / 2f);
            Cells = new Cell[_width *_height];
            for (int x = 0; x < _width; x++)
            {
               for (int z = 0; z < _height; z++)
               {
                   var position = _parent.position;
                   position += CellSize * new Vector3(x, 0.05f, z);
                   Cells[x * _height + z] = new Cell(position, _cellVisualPrefab, _parent);
               }
            }
        }

        public virtual void SelectCell(int x, int z)
        {
            var cell = Cells[x * _height + z];
            cell.Select();
        }

        public static Vector3 WorldToCellPosition(Vector3 worldPosition)
        {
            worldPosition.x = GetPoint(worldPosition.x);
            worldPosition.z = GetPoint(worldPosition.z);
            return worldPosition;
        }
        
        public static float GetPoint(float axis)
        {
            return Mathf.Floor(axis / Grid.CellSize) * Grid.CellSize;
        }

        public static int GetCellPoint(float axis)
        {
            var point = axis / CellSize;
            // point /= 10;
            return Mathf.FloorToInt(point)/* * 10*/;
        }
        
        public Cell GetCell(Vector3 worldPosition)
        {
            worldPosition = transform.InverseTransformPoint(worldPosition);
            int x = GetCellPoint(worldPosition.x);
            int z = GetCellPoint(worldPosition.z);

            int i = x * _height + z;

            if (i >= Cells.Length)
            {
                return null;
            }
            return Cells[i];
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(!Application.isPlaying)
                return;
            
            ShowGrid = _showGrid;
        }
#endif
        
    }

    [Serializable]
    public class Cell
    {
        public GameObject Occupying;
        public Vector3 Position;
        public CellVisual Visualisation;
        public GameObject GameObject;

        public Cell(Vector3 position, CellVisual cellVisualPrefab, Transform parent)
        {
            var newCell = GameObject.Instantiate(cellVisualPrefab, position, Quaternion.identity);
            GameObject = newCell.gameObject;
            newCell.transform.SetParent(parent);
            Visualisation = newCell;
            Position = position;
        }

        public void Select()
        {
            Grid.SelectedCell?.Deselect();
            Grid.SelectedCell = this;
            OnSelect();
        }

        public void Deselect()
        {
            Grid.SelectedCell = null;
            OnDeselect();
        }

        public void OnSelect()
        {
            Visualisation.SetValid(true);
        }

        public void OnDeselect()
        {
            Visualisation.SetValid(false);
        }

        public void Add(GameObject gameObject)
        {
            if(Occupying)
                Object.Destroy(Occupying);
            
            Occupying = gameObject;
            Occupying.transform.position = Position + new Vector3(Grid.CellSize / 2f, 0, Grid.CellSize / 2f);
        }

        public bool CanAdd()
        {
            return !Occupying;
        }
    }
}