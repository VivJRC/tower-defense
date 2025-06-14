using System.Collections.Generic;
using Terrain;
using UnityEngine;

namespace Enemies
{
    public class Enemy
    {
        public E_EnemyType Type => _model.Type;
        private EnemyModel _model;
        private EnemyView _view;
        public EnemyView View => _view;


        private float _currentHealth;
        public float CurrentHealth => _currentHealth;

        private Vector2 _currentPos;
        private Cell _currentCell;
        private List<Cell> _path;

        private float _speed;
        private bool _reachedEnd;
        public bool ReachedEnd => _reachedEnd;
        private Vector2 _target;

        public Enemy(EnemyModel model, EnemyView view, Cell start, List<Cell> path)
        {
            _model = model;
            _view = view;
            _currentHealth = _model.MaxHealth;
            _view.gameObject.SetActive(true);
            _view.InitHealth(_model.MaxHealth);

            _currentPos = start.Coordinates;
            _view.UpdatePos(_currentPos * 85);
            _currentCell = start;

            _path = new List<Cell>();
            for (int i = 0; i < path.Count; ++i)
            {
                _path.Add(path[i]); // copy path
            }
            _reachedEnd = false;
            _speed = 2f;
        }

        public void Move(float deltaTime)
        {
            if ((_currentCell.Coordinates - _currentPos).sqrMagnitude < 0.01f)
            {
                _path.Remove(_currentCell);
                if (_currentCell.CellType != E_CellType.END)
                {
                    _currentCell = _path[0];
                }
                else
                {
                    _reachedEnd = true;
                }
            }
            else
            {
                _target = (_currentCell.Coordinates - _currentPos).normalized;
            }
            _currentPos += _speed * deltaTime * _target;
            _view.UpdatePos(_currentPos * 85);
        }

        public void AddDamage(float damage)
        {
            _currentHealth -= damage;
            _view.UpdateHealth(_currentHealth);
        }

        public void Kill()
        {
            _view.gameObject.SetActive(false);
        }
    }
}
