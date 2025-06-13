using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Terrain
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Map _map;
        [SerializeField] private Transform _cellParent;
        [SerializeField] private CellView _cellPrefab;
        private List<CellView> _cellViews;
        private Map _currentMap;

        public void Init()
        {
            DisplayMap(_map);
        }

        public void CustomUpdate(float deltaTime)
        {

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
            i = 0;
            foreach (Cell cell in map.cells)
            {
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
                cellView.transform.localPosition = new Vector3(cell.coordinates.x * 85, cell.coordinates.y * 85, 0);
                cellView.SetType(cell);
                i++;
            }
        }

        public CellView GetCellAtCoordinates(int x, int y)
        {
            return GetCellAtCoordinates(new Vector2(x, y));
        }

        public CellView GetCellAtCoordinates(Vector2 coordinates)
        {
            foreach (CellView cellview in _cellViews)
            {
                if (cellview.Cell.coordinates == coordinates)
                {
                    return cellview;
                }
            }
            return null;
        }

        public List<Cell> GetPath()
        {
            return _currentMap.GetPath();
        }


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
                CellView cellview = GetCellAtCoordinates(cells[i].coordinates);
                yield return cellview.Flash();
            }
        }
    }
}