using UnityEngine;

namespace Enemies
{
    public class Enemy
    {
        private EnemyModel _model;

        private int _maxHealth => _model.MaxHealth;
        private int _currentHealth;

        private Vector3 _currentPos;

        public void Init(EnemyModel model)
        {
            _currentHealth = _maxHealth;
        }

        public void Move(float deltaTime)
        {

        }

        public void Die()
        {

        }
    }
}
