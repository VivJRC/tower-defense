using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Terrain
{
    public class MapEditor : MonoBehaviour
    {
        [SerializeField] private CellEditor _cellPrefab;
        [SerializeField] private Transform _cellParent;
        [SerializeField] private Vector2 _size;
        [SerializeField] private string _mapName;
        [SerializeField] private Map _map;

        [Button]
        private void CreateCells()
        {
            Clean();

            for (int x = 0; x < _size.x; ++x)
            {
                for (int y = 0; y < _size.y; ++y)
                {
                    CellEditor cell = Instantiate(_cellPrefab, _cellParent);
                    cell.transform.position = new Vector3(x * 50 + 50, y * 50 + 50, 0);
                    cell.gameObject.name = "(" + x + "," + y + ")";
                    cell.x = x;
                    cell.y = y;
                }
            }
        }

        [Button]
        private void CreateMap()
        {
            CellEditor[] currentCells = _cellParent.GetComponentsInChildren<CellEditor>();
            Map map = ScriptableObject.CreateInstance<Map>();

            int maxX = 0;
            int maxY = 0;

            // Find the maximum coordinates
            foreach (CellEditor cellEditor in currentCells)
            {
                if (cellEditor.x > maxX)
                {
                    maxX = cellEditor.x;
                }
                if (cellEditor.y > maxY)
                {
                    maxY = cellEditor.y;
                }
            }

            map.SetSize(maxX + 1, maxY + 1);

            // Set each cell's type using its individual coordinates
            foreach (CellEditor cellEditor in currentCells)
            {
                map.SetCellType(cellEditor.x + 1, cellEditor.y + 1, cellEditor.cellType);
            }

            string path = "Assets/Data/Map";
            string assetPath = Path.Combine(path, (string.IsNullOrEmpty(_mapName) ? "Map" : _mapName) + ".asset");
            AssetDatabase.CreateAsset(map, assetPath);
            EditorUtility.SetDirty(map);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [Button]
        private void ViewMap()
        {
            Clean();
            Vector2 size = _map.Size;

            for (int x = 0; x < size.x; ++x)
            {
                for (int y = 0; y < size.y; ++y)
                {
                    CellEditor cell = Instantiate(_cellPrefab, _cellParent);
                    cell.transform.position = new Vector3(x * 50 + 50, y * 50 + 50, 0);
                    cell.gameObject.name = "(" + x + "," + y + ")";

                    Cell mapCell = _map.GetCell(x + 1, y + 1);
                    cell.cellType = mapCell?.cellType ?? E_CellType.DEFAULT;
                }
            }
        }

        private void Clean()
        {
            CellEditor[] previousCells = _cellParent.GetComponentsInChildren<CellEditor>();
            foreach (CellEditor cell in previousCells)
            {
                DestroyImmediate(cell.gameObject);
            }
        }
    }
}