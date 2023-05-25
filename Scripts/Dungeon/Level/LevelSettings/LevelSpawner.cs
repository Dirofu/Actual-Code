using NoCodingEasyLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour, ILocalizer
{
    [SerializeField] private List<StageSettings> _stages;
    [SerializeField] private List<LevelPreset> _presets;

    [Header("Spawner Settings")]
    [SerializeField] private int _maxCountEnemyOnScene;
    [SerializeField] private float _timeBetweenSpawn;
    [SerializeField] private List<GameObject> _spawnedEnemies;
    [SerializeField] private Transform _enemyParent;
    [SerializeField] private Character _player;
    [SerializeField] private MenuOpener _menuOpener;
    [SerializeField] private AttackEventProjectile _playerEventProjectile;
    [SerializeField] private CoinWallet _coins;

    [Header("Level Title Settings")]
    [SerializeField] private TMPro.TMP_Text _levelText;
    [SerializeField] private LocalizeField _titleLocalizator;

    private LevelPreset _activePreset;
    private PlayerMovement _playerMovement;
    private SaveSystem _saveSystem;

    public int CurrentLevel { get; private set; } = 0;
    public int CurrentStage { get; private set; } = 0;

    public void SpawnNextDungeonLevel()
    {
        CurrentLevel++;

        if (_stages[CurrentStage].LevelCount > CurrentLevel)
        {
            Destroy(_activePreset.gameObject);

            _playerEventProjectile.GetPool().DisableAllProjectiles();
            SpawnNewLevel();
            SetPlayerToStartPoint();
            StartSpawnEnemy();
        }
        else
        {
            FinishDungeon();
        }
    }

    public void ReloadLocalizationField()
    {
        SetLevelText();
    }

    public void SetSave(SaveSystem save)
    {
        _saveSystem = save;
        LoadSettings(_saveSystem.SaveData);

        SpawnNewLevel();
        SetPlayerToStartPoint();
        StartSpawnEnemy();
    }

    private void FinishDungeon()
    {
        _menuOpener.OpenFinishMenu();
        CurrentStage++;
        _saveSystem.Save();
        Debug.Log($"Passed levels: {CurrentLevel}");
        CurrentLevel = 0;
    }

    private void LoadSettings(SaveData data)
    {
        CurrentStage = data.StagesComplete;
    }

    private void SetLevelText()
    {
        _levelText.text = $"{_titleLocalizator.GetLocalizeText(this)} {CurrentLevel + 1}";
    }

    private void EndDungeonLevel()
    {
        _activePreset.OpenGate();
    }

    private void SpawnNewLevel()
    {
        _activePreset = _stages[CurrentStage].GetLevelPreset(CurrentLevel);

        if (_activePreset == null)
            _activePreset = GetRandomLevelPreset();

        SetLevelText();
        _activePreset = Instantiate(_activePreset, transform);
    }

    private LevelPreset GetRandomLevelPreset() => _presets[Random.Range(0, _presets.Count)];

    private void SetPlayerToStartPoint()
    {
        _player.transform.position = _activePreset.PlayerSpawnpoint.position;

        if (_playerMovement == null)
            _playerMovement = _player.GetComponent<PlayerMovement>();

        _playerMovement.StopDashing();
    }

    private void StartSpawnEnemy()
    {
        StartCoroutine(SpawnEnemy(_stages[CurrentStage].GetEnemisOnLevel(CurrentLevel)));
    }

    private void SetUpEnemy(EnemySettuper enemy, int level)
    {
        enemy.SetEnemyData(level);
        _spawnedEnemies.Add(enemy.gameObject);
        enemy.SetMovementTarget(_player);
        enemy.Character.Died += EnemyDied;
    }

    private void EnemyDied(Character character)
    {
        _spawnedEnemies.Remove(character.gameObject);

        var coins = _stages[CurrentStage].GetEnemyCost(character.Type, character.Level);

        _coins.AddCoins(coins);
        character.Died -= EnemyDied;
    }

    private IEnumerator SpawnEnemy(List<EnemySettingOnLevel> enemiesType)
    {
        for (int i = 0; i < enemiesType.Count; i++)
        {
            int enemyCount = enemiesType[i].Count;

            for (int j = 0; j < enemyCount; j++)
            {
                while (_spawnedEnemies.Count >= _maxCountEnemyOnScene)
                {
                    yield return null;
                }

                EnemySettingOnLevel enemySetting = enemiesType[i];

                EnemySettuper enemy = Instantiate(
                    enemySetting.EnemyPrefab,
                    _activePreset.GetRandomSpawnpointPosition(),
                    enemySetting.EnemyPrefab.transform.rotation,
                    _enemyParent);

                SetUpEnemy(enemy, enemySetting.Level - 1);
                yield return new WaitForSeconds(_timeBetweenSpawn);
            }
        }
        enemiesType.Clear();

        StartCoroutine(WaitToAllEnemiesDie());
        StopCoroutine(SpawnEnemy(enemiesType));
    }

    private IEnumerator WaitToAllEnemiesDie()
    {
        while (_spawnedEnemies.Count > 0)
            yield return new WaitForEndOfFrame();

        EndDungeonLevel();
        StopCoroutine(WaitToAllEnemiesDie());
    }
}