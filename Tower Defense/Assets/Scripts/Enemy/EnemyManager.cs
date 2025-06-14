using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Terrain;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform _viewParent;
        [SerializeField] private EnemyDatas[] _datas;

        private List<Enemy> _enemies;
        private Dictionary<E_EnemyType, List<EnemyView>> _availableViews;
        private Cell _start;
        private List<Cell> _path;

        public int reachedEndThisFrame;

        public void Init(Cell start, List<Cell> path)
        {
            _start = start;
            _path = path;
            _enemies = new List<Enemy>();
            reachedEndThisFrame = 0;

            _availableViews = new Dictionary<E_EnemyType, List<EnemyView>>();
            foreach (EnemyDatas datas in _datas)
            {
                List<EnemyView> viewList = new();
                for (int i = 0; i < 10; ++i)
                {
                    EnemyView view = Instantiate(datas.view, _viewParent);
                    view.gameObject.SetActive(false);
                    viewList.Add(view);
                }
                _availableViews.Add(datas.type, viewList);
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
            EnemyModel model = GetModel(type);
            if (model == null)
                Debug.LogError("Couldn't find an EnemyModel of type: " + type);

            EnemyView view;
            if (_availableViews[type].Count > 0)
            {
                view = _availableViews[type][0];
                _availableViews[type].RemoveAt(0);
            }
            else
            {
                EnemyView viewPrefab = GetView(type);
                if (viewPrefab == null)
                    Debug.LogError("Couldn't find an EnemyView of type: " + type);

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
                KillEnemy(enemy);
            }
        }

        private void KillEnemy(Enemy enemy)
        {
            if (!_enemies.Contains(enemy))
            {
                Debug.LogError("Trying to kill an unregistered enemy.");
            }
            _enemies.Remove(enemy);
            _availableViews[enemy.Type].Add(enemy.View);
            enemy.Kill();
        }

        #region HELPERS
        private EnemyModel GetModel(E_EnemyType type)
        {
            return GetData(type).model;
        }

        private EnemyView GetView(E_EnemyType type)
        {
            return GetData(type).view;
        }

        private EnemyDatas GetData(E_EnemyType type)
        {
            foreach (EnemyDatas data in _datas)
            {
                if (data.type == type)
                {
                    return data;
                }
            }
            return null;
        }

        [Serializable]
        private class EnemyDatas
        {
            public E_EnemyType type;
            public EnemyModel model;
            public EnemyView view;
        }
        #endregion

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
