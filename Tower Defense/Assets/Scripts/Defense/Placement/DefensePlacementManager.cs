using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEF.Placement
{
    public class DefensePlacementManager : MonoBehaviour
    {
        [SerializeField] private DefenseConfig _defenseConfig;
        [SerializeField] private DefenseButton[] _defenseButtons;

        public void Init()
        {
            foreach (DefenseButton btn in _defenseButtons)
            {
                btn.Init(StartDrag, _defenseConfig.GetCost(btn.DefenseType), _defenseConfig.GetView(btn.DefenseType));
            }
        }

        public void StartDrag(E_DefenseType type)
        {

        }

        public void CustomUpdate(float deltaTime)
        {

        }
    }
}