using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ATK;
using MAP;
using UnityEngine;

namespace DEF.Placement
{
    public class DefensePlacementManager : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _cellsParent;
        [SerializeField] private DefenseConfig _defenseConfig;
        [SerializeField] private DefenseButton[] _defenseButtons;
        [SerializeField] private Transform _ghostParent;
        [SerializeField] private DefenseGhost _ghostPrefab;

        private List<CellView> _map;
        private Dictionary<E_DefenseType, DefenseGhost> _ghosts;
        private bool _drag;
        private DefenseGhost _currentGhost;
        private Vector2 _ghostPos;
        private CellView _targetCell;
        public DefensePlacement defenseToPlace;

        public void Init(List<CellView> map)
        {
            _map = map;
            _ghosts = new Dictionary<E_DefenseType, DefenseGhost>();
            foreach (DefenseButton btn in _defenseButtons)
            {
                DefenseModel model = _defenseConfig.GetModel(btn.DefenseType);
                DefenseView viewPrefab = _defenseConfig.GetView(btn.DefenseType);
                btn.Init(model.Cost, viewPrefab);
                btn.onBeginDrag = OnBeginDrag;
                btn.onDrag = OnDrag;
                btn.onEndDrag = OnEndDrag;

                DefenseGhost gohst = Instantiate(_ghostPrefab, _ghostParent);
                gohst.Init(model, viewPrefab);
                _ghosts.Add(btn.DefenseType, gohst);
            }
        }

        public void OnBeginDrag(E_DefenseType type, Vector2 pos)
        {
            _drag = true;
            _ghostPos = pos;
            _currentGhost = _ghosts[type];
        }

        private void OnDrag(Vector2 pos)
        {
            _ghostPos = pos;
        }

        private void OnEndDrag(Vector2 pos)
        {
            _ghostPos = pos;
            _drag = false;
        }

        public void CustomUpdate(float deltaTime)
        {
            if (!_drag)
            {
                if (_currentGhost != null)
                {
                    UpdateTargetCell(false);
                    _currentGhost.HideGhost();

                    // check if must create Defense
                    if (_targetCell != null && IsCellValid(_targetCell))
                    {
                        defenseToPlace = new DefensePlacement()
                        {
                            defenseType = _currentGhost.DefenseType,
                            coordinates = _targetCell.Cell.Coordinates
                        };
                        _targetCell.AddDefense();
                    }
                    _currentGhost = null;
                }
                return;
            }
            // else

            if (!_currentGhost.IsVisible)
            {
                _currentGhost.ShowGhost();
            }

            _currentGhost.UpdatePos(_ghostPos);
            UpdateTargetCell(true);
            _currentGhost.ShowZone(IsCellValid(_targetCell));
        }

        private void UpdateTargetCell(bool highlight)
        {
            CellView hovered = GetCellViewAtPos(_ghostPos);
            if (hovered != _targetCell)
            {
                _targetCell?.Hihlight(false);
                _targetCell = hovered;
            }
            if (_targetCell != null)
            {
                _targetCell.Hihlight(true && highlight);
            }
        }

        private bool IsCellValid(CellView cellView)
        {
            return cellView != null && cellView.Cell.CellType == E_CellType.SLOT && !cellView.HasDefense;
        }

        private CellView GetCellViewAtPos(Vector2 pos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _cellsParent,
                pos,
                _canvas.worldCamera,
                out Vector2 localPosition
            );

            Vector2 cellPos = new Vector2(localPosition.x / 85f, localPosition.y / 85f);

            for (int i = 0; i < _map.Count; ++i)
            {
                if ((_map[i].Cell.Coordinates - cellPos).sqrMagnitude <= 0.5f)
                {
                    return _map[i];
                }
            }
            return null;
        }

        public class DefensePlacement
        {
            public E_DefenseType defenseType;
            public Vector2 coordinates;
        }
    }
}