using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace MAP
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Map _map;
        [SerializeField] private Transform _cellParent;
        [SerializeField] private CellView _cellPrefab;
        private List<CellView> _cellViews;
        private List<Cell> _cells;
        private Map _currentMap;

        public void Init()
        {
            DisplayMap(_map);
        }
        
        public void DisplayMap(Map map)
        {
            _currentMap = map;
            int i;
            if (_cellViews != null && _cellViews.Count > 0)
            {
                for (i = 0; i < _cellViews.Count; ++i)
                {
                    _cellViews[i].gameObject.SetActive(false);
                }
            }

            _cellViews ??= new List<CellView>();
            _cells ??= new List<Cell>();
            i = 0;
            foreach (CellModel cellModel in map.cells)
            {
                Cell cell = new Cell(cellModel);
                _cells.Add(cell);

                CellView cellView;
                if (i >= _cellViews.Count)
                {
                    cellView = Instantiate(_cellPrefab, _cellParent);
                    _cellViews.Add(cellView);
                }
                else
                {
                    cellView = _cellViews[i];
                }
                cellView.gameObject.SetActive(true);
                cellView.transform.localPosition = new Vector3(cell.Coordinates.x * 85, cell.Coordinates.y * 85, 0);
                cellView.gameObject.name = "(" + cell.Coordinates.x + "," + cell.Coordinates.y + ")";
                cellView.SetType(cellModel);
                i++;
            }
        }

        public List<Cell> GetInZone(Vector2 coordinates, int zone)
        {
            List<Cell> path = GetPath();
            List<Cell> inZone = new();
            for (int i = 0; i < path.Count; ++i)
            {
                if(Mathf.Abs(path[i].Coordinates.x - coordinates.x) <= zone && Mathf.Abs(path[i].Coordinates.y - coordinates.y) <= zone)
                {
                    inZone.Add(path[i]);
                }
            }
            return inZone;
        }

        public List<Cell> GetMap()
        {
            return _cells;
        }
        
        public CellView GetCellViewAtCoordinates(int x, int y)
        {
            return GetCellViewAtCoordinates(new Vector2(x, y));
        }

        public CellView GetCellViewAtCoordinates(Vector2 coordinates)
        {
            foreach (CellView cellview in _cellViews)
            {
                if (cellview.Cell.Coordinates == coordinates)
                {
                    return cellview;
                }
            }
            return null;
        }

        private Cell GetCellAtCoordinates(int x, int y)
        {
            return GetCellAtCoordinates(new Vector2(x, y));
        }
        
        private Cell GetCellAtCoordinates(Vector2 coordinates)
        {
            foreach (Cell cell in _cells)
            {
                if (cell.Coordinates == coordinates)
                {
                    return cell;
                }
            }
            return null;
        }

        public List<Cell> GetPath()
        {
            List<CellModel> models = _currentMap.GetPath();
            List<Cell> cells = new();
            foreach (CellModel cellModel in models)
            {
                cells.Add(GetCellAtCoordinates(cellModel.Coordinates));
            }
            return cells;
        }

        public Cell GetStart()
        {
            return GetCellAtCoordinates(_currentMap.GetStart().Coordinates); 
        }

        #region DEBUG
        [Button]
        public void DebugGetPath()
        {
            StartCoroutine(DebugCoroutineGetPath());
        }

        private IEnumerator DebugCoroutineGetPath()
        {
            List<Cell> cells = GetPath();
            for (int i = 0; i < cells.Count; ++i)
            {
                CellView cellView = GetCellViewAtCoordinates(cells[i].Coordinates);
                yield return cellView.Flash();
            }
        }
        #endregion
    }
}