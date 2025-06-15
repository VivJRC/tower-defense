using UnityEngine;

namespace MAP
{
    [System.Serializable]
    public class CellModel
    {
        [SerializeField] private E_CellType cellType;
        public E_CellType CellType => cellType;

        [SerializeField] private Vector2 coordinates;
        public Vector2 Coordinates => coordinates;

        #if UNITY_EDITOR
        public void SetCellType(E_CellType type)
        {
            cellType = type;
        }
        public void SetCoordinates(Vector2 coord)
        {
            coordinates = coord;
        }
        #endif
    }
}