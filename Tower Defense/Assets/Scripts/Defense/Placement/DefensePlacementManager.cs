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
            _currentGhost = _ghosts[type];
            _currentGhost.ShowGhost();
            _currentGhost.HideZone();
            _currentGhost.UpdatePos(pos);
        }

        private void OnDrag(Vector2 pos)
        {
            _currentGhost.UpdatePos(pos);
            
        }

        private void OnEndDrag(Vector2 pos)
        {
            _currentGhost.UpdatePos(pos);
            _currentGhost.HideGhost();
            _currentGhost = null;
        }
    }
}