using System;
using System.Collections.Generic;
using ATK;
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

        public List<Defense> defendingThisFrame;

        public void Init()
        {
            defendingThisFrame = new List<Defense>();
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

        public void Reset()
        {
            for (int i = _defenses.Count - 1; i >= 0; --i)
            {
                _availableViews[_defenses[i].DefenseType].Add(_defenses[i].View);
                _defenses[i].Kill();
                _defenses.Remove(_defenses[i]);
            }
            defendingThisFrame.Clear();
        }

        public void CustomUpdate(float deltaTime)
        {
            for (int i = 0; i < _defenses.Count; ++i)
            {
                _defenses[i].CustomUpdate(deltaTime);
                if (_defenses[i].ReadyToAttack)
                {
                    Enemy target = _defenses[i].GetTarget();
                    if (target != null)
                    {
                        defendingThisFrame.Add(_defenses[i]);
                    }
                }
            }
        }

        public void AddDefense(E_DefenseType type, Cell cell, List<Cell> inZone)
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

            Defense defense = new(model, view, cell, inZone);
            _defenses.Add(defense);
        }
        

    }
}