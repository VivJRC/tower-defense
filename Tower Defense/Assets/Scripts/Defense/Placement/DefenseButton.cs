using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DEF.Placement
{
    public class DefenseButton : MonoBehaviour
    {
        [SerializeField] private E_DefenseType _defenseType;
        public E_DefenseType DefenseType => _defenseType;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private Transform _viewParent;

        private Action<E_DefenseType> _callback;

        public void Init(Action<E_DefenseType> callback, int cost, DefenseView defenseView)
        {
            _button.onClick.RemoveAllListeners();
            _callback = callback;
            _button.onClick.AddListener(() => { _callback?.Invoke(_defenseType); });
            _cost.text = cost.ToString();
            DefenseView view = Instantiate(defenseView, _viewParent);
        }
    }
}