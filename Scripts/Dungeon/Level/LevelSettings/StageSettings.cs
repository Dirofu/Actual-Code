using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Dungeon Level/Create new StageSettings", order = 5)]
public class StageSettings : ScriptableObject
{
    [SerializeField] private List<EnemiesOnLevel> _levels;

    public int LevelCount => _levels.Count;
    public LevelPreset GetLevelPreset(int level) => _levels[level].Preset;
    public List<EnemySettingOnLevel> GetEnemisOnLevel(int level) => _levels[level].GetEnemis();
    public int GetGoldReward(int level) => _levels[level].CalculateGoldReward();
    public int GetEnemyCost(CharacterType type, int level) => _levels[level].GetEnemyCost(type, level);
}

[System.Serializable]
public class EnemiesOnLevel
{
    [Tooltip("Sets the level preset. Leave the preset empty for a random option")]
    [SerializeField] private LevelPreset _preset;
    [SerializeField] private List<EnemySettingOnLevel> _enemyTypes;

    private Dictionary<CharacterType, int> _typeCost = new Dictionary<CharacterType, int>()
    {
        { CharacterType.Swordman, 15 },
        { CharacterType.Scout, 20 },
        { CharacterType.Mage, 30 },
        { CharacterType.Cavalry, 45 },
    };

    public LevelPreset Preset => _preset;

    public int CalculateGoldReward()
    {
        int reward = 0;
        List<EnemySettingOnLevel> enemiesOnLevel = GetEnemis();

        foreach (var item in enemiesOnLevel)
        {
            item.EnemyPrefab.TryGetComponent(out Character character);

            for (int i = 0; i < item.Count; i++)
            {
                reward += GetEnemyCost(character.Type, item.Level);
            }
        }

        return reward;
    }

    public List<EnemySettingOnLevel> GetEnemis()
    {
        List<EnemySettingOnLevel> enemies = new List<EnemySettingOnLevel>();

        for (int i = 0; i < _enemyTypes.Count; i++)
            enemies.Add(_enemyTypes[i]);

        return enemies;
    }

    public int GetEnemyCost(CharacterType type, int level) => _typeCost[type] * level;
}

[System.Serializable]
public class EnemySettingOnLevel
{
    [SerializeField] private EnemySettuper _enemyPrefab;
    [SerializeField] private int _count;
    
    [Range(1,8)]
    [SerializeField] private int _level;

    public EnemySettuper EnemyPrefab => _enemyPrefab;
    public int Count => _count;
    public int Level => _level;
}