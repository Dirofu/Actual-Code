using Agava.YandexGames;
using NoCodingEasyLocalization;
using System;
using System.Collections;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private LocalizeMaster _localizeMaster = null;

    private SaveData _saveData;
    private PlayerSkillsData _skillsData;
    private AbilityButtonsData _abilityButtonsData;
    private ResourcesStorage _resourcesStorage;
    private SoundSettuper _sound;
    private CoinWallet _wallet;
    private LevelSpawner _spawner;
    private string _json;

    public SaveData SaveData => _saveData;

    private void Awake()
    {
        _saveData = new SaveData();
#if !UNITY_EDITOR
        Load();
#else
        SetStandardSave();
        OnLevelWasLoaded(0);
#endif
    }

    private void OnLevelWasLoaded(int _)
    {
        FindLevelSpawner();
        FindCoinWallet();
        FindSkillsData();
        FindSettingsData();
        FindButtonsData();
        FindResources();
    }

    public void Save()
    {
        GetResources();
        GetSettings();
        GetPlayerSkills();
        GetStageCompleteData();
        GetButtonsAcquired();
        GetEarnedGold();

#if !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized == true)
        {
            _json = JsonUtility.ToJson(_saveData);
            Debug.Log($"Save json: {_json}");
            PlayerAccount.SetCloudSaveData(_json);
        }
#endif
        SavePrefs();
    }

    public void Load()
    {
        if (PlayerAccount.IsAuthorized == true)
            PlayerAccount.GetCloudSaveData(SetLoadedCloudSaveData);
        else
            SetLoadedPrefsSaveData();
    }

    public void SaveCoinsFromWalletOnEndDungeon()
    {
        if (_wallet == null)
            return;

        _saveData.GoldCount += _wallet.Coins;
        Save();
    }

    private void GetResources()
    {
        var village = FindObjectOfType<Village>();

        if (village == null)
            return;

        _saveData.GoldCount = village.GetResourceAmount(ResourceType.Gold);
        _saveData.WoodCount = village.GetResourceAmount(ResourceType.Wood);
        _saveData.RockCount = village.GetResourceAmount(ResourceType.Stone);
    }

    private void GetSettings()
    {
        if (_sound == null)
            return;

        _saveData.MusicEnable = _sound.MusicEnable;
        _saveData.EffectsEnable = _sound.EffectsEnable;
        _saveData.VolumeValue = _sound.Volume;
        _saveData.Language = (int)_localizeMaster.GetSelectedLang();
    }

    private void GetPlayerSkills()
    {
        if (_skillsData == null)
            return;

        _saveData.HealthLevel = _skillsData.HealthLevel;
        _saveData.DamageLevel = _skillsData.DamageLevel;
        _saveData.MoveSpeedLevel = _skillsData.MoveSpeedLevel;
        _saveData.AttackRangeLevel = _skillsData.AttackRangeLevel;
        _saveData.AttackSpeedLevel = _skillsData.AttackSpeedLevel;
        _saveData.RebounceProjectileLevel = _skillsData.RebounceProjectileCount;
        _saveData.DoubleAttack = _skillsData.DoubleAttackActive;
        _saveData.ThroughArrow = _skillsData.ThroughArrow;
        _saveData.ExploseArrow = _skillsData.ExploseArrow;
        _saveData.ExploseArrowDamageLevel = _skillsData.ExploseArrowDamage;
        _saveData.ExploseArrowRangeLevel = _skillsData.ExploseArrowRange;
    }

    private void GetEarnedGold()
    {
        if (_wallet == null)
            return;

        _saveData.GoldEarned += _wallet.Coins;
    }

    private void GetStageCompleteData()
    {
        if (_spawner == null)
            return;

        _saveData.StagesComplete = _spawner.CurrentStage;
    }

    private void GetButtonsAcquired()
    {
        if (_abilityButtonsData == null)
            return;

        _saveData.Damage0Button = _abilityButtonsData.Damage0Button.IsAcquired;
        _saveData.Damage1Button = _abilityButtonsData.Damage1Button.IsAcquired;
    }

    private void SetLoadedCloudSaveData(string value)
    {
        _saveData = JsonUtility.FromJson<SaveData>(value);

        if (_saveData.NotFirstPlaying == false)
            SetStandardSave();
    }

    private void SetLoadedPrefsSaveData()
    {
        GetPrefsSave();

        if (_saveData.NotFirstPlaying == false)
            SetStandardSave();
    }

    private void FindLevelSpawner()
    {
        _spawner = FindObjectOfType<LevelSpawner>();

        if (_spawner != null)
            _spawner.SetSave(this);
    }

    private void FindCoinWallet()
    {
        _wallet = FindObjectOfType<CoinWallet>();

        if (_wallet != null)
            _wallet.SetSave(this);
    }

    private void FindSettingsData()
    {
        _sound = FindObjectOfType<SoundSettuper>();

        if (_sound != null)
            _sound.SetSave(this);
    }

    private void FindSkillsData()
    {
        _skillsData = FindObjectOfType<PlayerSkillsData>();

        if (_skillsData != null)
            _skillsData.SetSave(this);
    }

    private void FindButtonsData()
    {
        _abilityButtonsData = FindObjectOfType<AbilityButtonsData>();
        
        if (_abilityButtonsData != null)
            _abilityButtonsData.SetSave(this);
    }

    private void FindResources()
    {
        _resourcesStorage = FindObjectOfType<ResourcesStorage>();

        if (_resourcesStorage != null)
            _resourcesStorage.SetSave(this);
    }

    private void SetStandardSave()
    {
        PlayerPrefs.SetInt(Constantes.NotFirstPlaying, 1);
        _saveData.NotFirstPlaying = true;

        _saveData.GoldCount = 100;
        _saveData.WoodCount = 100;
        _saveData.RockCount = 100;

        _saveData.StagesComplete = 0;
        _saveData.GoldEarned = 0;

        _saveData.HealthLevel = 1;
        _saveData.DamageLevel = 1;
        _saveData.MoveSpeedLevel = 1;
        _saveData.AttackRangeLevel = 1;
        _saveData.AttackSpeedLevel = 1;
        _saveData.RebounceProjectileLevel = 0;
        _saveData.DoubleAttack = false;
        _saveData.ThroughArrow = false;
        _saveData.ExploseArrow = false;
        _saveData.ExploseArrowDamageLevel = 1;
        _saveData.ExploseArrowRangeLevel = 1;

        _saveData.MusicEnable = true;
        _saveData.EffectsEnable = true;
        _saveData.VolumeValue = -20f;
        _saveData.Language = 30;

#if !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized == true)
        {
            _json = JsonUtility.ToJson(_saveData);
            PlayerAccount.SetCloudSaveData(_json);
        }
#endif
        SavePrefs();
    }

    private void GetPrefsSave()
    {
        _saveData.NotFirstPlaying = PlayerPrefs.GetInt(Constantes.NotFirstPlaying) == 1;

        _saveData.GoldCount = PlayerPrefs.GetInt(Constantes.GoldCount);
        _saveData.WoodCount = PlayerPrefs.GetInt(Constantes.WoodCount);
        _saveData.RockCount = PlayerPrefs.GetInt(Constantes.RockCount);

        _saveData.StagesComplete = PlayerPrefs.GetInt(Constantes.StagesComplete);
        _saveData.GoldEarned = PlayerPrefs.GetInt(Constantes.GoldEarned);

        _saveData.HealthLevel = PlayerPrefs.GetInt(Constantes.HealthLevel);
        _saveData.DamageLevel = PlayerPrefs.GetInt(Constantes.DamageLevel);
        _saveData.MoveSpeedLevel = PlayerPrefs.GetInt(Constantes.MoveSpeedLevel);
        _saveData.AttackRangeLevel = PlayerPrefs.GetInt(Constantes.AttackRangeLevel);
        _saveData.AttackSpeedLevel = PlayerPrefs.GetInt(Constantes.AttackSpeedLevel);
        _saveData.RebounceProjectileLevel = PlayerPrefs.GetInt(Constantes.RebounceProjectileLevel);
        _saveData.DoubleAttack = PlayerPrefs.GetInt(Constantes.DoubleAttack) == 1;
        _saveData.ThroughArrow = PlayerPrefs.GetInt(Constantes.ThroughArrow) == 1;
        _saveData.ExploseArrow = PlayerPrefs.GetInt(Constantes.ExploseArrow) == 1;

        if (_saveData.ExploseArrow == true)
        {
            _saveData.ExploseArrowDamageLevel = PlayerPrefs.GetInt(Constantes.ExploseArrowDamageLevel);
            _saveData.ExploseArrowRangeLevel = PlayerPrefs.GetInt(Constantes.ExploseArrowRangeLevel);
        }

        _saveData.MusicEnable = PlayerPrefs.GetInt(Constantes.MusicEnable) == 1;
        _saveData.EffectsEnable = PlayerPrefs.GetInt(Constantes.EffectsEnable) == 1;
        _saveData.VolumeValue = -PlayerPrefs.GetInt(Constantes.VolumeValue);
        _saveData.Language = PlayerPrefs.GetInt(Constantes.Language);
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetInt(Constantes.NotFirstPlaying, _saveData.NotFirstPlaying ? 1 : 0);
        PlayerPrefs.SetInt(Constantes.GoldCount, _saveData.GoldCount);
        PlayerPrefs.SetInt(Constantes.WoodCount, _saveData.WoodCount);
        PlayerPrefs.SetInt(Constantes.RockCount, _saveData.RockCount);

        PlayerPrefs.SetInt(Constantes.StagesComplete, _saveData.StagesComplete);
        PlayerPrefs.SetInt(Constantes.GoldEarned, _saveData.GoldEarned);

        PlayerPrefs.SetInt(Constantes.HealthLevel, _saveData.HealthLevel);
        PlayerPrefs.SetInt(Constantes.DamageLevel, _saveData.DamageLevel);
        PlayerPrefs.SetInt(Constantes.MoveSpeedLevel, _saveData.MoveSpeedLevel);
        PlayerPrefs.SetInt(Constantes.AttackRangeLevel, _saveData.AttackRangeLevel);
        PlayerPrefs.SetInt(Constantes.AttackSpeedLevel, _saveData.AttackSpeedLevel);
        PlayerPrefs.SetInt(Constantes.RebounceProjectileLevel, _saveData.RebounceProjectileLevel);
        PlayerPrefs.SetInt(Constantes.DoubleAttack, _saveData.DoubleAttack ? 1 : 0);
        PlayerPrefs.SetInt(Constantes.ThroughArrow, _saveData.ThroughArrow ? 1 : 0);
        PlayerPrefs.SetInt(Constantes.ExploseArrow, _saveData.ExploseArrow ? 1 : 0);

        if (_saveData.ExploseArrow == true)
        {
            PlayerPrefs.SetInt(Constantes.ExploseArrowDamageLevel, _saveData.ExploseArrowDamageLevel);
            PlayerPrefs.SetInt(Constantes.ExploseArrowRangeLevel, _saveData.ExploseArrowRangeLevel);
        }

        PlayerPrefs.SetInt(Constantes.MusicEnable, _saveData.MusicEnable ? 1 : 0);
        PlayerPrefs.SetInt(Constantes.EffectsEnable, _saveData.EffectsEnable ? 1 : 0);
        PlayerPrefs.SetFloat(Constantes.VolumeValue, _saveData.VolumeValue);
        PlayerPrefs.SetInt(Constantes.Language, _saveData.Language);
    }
}

[Serializable]
public class SaveData
{
    public bool NotFirstPlaying;

#region RESOURCES
    public int GoldCount;
    public int WoodCount;
    public int RockCount;
#endregion
    
#region LEADERBOARD
    public int StagesComplete;
    public int GoldEarned;
#endregion

#region SKILLS
    public int HealthLevel;
    public int DamageLevel;
    public int MoveSpeedLevel;
    public int AttackRangeLevel;
    public int AttackSpeedLevel;
    public int RebounceProjectileLevel;
    public bool DoubleAttack;
    public bool ThroughArrow;
    public bool ExploseArrow;
    public int ExploseArrowDamageLevel;
    public int ExploseArrowRangeLevel;
#endregion

#region ABILITYBUTTONS
    public bool Damage0Button;
    public bool Damage1Button;
#endregion

#region SETTINGS
    public bool MusicEnable;
    public bool EffectsEnable;
    public float VolumeValue;
    public int Language;
#endregion
}
