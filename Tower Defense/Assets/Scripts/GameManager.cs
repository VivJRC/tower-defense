using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;
using Enemies;
using UnityEngine.UI;
using TMPro;
using Defenses;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private MapManager _mapView;
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private DefenseManager _defenseManager;
    [SerializeField] private Button _speedBtn;
    [SerializeField] private TextMeshProUGUI _speedBtnText;
    [SerializeField] private int _startHealth;
    private int _currentHealth;
    [SerializeField] private TextMeshProUGUI _health;
    private bool _gameOver;

    private float _speed;

    private void Start()
    {
        _gameOver = false;
        _mapView.Init();
        Cell start = _mapView.GetStart();
        List<Cell> path = _mapView.GetPath();
        _enemyManager.Init(start, path);
        _waveManager.Init();
        _defenseManager.Init();
        _speed = 1f;
        _speedBtnText.text = ">>";
        _speedBtn.onClick.AddListener(OnSpeedBtnClicked);
        _health.text = _startHealth.ToString();
        _currentHealth = _startHealth;
    }

    private void OnDestroy()
    {
        _speedBtn.onClick.RemoveListener(OnSpeedBtnClicked);
    }

    private void OnSpeedBtnClicked()
    {
        _speed = (_speed == 1f) ? 3f : 1f;
        _speedBtnText.text = (_speed == 1f) ? ">>" : ">";
    }

    private void Update()
    {
        if (_gameOver)
            return;

        float deltaTime = Time.deltaTime * _speed;
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
        if (_enemyManager.reachedEndThisFrame > 0)
        {
            for (int i = 0; i < _enemyManager.reachedEndThisFrame; ++i)
            {
                _currentHealth--;
            }
            _enemyManager.reachedEndThisFrame = 0;
            _health.text = _currentHealth.ToString();

            if (_currentHealth == 0)
            {
                Debug.Log("Game Over!!!");
                _gameOver = true;
            }
        }
        _defenseManager.CustomUpdate(deltaTime);
    }
}
