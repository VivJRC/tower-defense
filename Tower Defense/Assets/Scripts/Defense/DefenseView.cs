using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DEF
{
    public class DefenseView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button _button;
        [SerializeField] private Image _raycast;

        private Vector3 _attackPos;
        private Vector3 _basePos;
        private bool _isAttacking = true;
        private float _attackDuration;
        private bool _goingForward;
        private float _attackTimer;

        public void SetPosition(Vector2 pos)
        {
            _rectTransform.localPosition = pos;
            _basePos = pos;
        }

        public void InitDefense()
        {
            _raycast.raycastTarget = true;
        }

        public void Attack(Vector2 pos, float duration)
        {
            _attackPos = pos;
            _isAttacking = true;
            _attackTimer = 0f;
            _goingForward = true;
            _attackDuration = duration;
        }

        public void CustomUpdate(float deltaTime)
        {
            if (!_isAttacking)
                return;

            _attackTimer += deltaTime;
            float t = _attackTimer / (_attackDuration*0.5f);
            t = Mathf.Clamp01(t);

            if (_goingForward)
            {
                _rectTransform.localPosition = Vector3.Lerp(_basePos, _attackPos, t);
            }
            else
            {
                _rectTransform.localPosition = Vector3.Lerp(_attackPos, _basePos, t);
            }

            if (t >= 1f)
            {
                _attackTimer = 0f;
                if (_goingForward)
                {
                    _goingForward = false;
                }
                else
                {
                    _isAttacking = false;
                }
            }
        }
    }
}
