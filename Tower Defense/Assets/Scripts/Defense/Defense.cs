using System.Collections.Generic;
using UnityEngine;
using MAP;
using ATK;
using System.Collections;

namespace DEF
{
    public class Defense
    {
        private DefenseModel _model;
        private DefenseView _view;
        public DefenseView View => _view;
        private List<Cell> _inZone;
        private float _attackDelay = 0.8f;
        private float _attackTimer;
        private bool _readyToAttack;
        private Cell _cell;
        public bool ReadyToAttack => _readyToAttack;
        public E_DefenseType DefenseType => _model.Type;

        public Defense(DefenseModel model, DefenseView view, Cell cell, List<Cell> inZone)
        {
            _cell = cell;
            _model = model;
            _view = view;
            _view.gameObject.SetActive(true);
            _view.SetPosition(_cell.Coordinates * 85);
            _view.InitDefense();
            _inZone = inZone;
            _readyToAttack = false;
            _attackTimer = _attackDelay;
        }

        public void CustomUpdate(float deltaTime)
        {
            _attackTimer += deltaTime;
            if (_attackTimer >= _attackDelay && !_readyToAttack)
            {
                _readyToAttack = true;
            }

            _view.CustomUpdate(deltaTime);
        }

        public void Attack(Enemy enemy)
        {
            _readyToAttack = false;
            _attackTimer = 0f;

            _view.Attack(enemy.CurrentPos * 85, _attackDelay);
        }

        private Cell GetTargetCell()
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
            return cell;
        }

        public Enemy GetTarget()
        {
            return GetTargetCell()?.Enemies[0].CurrentHealth>0 ? GetTargetCell()?.Enemies[0] : null;
        }

        public float GetDamage()
        {
            Enemy target = GetTarget();
            return (target != null) ? _model.GetDamageForEnemy(target) : 0f;
        }

        public void Kill()
        {
            _cell.RemoveDefense();
            _view.gameObject.SetActive(false);
        }
    }
}