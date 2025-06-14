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

        private Vector3 _currentPos;

        public Enemy(EnemyModel model, EnemyView view)
        {
            _model = model;
            _view = view;
            _currentHealth = _model.MaxHealth;
            _view.gameObject.SetActive(true);
            _view.InitHealth(_model.MaxHealth);
        }

        public void Move(float deltaTime)
        {

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
