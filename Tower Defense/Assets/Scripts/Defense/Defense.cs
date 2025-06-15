using System.Collections.Generic;
using UnityEngine;
using MAP;
using ATK;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

namespace DEF
{
    public class Defense
    {
        private DefenseModel _model;
        private DefenseView _view;
        private List<Cell> _inZone;
        private float _attackDelay;
        private bool _readyToAttack;
        public bool ReadyToAttack => _readyToAttack;

        public Defense(DefenseModel model, DefenseView view, Vector2 coordinates, List<Cell> inZone)
        {
            _model = model;
            _view = view;
            _view.gameObject.SetActive(true);
            _view.SetPosition(coordinates * 85);
            _inZone = inZone;
            _readyToAttack = false;
        }

        public void CustomUpdate(float deltaTime)
        {
            _attackDelay += deltaTime;
            if (_attackDelay >= 1f && !_readyToAttack)
            {
                _readyToAttack = true;
            }
        }

        public void Attack()
        {
            _readyToAttack = false;
            _attackDelay = 0f;
        }

        public Enemy GetTarget()
        {
            Cell cell = null;
            for (int i = _inZone.Count - 1; i >= 0; i--) // closest to the end
            {
                if (_inZone[i].Enemies.Count > 0)
                {
                    cell = _inZone[i];
                    break;
                }
            }
            return cell?.Enemies[0];
        }

        public float GetDamage()
        {
            Enemy target = GetTarget();
            return (target != null) ? _model.GetDamageForEnemy(target) : 0f;
        }
    }
}