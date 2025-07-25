using System;
using System.Collections.Generic;
using MAP;
using UnityEngine;

namespace ATK
{
    public class Enemy
    {
        public E_EnemyType Type => _model.Type;
        private EnemyModel _model;
        private EnemyView _view;
        public EnemyView View => _view;

        public int Drop => _model.Drop;

        private float _currentHealth;
        public float CurrentHealth => _currentHealth;

        private Vector2 _currentPos;
        public Vector2 CurrentPos => _currentPos;
        private Cell _previousTarget;
        private Cell _currentTarget;
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
            _currentTarget = start;

            _path = new List<Cell>();
            for (int i = 0; i < path.Count; ++i)
            {
                _path.Add(path[i]); // copy path
            }
            _reachedEnd = false;
            _speed = 2f;
        }

        public void CustomUpdate(float deltaTime)
        {
            if ((_currentTarget.Coordinates - _currentPos).sqrMagnitude < 0.01f)
            {
                _previousTarget = _currentTarget;
                _path.Remove(_currentTarget);
                if (_currentTarget.CellType != E_CellType.END)
                {
                    _currentTarget = _path[0];
                }
                else
                {
                    _reachedEnd = true;
                }
            }
            else
            {
                _target = (_currentTarget.Coordinates - _currentPos).normalized;
            }
            _currentPos += _speed * deltaTime * _target;
            _view.UpdatePos(_currentPos * 85);

            UpdateCurrentCell();
        }

        private void UpdateCurrentCell()
        {
            float minDistance = Mathf.Infinity;
            Cell newCurrent = null;

            float distance;
            for (int i = 0; i < _path.Count; ++i)
            {
                distance = (_path[i].Coordinates - _currentPos).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    newCurrent = _path[i];
                }
            }
            distance = (_previousTarget.Coordinates - _currentPos).sqrMagnitude;
            if (distance < minDistance)
            {
                newCurrent = _previousTarget;
            }

            if (newCurrent != _currentCell)
            {
                _currentCell?.RemoveEnemy(this);
                newCurrent?.AddEnemy(this);
                _currentCell = newCurrent;
            }
        }

        public void AddDamage(float damage)
        {
            _currentHealth -= damage;
            _view.UpdateHealth(_currentHealth);
        }

        public void Kill()
        {
            _view.gameObject.SetActive(false);
            _currentCell?.RemoveEnemy(this);
        }

        public static implicit operator Enemy(Cell v)
        {
            throw new NotImplementedException();
        }
    }
}
