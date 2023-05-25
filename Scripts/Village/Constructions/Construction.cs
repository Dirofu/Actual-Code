using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class Construction : InteractableObject
{
    [SerializeField] private GameObject _model;
    [SerializeField] private ConstructionInfo _info;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private AudioClip _endBuilding;

    protected AudioSource Source;

    private const float UpgradeDelay = 1.2f;

    public ConstructionInfo Info => _info;
    public UpgradeImprovementInfo CurrentLevel => _info.Levels[CurrentLevelIndex];
    public int CurrentLevelIndex { get; private set; }
    public int LevelCount => _info.Levels.Count;
    public bool HasNextLevel => CurrentLevelIndex + 1 < LevelCount;
    public bool IsBuilt { get; private set; }
    public bool IsBuilding { get; private set; }

    public event Action<Construction> UpgradeStarted;
    public event Action<Construction> Upgraded;

    protected virtual void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    public void Upgrade()
    {
        if (HasNextLevel == false)
            throw new InvalidOperationException();

        UpgradeStarted?.Invoke(this);
        IsBuilding = true;
        StartCoroutine(UpgradeWithDelay(UpgradeDelay));
    }

    public UpgradeImprovementInfo GetNextLevel()
    {
        if (HasNextLevel == true)
            return _info.Levels[CurrentLevelIndex + 1];
        else
            return null;
    }

    private IEnumerator UpgradeWithDelay(float delay)
    {
        GameObject nextModel;
        GameObject oldModel;

        Source.Play();
        _particle.Play();
        yield return new WaitForSeconds(delay);
        nextModel = GetNextLevel().Model;

        if (nextModel != _model)
        {
            oldModel = _model;
            Destroy(_model.gameObject);
            _model = Instantiate(nextModel, transform.position, oldModel.transform.rotation, transform);
        }

        CurrentLevelIndex++;
        IsBuilding = false;
        IsBuilt = true;
        Upgraded?.Invoke(this);
        _particle.Stop();
        Source.Stop();
        Source.PlayOneShot(_endBuilding);
        StopCoroutine(UpgradeWithDelay(delay));
    }
}
