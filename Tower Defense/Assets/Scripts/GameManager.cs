using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;
using Enemies;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private MapManager _mapView;
    [SerializeField] private WaveManager _waveManager;

    private void Start()
    {
        _mapView.Init();
        Cell start = _mapView.GetStart();
        List<Cell> path = _mapView.GetPath();
        _enemyManager.Init(start, path);
        _waveManager.Init();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        _waveManager.CustomUpdate(deltaTime);
        if (_waveManager.frameSpawn.Count > 0)
        {
            for (int i = _waveManager.frameSpawn.Count - 1; i >= 0; i--)
            {
                _enemyManager.AddEnemy(_waveManager.frameSpawn[i]);
                _waveManager.frameSpawn.RemoveAt(i);
            }
        }
        _enemyManager.CustomUpdate(deltaTime);
    }
}
