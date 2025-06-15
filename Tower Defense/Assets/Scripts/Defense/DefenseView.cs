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
    }
}
