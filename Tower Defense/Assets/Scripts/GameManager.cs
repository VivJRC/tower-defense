using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain;
using Enemies;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private MapManager _mapView;
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private Button _speedBtn;
    [SerializeField] private TextMeshProUGUI _speedBtnText;

    private float _speed;

    private void Start()
    {
        _mapView.Init();
        Cell start = _mapView.GetStart();
        List<Cell> path = _mapView.GetPath();
        _enemyManager.Init(start, path);
        _waveManager.Init();
        _speed = 1f;
        _speedBtnText.text = ">>";
        _speedBtn.onClick.AddListener(OnSpeedBtnClicked);
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
    }
}
