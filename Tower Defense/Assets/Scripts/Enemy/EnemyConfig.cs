using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ATK
{
    [CreateAssetMenu]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private Data[] _datas;
        public Data[] Datas => _datas;

        public EnemyModel GetModel(E_EnemyType type)
        {
            return GetData(type).model;
        }

        public EnemyView GetView(E_EnemyType type)
        {
            return GetData(type).view;
        }

        public Data GetData(E_EnemyType type)
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
            public EnemyModel model;
            public EnemyView view;
        }
    }
}