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

        public void SetPosition(Vector2 pos)
        {
            _rectTransform.localPosition = pos;
        }

        public void InitDefense()
        {
            _raycast.raycastTarget = true;
        }

        public void Attack(Vector2 pos)
        {
            _rectTransform.DOLocalMove(pos, 0.3f)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}
