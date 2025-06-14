using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _currentHealth;
        private float _maxHealth;

        public void InitHealth(float maxHealth)
        {
            _currentHealth.fillAmount = 1;
            _maxHealth = maxHealth;
        }

        public void UpdateHealth(float currentHealth)
        {
            _currentHealth.fillAmount = currentHealth / _maxHealth;
        }

        public void InitPos(Vector2 pos)
        {
            _rectTransform.localPosition = pos;
        }
    }
}