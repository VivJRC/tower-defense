using System.Collections.Generic;
using Enemies;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Waves _waves;
    [SerializeField] private TextMeshProUGUI _warning;

    private Wave _currentWave;
    private int _currentIndex;
    private float _waveTimer;

    private List<E_EnemyType> _spawnQueue;
    [HideInInspector] public List<E_EnemyType> frameSpawn;
    private float _spawnTimer;

    private float _spawnDelay = 1f;

    private bool _lastWave;
    public bool LastWave => _lastWave;

    public void Init()
    {
        _waveTimer = 0f;
        _spawnQueue = new List<E_EnemyType>();
        frameSpawn = new List<E_EnemyType>();
        _lastWave = false;
        _currentIndex = 0;
        _spawnTimer = 0f;
        _warning.text = "";
        _currentWave = _waves._Waves[_currentIndex];
    }

    public void CustomUpdate(float deltaTime)
    {
        _waveTimer += deltaTime;
        _spawnTimer += deltaTime;

        float remainingTime = _currentWave.Delay - _waveTimer;
        if (remainingTime <= 4f && remainingTime>0)
        {
            _warning.text = "Next wave in " + (int)remainingTime;
        }

        if (_waveTimer >= _currentWave.Delay && !_lastWave)
        {
            foreach (WaveItem waveItem in _currentWave.WaveItems)
            {
                for (int i = 0; i < waveItem.quantity; ++i)
                {
                    _spawnQueue.Add(waveItem.enemyType);
                }
            }
            _waveTimer = 0f;
            _spawnTimer = _spawnDelay;
            _currentIndex++;
            _warning.text = "";
            if (_currentIndex < _waves._Waves.Length)
            {
                _currentWave = _waves._Waves[_currentIndex];
            }
            else
            {
                _lastWave = true;
            }
        }

        if (_spawnTimer >= _spawnDelay && _spawnQueue.Count > 0)
        {
            _spawnTimer = 0f;
            frameSpawn.Add(_spawnQueue[0]);
            _spawnQueue.RemoveAt(0);
        }
    }
}
