using System.Collections.Generic;
using UnityEngine;
using ATK;

namespace MAP
{
    public class Cell
    {
        private bool _hasDefense;
        public bool HasDefense => _hasDefense;

        private List<Enemy> _enemies;
        public List<Enemy> Enemies => _enemies;

        private CellModel _model;

        public Vector2 Coordinates => _model.Coordinates;
        public E_CellType CellType => _model.CellType;

        public Cell(CellModel model)
        {
            _model = model;
            _enemies = new List<Enemy>();
        }

        public void AddDefense()
        {
            _hasDefense = true;
        }

        public void RemoveDefense()
        {
            _hasDefense = false;
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }
    }
}
