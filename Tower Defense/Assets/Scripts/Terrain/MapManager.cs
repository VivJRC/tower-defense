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

        public List<Cell> GetMap()
        {
            return _cells;
        }
        
        public CellView GetCellAtCoordinates(int x, int y)
        {
            return GetCellAtCoordinates(new Vector2(x, y));
        }

        public CellView GetCellAtCoordinates(Vector2 coordinates)
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

        public List<CellModel> GetPath()
        {
            return _currentMap.GetPath();
        }

        public CellModel GetStart()
        {
            return  _currentMap.GetStart();
        }

        #region DEBUG
        [Button]
        public void DebugGetPath()
        {
            StartCoroutine(DebugCoroutineGetPath());
        }

        private IEnumerator DebugCoroutineGetPath()
        {
            List<CellModel> cells = GetPath();
            for (int i = 0; i < cells.Count; ++i)
            {
                CellView cellView = GetCellAtCoordinates(cells[i].Coordinates);
                yield return cellView.Flash();
            }
        }
        #endregion
    }
}