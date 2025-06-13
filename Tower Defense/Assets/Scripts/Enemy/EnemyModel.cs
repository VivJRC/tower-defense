using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu()]
    public class EnemyModel : ScriptableObject
    {
        [SerializeField] private int _health;
        public int MaxHealth => _health;

        [SerializeField] private E_EnemyType _type;
        public E_EnemyType Type => _type;

        [SerializeField] private int _drop;
        public int Drop => _drop;
    }
}