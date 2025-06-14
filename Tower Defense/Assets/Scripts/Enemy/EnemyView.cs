using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ATK
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
            _currentHealth.DOFillAmount(currentHealth / _maxHealth, 0.1f);
        }

        public void UpdatePos(Vector2 pos)
        {
            _rectTransform.localPosition = pos;
        }
        
    }
}