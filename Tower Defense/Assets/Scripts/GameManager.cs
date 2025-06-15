using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MAP;
using ATK;
using DEF;
using WAVE;
using DEF.Placement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private MapManager _mapView;
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private DefenseManager _defenseManager;
    [SerializeField] private DefensePlacementManager _defensePlacementManager;
    [SerializeField] private Button _speedBtn;
    [SerializeField] private TextMeshProUGUI _speedBtnText;
    [SerializeField] private Button _pauseBtn;
    [SerializeField] private TextMeshProUGUI _pauseBtnText;
    [SerializeField] private int _startHealth;
    private int _currentHealth;
    [SerializeField] private TextMeshProUGUI _health;
    private bool _gameOver;

    private float _speed;
    private bool _pause;

    private void Start()
    {
        _gameOver = false;

        _mapView.Init();
        Cell start = _mapView.GetStart();
        List<Cell> path = _mapView.GetPath();
        _enemyManager.Init(start, path);
        _waveManager.Init();
        _defenseManager.Init();
        _defensePlacementManager.Init(_mapView.GetMap());

        _speed = 1f;
        _speedBtnText.text = ">>";
        _speedBtn.onClick.AddListener(OnSpeedBtnClicked);
        _pauseBtn.onClick.AddListener(OnPauseBtnClicked);

        _pause = false;
        _pauseBtnText.text = "||";

        _health.text = _startHealth.ToString();
        _currentHealth = _startHealth;
    }

    private void OnDestroy()
    {
        _speedBtn.onClick.RemoveListener(OnSpeedBtnClicked);
        _pauseBtn.onClick.RemoveListener(OnPauseBtnClicked);
    }

    private void OnSpeedBtnClicked()
    {
        _speed = (_speed == 1f) ? 3f : 1f;
        _speedBtnText.text = (_speed == 1f) ? ">>" : ">";
    }

    private void OnPauseBtnClicked()
    {
        _pause = !_pause;
        _pauseBtnText.text = _pause ? "|>" : "||";
    }

    private void Update()
    {
        if (_gameOver || _pause)
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
        _defensePlacementManager.CustomUpdate(deltaTime);
        if (_defensePlacementManager.defenseToPlace != null)
        {
            _defenseManager.AddDefense
            (
                _defensePlacementManager.defenseToPlace.defenseType,
                _defensePlacementManager.defenseToPlace.coordinates
            );
            _defensePlacementManager.defenseToPlace = null;
        }

        _defenseManager.CustomUpdate(deltaTime);
    }
}
