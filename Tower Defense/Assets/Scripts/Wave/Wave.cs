using System;
using UnityEngine;

namespace WAVE
{
    [Serializable]
    public class Wave
    {
        [SerializeField] private WaveItem[] _waveItems;
        public WaveItem[] WaveItems => _waveItems;

        [SerializeField] private float _delay;
        public float Delay => _delay;
    }
}