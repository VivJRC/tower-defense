using DG.Tweening;
using UnityEngine;

namespace DEF
{
    public class DefenseView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public void SetPosition(Vector2 pos)
        {
            _rectTransform.localPosition = pos;
        }

        public void Attack(Vector2 pos)
        {
            _rectTransform.DOLocalMove(pos, 0.3f)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}
