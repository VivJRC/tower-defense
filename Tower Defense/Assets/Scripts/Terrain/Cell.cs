using UnityEngine;

namespace MAP
{
    public class Cell
    {
        private bool _hasDefense;
        public bool HasDefense => _hasDefense;
        private CellModel _model;

        public Vector2 Coordinates => _model.Coordinates;
        public E_CellType CellType => _model.CellType;

        public Cell(CellModel model)
        {
            _model = model;
        }

        public void AddDefense()
        {
            _hasDefense = true;
        }

        public void RemoveDefense()
        {
            _hasDefense = false;
        }
    }
}
