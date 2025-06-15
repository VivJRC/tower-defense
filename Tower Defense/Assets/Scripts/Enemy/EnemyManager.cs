using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using MAP;

namespace ATK
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform _viewParent;
        [SerializeField] private EnemyConfig _enemyConfig;

        private List<Enemy> _enemies;
        public List<Enemy> Enemies => _enemies;
        private Dictionary<E_EnemyType, List<EnemyView>> _availableViews;
        private Cell _start;
        private List<Cell> _path;

        public List<Enemy> toKillThisFrame;

        [HideInInspector] public int reachedEndThisFrame;

        public void Init(Cell start, List<Cell> path)
        {
            _start = start;
            _path = path;
            toKillThisFrame = new List<Enemy>();
            _enemies = new List<Enemy>();
            reachedEndThisFrame = 0;

            _availableViews = new Dictionary<E_EnemyType, List<EnemyView>>();
            foreach (EnemyConfig.Data datas in _enemyConfig.Datas)
            {
                List<EnemyView> viewList = new();
                for (int i = 0; i < 10; ++i)
                {
                    EnemyView view = Instantiate(datas.view, _viewParent);
                    view.gameObject.SetActive(false);
                    viewList.Add(view);
                }
                _availableViews.Add(datas.model.Type, viewList);
            }
        }

        public void CustomUpdate(float deltaTime)
        {
            for (int i = _enemies.Count-1; i>=0; --i)
            {
                _enemies[i].Move(deltaTime);
                if (_enemies[i].ReachedEnd)
                {
                    reachedEndThisFrame++;
                    KillEnemy(_enemies[i]);
                }
            }
        }

        public void AddEnemy(E_EnemyType type)
        {
            EnemyModel model = _enemyConfig.GetModel(type);
            if (model == null)
                Debug.LogError("Couldn't find an EnemyModel of type: " + type + " in EnemyManager.");

            EnemyView view;
            if (_availableViews[type].Count > 0)
            {
                view = _availableViews[type][0];
                _availableViews[type].RemoveAt(0);
            }
            else
            {
                EnemyView viewPrefab = _enemyConfig.GetView(type);
                if (viewPrefab == null)
                    Debug.LogError("Couldn't find an EnemyView of type: " + type + " in EnemyManager.");

                view = Instantiate(viewPrefab, _viewParent);
            }

            Enemy enemy = new(model, view, _start, _path);
            _enemies.Add(enemy);
        }

        public void AddDamage(Enemy enemy, float damage)
        {
            enemy.AddDamage(damage);
            if (enemy.CurrentHealth <= 0)
            {
                toKillThisFrame.Add(enemy);
            }
        }

        public void KillEnemy(Enemy enemy)
        {
            if (!_enemies.Contains(enemy))
            {
                Debug.LogError("Trying to kill an unregistered enemy.");
            }
            _enemies.Remove(enemy);
            _availableViews[enemy.Type].Add(enemy.View);
            enemy.Kill();
        }


        #region DEBUG
        [Button]
        private void DebugAddEnemy()
        {
            AddEnemy(E_EnemyType.NONE);
        }

        [Button]
        private void DebugKillRandomEnemy()
        {
            KillEnemy(_enemies[UnityEngine.Random.Range(0, _enemies.Count)]);
        }
        #endregion
    }
}
