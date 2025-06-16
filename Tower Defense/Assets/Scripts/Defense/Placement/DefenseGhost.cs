using MAP;
using UnityEngine;

namespace DEF.Placement
{
    public class DefenseGhost : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Transform _viewParent;
        public bool IsVisible => this.gameObject.activeSelf;
        private DefenseModel _defenseModel;

        private E_DefenseType _defenseType;
        public E_DefenseType DefenseType => _defenseType;

        private int _cost;
        public int Cost => _cost;

        private int _zone;
        public int Zone => _zone;

        private RectTransform _zonePreview;

        public void Init(DefenseModel defenseModel, DefenseView viewPrefab, RectTransform zonePreview)
        {
            _cost = defenseModel.Cost;
            _zone = defenseModel.Zone;
            _defenseType = defenseModel.Type;
            Instantiate(viewPrefab, _viewParent);
            _zonePreview = zonePreview;
            _defenseModel = defenseModel;
            HideGhost();
        }

        public void UpdatePos(Vector2 pos)
        {
            _rectTransform.position = pos;
        }

        public void ShowGhost()
        {
            this.gameObject.SetActive(true);
            int side = 85 + (_defenseModel.Zone*2 * 85);
            _zonePreview.sizeDelta = new Vector2(side, side);
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