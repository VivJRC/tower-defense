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
        private CellView _currentCell;
        private List<CellView> _path;

        private float _speed;
        public bool reachedEnd;
        private Vector2 _target;

        public Enemy(EnemyModel model, EnemyView view, CellView start, List<CellView> path)
        {
            _model = model;
            _view = view;
            _currentHealth = _model.MaxHealth;
            _view.gameObject.SetActive(true);
            _view.InitHealth(_model.MaxHealth);

            _currentPos = start.Cell.Coordinates;
            _view.UpdatePos(_currentPos * 85);
            _currentCell = start;

            _path = new List<CellView>();
            for (int i = 0; i < path.Count; ++i)
            {
                _path.Add(path[i]); // copy path
            }
            reachedEnd = false;
            _speed = 2f;
        }

        public void Move(float deltaTime)
        {
            if ((_currentCell.Cell.Coordinates - _currentPos).sqrMagnitude < 0.01f)
            {
                _path.Remove(_currentCell);
                if (_currentCell.Cell.CellType != E_CellType.END)
                {
                    _currentCell = _path[0];
                }
                else
                {
                    reachedEnd = true;
                }
            }
            else
            {
                _target = (_currentCell.Cell.Coordinates - _currentPos).normalized;
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
