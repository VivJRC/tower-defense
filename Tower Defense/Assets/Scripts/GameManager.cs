using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;
using Enemies;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Map _map;

    private void Start()
    {
        _enemyManager.Init();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        _enemyManager.CustomUpdate(deltaTime);
    }
}
