using System.Collections;
using System.Collections.Generic;
using ATK;
using UnityEngine;

namespace DEF.Placement
{
    public class DefensePlacementManager : MonoBehaviour
    {
        [SerializeField] private DefenseConfig _defenseConfig;
        [SerializeField] private DefenseButton[] _defenseButtons;
        [SerializeField] private Transform _ghostParent;
        [SerializeField] private DefenseGhost _ghostPrefab;

        private Dictionary<E_DefenseType, DefenseGhost> _ghosts;
        private bool _drag;
        private DefenseGhost _currentGhost;
        private Vector2 _ghostPos;

        public void Init()
        {
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
                    _currentGhost.HideGhost();
                    _currentGhost = null;

                    // check if must create Defense
                }
                return;
            }
            // else

            if (!_currentGhost.IsVisible)
            {
                _currentGhost.ShowGhost();
            }

            _currentGhost.UpdatePos(_ghostPos);
            // check if must show zone
        }
    }
}