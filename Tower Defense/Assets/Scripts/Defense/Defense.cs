using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defenses
{
    public class Defense
    {
        private DefenseModel _model;
        private DefenseView _view;

        public Defense(DefenseModel model, DefenseView view)
        {
            _model = model;
            _view = view;
        }
    }
}