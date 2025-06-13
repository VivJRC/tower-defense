using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        private List<Enemy> _ennemies;

        public void Init()
        {
            _ennemies = new List<Enemy>();
        }

        public void CustomUpdate(float deltaTime)
        {
            for (int i = 0; i < _ennemies.Count; ++i)
            {
                _ennemies[i].Move(deltaTime);
            }
        }

        public void AddEnemy(E_EnemyType type)
        {

        }
    }
}
