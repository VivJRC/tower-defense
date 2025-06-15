using System.Collections.Generic;
using UnityEngine;
using MAP;

namespace DEF
{
    public class Defense
    {
        private DefenseModel _model;
        private DefenseView _view;
        private List<Cell> _inZone;

        public Defense(DefenseModel model, DefenseView view, Vector2 coordinates, List<Cell> inZone)
        {
            _model = model;
            _view = view;
            _view.gameObject.SetActive(true);
            _view.SetPosition(coordinates * 85);
            _inZone = inZone;
        }
    }
}