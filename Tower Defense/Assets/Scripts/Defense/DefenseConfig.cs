using UnityEngine;
using System;

namespace DEF
{
    [CreateAssetMenu]
    public class DefenseConfig : ScriptableObject
    {
        [SerializeField] private Data[] _datas;
        public Data[] Datas => _datas;

        public DefenseModel GetModel(E_DefenseType type)
        {
            return GetData(type).model;
        }

        public DefenseView GetView(E_DefenseType type)
        {
            return GetData(type).view;
        }

        public int GetCost(E_DefenseType type)
        {
            return GetModel(type).Cost;
        }

        public Data GetData(E_DefenseType type)
        {
            foreach (Data data in _datas)
            {
                if (data.model.Type == type)
                {
                    return data;
                }
            }
            return null;
        }

        [Serializable]
        public class Data
        {
            public DefenseModel model;
            public DefenseView view;
        }
    }
}