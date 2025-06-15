using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MAP;
using ATK;
using DEF;
using WAVE;
using DEF.Placement;
using DG.Tweening;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private DefenseManager _defenseManager;
    [SerializeField] private DefensePlacementManager _defensePlacementManager;

    [Header("End")]
    [SerializeField] private Button _replay;
    [SerializeField] private TextMeshProUGUI _endText;
    [SerializeField] private CanvasGroup _endScreen;

    [Header("Speed")]
    [SerializeField] private Button _speedBtn;
    [SerializeField] private TextMeshProUGUI _speedBtnText;

    [Header("Pause")]
    [SerializeField] private Button _pauseBtn;
    [SerializeField] private TextMeshProUGUI _pauseBtnText;
    [SerializeField] private GameObject _pauseScreen;

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

        _speedBtn.onClick.AddListener(OnSpeedBtnClicked);
        _pauseBtn.onClick.AddListener(OnPauseBtnClicked);

        Reset();
    }

    private void Reset()
    {
        _gameOver = false;
        
        _replay.onClick.RemoveListener(Reset);

        _endScreen.alpha = 0;
        _endScreen.interactable = false;
        _endScreen.blocksRaycasts = false;

        _speed = 1f;
        _speedBtnText.text = ">>";

        _pause = false;
        _pauseScreen.SetActive(false);
        _pauseBtnText.text = "||";

        CurrentHealth = _startHealth;
        CurrentTokens = _startTokens;

        _enemyManager.Reset();
        _waveManager.Reset();
        _defenseManager.Reset();
        _defensePlacementManager.Reset();
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
        _pauseScreen.SetActive(_pause);
        _pauseBtnText.text = _pause ? "|>" : "||";
    }

    public IEnumerator ShowEndScreen(bool won)
    {
        yield return new WaitForSeconds(1);
        _endScreen.interactable = true;
        _endScreen.blocksRaycasts = true;
        _endScreen.DOFade(1f, 0.3f);
        _replay.onClick.AddListener(Reset);
        _endText.text = won ? "You won!" : "You lost!";
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
        if (_waveManager.LastWave && _waveManager.SpawnQueueCount == 0 && _enemyManager.Enemies.Count == 0)
        {
            _gameOver = true;
            StartCoroutine(ShowEndScreen(true));
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
                _gameOver = true;
                StartCoroutine(ShowEndScreen(false));
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
            int cost = _defensePlacementManager.defenseToPlace.cost;
            if (cost <= CurrentTokens)
            {
                E_DefenseType type = _defensePlacementManager.defenseToPlace.defenseType;
                Cell cell = _defensePlacementManager.defenseToPlace.cell;
                int zone = _defensePlacementManager.defenseToPlace.zone;
                CurrentTokens -= cost;
                List<Cell> inZone = _mapManager.GetInZone(cell.Coordinates, zone);
                // // DEBUG
                // for (int i = 0; i < inZone.Count; ++i)
                // {
                //     _mapManager.GetCellViewAtCoordinates(inZone[i].Coordinates).StartFlash();
                // }
                _defenseManager.AddDefense(type, cell, inZone);
                _defensePlacementManager.defenseToPlace.cell.AddDefense();
            }
            else
            {
                _tokens.DOColor(Color.red, 0.3f).OnComplete(() => { _tokens.DOColor(Color.white, 0.3f); });
            }
            _defensePlacementManager.defenseToPlace = null;
        }
        #endregion

        #region DEFENSE MANAGER
        _defenseManager.CustomUpdate(deltaTime);
        if (_defenseManager.defendingThisFrame.Count > 0)
        {
            for (int i = _defenseManager.defendingThisFrame.Count - 1; i >= 0; --i)
            {
                Defense defense = _defenseManager.defendingThisFrame[i];
                Enemy target = defense.GetTarget();
                float damage = defense.GetDamage();

                defense.Attack(target);
                _enemyManager.AddDamage(target, damage);
                _defenseManager.defendingThisFrame.RemoveAt(i);
            }
        }
        #endregion
    }
}
