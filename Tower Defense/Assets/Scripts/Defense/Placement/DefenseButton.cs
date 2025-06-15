using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DEF.Placement
{
    public class DefenseButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private E_DefenseType _defenseType;
        public E_DefenseType DefenseType => _defenseType;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private Transform _viewParent;

        public Action<E_DefenseType, Vector2> onBeginDrag;
        public Action<Vector2> onDrag;
        public Action<Vector2> onEndDrag;

        public void Init(int cost, DefenseView viewPrefab)
        {
            _button.onClick.RemoveAllListeners();
            _cost.text = cost.ToString();
            Instantiate(viewPrefab, _viewParent);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag.Invoke(_defenseType, eventData.position);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            onDrag.Invoke(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag.Invoke(eventData.position);
        }
    }
}