using MAP;
using UnityEngine;

namespace DEF.Placement
{
    public class DefenseGhost : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _zonePreview;
        [SerializeField] private Transform _viewParent;
        public bool IsVisible => this.gameObject.activeSelf;

        private E_DefenseType _defenseType;
        public E_DefenseType DefenseType => _defenseType;

        private int _cost;
        public int Cost => _cost;

        private int _zone;
        public int Zone => _zone;

        public void Init(DefenseModel defenseModel, DefenseView viewPrefab)
        {
            _cost = defenseModel.Cost;
            _zone = defenseModel.Zone;
            _defenseType = defenseModel.Type;
            Instantiate(viewPrefab, _viewParent);
            _zonePreview.SetParent(_rectTransform.parent);
            int side = 85 + (defenseModel.Zone*2 * 85);
            _zonePreview.sizeDelta = new Vector2(side, side);
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
            _zonePreview.gameObject.SetActive(false);
        }

        public void ShowZone(bool display, Cell cell = null)
        {
            _zonePreview.gameObject.SetActive(display);
            if (cell != null)
            {
                _zonePreview.localPosition = cell.Coordinates*85;
            }
        }
    }
}