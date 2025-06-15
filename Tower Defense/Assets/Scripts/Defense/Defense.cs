using UnityEngine;

namespace DEF
{
    public class Defense
    {
        private DefenseModel _model;
        private DefenseView _view;

        public Defense(DefenseModel model, DefenseView view, Vector2 coordinates)
        {
            _model = model;
            _view = view;
            _view.gameObject.SetActive(true);
            _view.SetPosition(coordinates * 85);
        }
    }
}