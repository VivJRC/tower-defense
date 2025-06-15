using System;
using System.Collections.Generic;
using MAP;
using UnityEngine;

namespace DEF
{
    public class DefenseManager : MonoBehaviour
    {
        [SerializeField] private DefenseConfig _defenseConfig;
        [SerializeField] private Transform _viewParent;
        
        private List<Defense> _defenses;
        private Dictionary<E_DefenseType, List<DefenseView>> _availableViews;

        public void Init()
        {
            _defenses = new List<Defense>();

            _availableViews = new Dictionary<E_DefenseType, List<DefenseView>>();
            foreach (DefenseConfig.Data datas in _defenseConfig.Datas)
            {
                List<DefenseView> viewList = new();
                for (int i = 0; i < 10; ++i)
                {
                    DefenseView view = Instantiate(datas.view, _viewParent);
                    view.gameObject.SetActive(false);
                    viewList.Add(view);
                }
                _availableViews.Add(datas.model.Type, viewList);
            }
        }

        public void CustomUpdate(float deltaTime)
        {
            for (int i = 0; i < _defenses.Count; ++i)
            {

            }
        }

        public void AddDefense(E_DefenseType type, Vector2 coordinates, List<Cell> inZone)
        {
            DefenseModel model = _defenseConfig.GetModel(type);
            if (model == null)
                Debug.LogError("Couldn't find an DefenseModel of type: " + type + " in DefenseManager.");

            DefenseView view;
            if (_availableViews[type].Count > 0)
            {
                view = _availableViews[type][0];
                _availableViews[type].RemoveAt(0);
            }
            else
            {
                DefenseView viewPrefab = _defenseConfig.GetView(type);
                if (viewPrefab == null)
                    Debug.LogError("Couldn't find an DefenseView of type: " + type + " in DefenseManager.");

                view = Instantiate(viewPrefab, _viewParent);
            }

            Defense defense = new(model, view, coordinates, inZone);
            _defenses.Add(defense);
        }
    }
}