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

        public Enemy(EnemyModel model, EnemyView view, Cell start, List<Cell> path)
        {
            _model = model;
            _view = view;
            _currentHealth = _model.MaxHealth;
            _view.gameObject.SetActive(true);
            _view.InitHealth(_model.MaxHealth);
            _currentPos = start.Coordinates * 85;
            _view.InitPos(_currentPos);
            _currentCell = start;

            for (int i = 0; i < path.Count; ++i)
            {
                _path.Add(path[i]); // copy path
            }
        }

        public void Move(float deltaTime)
        {
            if ((_currentPos - _currentCell.Coordinates).sqrMagnitude < 0.1f)
            {
                _path.Remove(_currentCell);
                _currentCell = _path[0];
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
        }
    }
}
