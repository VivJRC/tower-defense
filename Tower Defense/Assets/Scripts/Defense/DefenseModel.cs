using ATK;
using UnityEngine;

namespace DEF
{
    [CreateAssetMenu]
    public class DefenseModel : ScriptableObject
    {
        [SerializeField] private E_DefenseType _type;
        public E_DefenseType Type => _type;

        [SerializeField] private int _zone;
        public int Zone => _zone;

        [SerializeField] private int _cost;
        public int Cost => _cost;

        [SerializeField] private DefenseDamage[] _damages;

        public float GetDamageForEnemy(Enemy enemy)
        {
            return GetDamageForEnemy(enemy.Type);
        }

        public float GetDamageForEnemy(E_EnemyType type)
        {
            foreach (DefenseDamage defenseDamage in _damages)
            {
                if (defenseDamage.enemyType == type)
                {
                    return defenseDamage.damage;
                }
            }
            Debug.LogWarning("Couldn't find E_EnemyType: " + type + " in " + this.name + "'s _damages table.");
            return 0f;
        }
    }

}