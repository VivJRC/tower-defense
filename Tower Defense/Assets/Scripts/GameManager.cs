using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;
using Enemies;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private MapManager _mapView;

    private void Start()
    {
        _enemyManager.Init();
        _mapView.Init();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        _mapView.CustomUpdate(deltaTime);
        _enemyManager.CustomUpdate(deltaTime);
    }
}
