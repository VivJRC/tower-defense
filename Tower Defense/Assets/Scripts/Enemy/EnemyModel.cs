using UnityEngine;

namespace ATK
{
    [CreateAssetMenu()]
    public class EnemyModel : ScriptableObject
    {
        [SerializeField] private float _health;
        public float MaxHealth => _health;

        [SerializeField] private E_EnemyType _type;
        public E_EnemyType Type => _type;

        [SerializeField] private int _drop;
        public int Drop => _drop;
    }
}