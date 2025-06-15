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
    [Header("Managers")]
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private DefenseManager _defenseManager;
    [SerializeField] private DefensePlacementManager _defensePlacementManager;

    [Header("Speed")]
    [SerializeField] private Button _speedBtn;
    [SerializeField] private TextMeshProUGUI _speedBtnText;

    [Header("Pause")]
    [SerializeField] private Button _pauseBtn;
    [SerializeField] private TextMeshProUGUI _pauseBtnText;

    [Header("Health")]
    [SerializeField] private int _startHealth;
    [SerializeField] private TextMeshProUGUI _health;
    private int _currentHealth;
    private int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            _health.text = value.ToString();
        }
    }

    [Header("Tokens")]
    [SerializeField] private int _startTokens;
    [SerializeField] private TextMeshProUGUI _tokens;
    private int _currentTokens;
    private int CurrentTokens
    {
        get
        {
            return _currentTokens;
        }
        set
        {
            _currentTokens = value;
            _tokens.text = value.ToString();
        }
    }
    private bool _gameOver;

    private float _speed;
    private bool _pause;

    private void Start()
    {
        _gameOver = false;

        _mapManager.Init();
        Cell start = _mapManager.GetStart();
        List<Cell> path = _mapManager.GetPath();
        _enemyManager.Init(start, path);
        _waveManager.Init();
        _defenseManager.Init();
        _defensePlacementManager.Init(_mapManager.GetMap());

        _speed = 1f;
        _speedBtnText.text = ">>";
        _speedBtn.onClick.AddListener(OnSpeedBtnClicked);
        _pauseBtn.onClick.AddListener(OnPauseBtnClicked);

        _pause = false;
        _pauseBtnText.text = "||";

        CurrentHealth = _startHealth;
        CurrentTokens = _startTokens;
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

        #region WAVE MANAGER
        _waveManager.CustomUpdate(deltaTime);
        if (_waveManager.frameSpawn.Count > 0)
        {
            for (int i = _waveManager.frameSpawn.Count - 1; i >= 0; i--)
            {
                _enemyManager.AddEnemy(_waveManager.frameSpawn[i]);
                _waveManager.frameSpawn.RemoveAt(i);
            }
        }
        #endregion

        #region ENEMY MANAGER
        _enemyManager.CustomUpdate(deltaTime);
        if (_enemyManager.reachedEndThisFrame > 0)
        {
            for (int i = 0; i < _enemyManager.reachedEndThisFrame; ++i)
            {
                CurrentHealth--;
            }
            _enemyManager.reachedEndThisFrame = 0;

            if (CurrentHealth == 0)
            {
                Debug.Log("Game Over!!!");
                _gameOver = true;
            }
        }
        if (_enemyManager.toKillThisFrame.Count > 0)
        {
            for (int i = _enemyManager.toKillThisFrame.Count - 1; i >= 0; --i)
            {
                _enemyManager.KillEnemy(_enemyManager.toKillThisFrame[i]);
                CurrentTokens += _enemyManager.toKillThisFrame[i].Drop;
                _enemyManager.toKillThisFrame.RemoveAt(i);
            }
        }
        #endregion

        #region PLACEMENT MANAGER
            _defensePlacementManager.CustomUpdate(deltaTime);
        if (_defensePlacementManager.defenseToPlace != null)
        {
            if (_defensePlacementManager.defenseToPlace.cost <= CurrentTokens)
            {
                CurrentTokens -= _defensePlacementManager.defenseToPlace.cost;
                _defenseManager.AddDefense
                (
                    _defensePlacementManager.defenseToPlace.defenseType,
                    _defensePlacementManager.defenseToPlace.cell.Coordinates
                );
                _defensePlacementManager.defenseToPlace.cell.AddDefense();
            }
            else
            {
                // add feedback not enough tokens
            }
            _defensePlacementManager.defenseToPlace = null;
        }
        #endregion

        #region DEFENSE MANAGER
        _defenseManager.CustomUpdate(deltaTime);
        #endregion
    }
}
