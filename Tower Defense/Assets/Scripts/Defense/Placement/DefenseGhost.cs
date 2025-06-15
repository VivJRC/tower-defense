using UnityEngine;

namespace DEF.Placement
{
    public class DefenseGhost : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _zone;
        [SerializeField] private Transform _viewParent;
        public bool IsVisible => this.gameObject.activeSelf;

        private E_DefenseType _defenseType;
        public E_DefenseType DefenseType => _defenseType;

        public void Init(DefenseModel defenseModel, DefenseView viewPrefab)
        {
            _defenseType = defenseModel.Type;
            Instantiate(viewPrefab, _viewParent);
            _zone.sizeDelta = new Vector2(defenseModel.Zone.x * 85, defenseModel.Zone.y * 85);
            HideGhost();
        }

        public void UpdatePos(Vector2 pos)
        {
            _rectTransform.position = pos;
        }

        public void ShowGhost()
        {
            this.gameObject.SetActive(true);
        }
        
        public void HideGhost()
        {
            this.gameObject.SetActive(false);
        }

        public void ShowZone(bool display)
        {
            _zone.gameObject.SetActive(display);
        }
    }
}