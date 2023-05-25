using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExtractionableResource : InteractableObject
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private int _amount;
    [SerializeField] private List<Collider> _colliders;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _modelAfterExtraction;
    [SerializeField] private ExtractingAmountAnimator _amountAnimator;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private AudioClip[] _clips;

    private AudioSource _source;

    public event Action Extracted;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();

        _model.SetActive(false);
        _modelAfterExtraction.SetActive(true);
    }

    public void Init()
    {
        foreach (var collider in _colliders)
            collider.enabled = true;

        _model.SetActive(true);
        _modelAfterExtraction.SetActive(false);
    }

    public void Extract(ResourcesStorage storage)
    {
        storage.Add(_resourceType, _amount);
        Extracted?.Invoke();
        _model.SetActive(false);
        _modelAfterExtraction.SetActive(true);
        _particle.Play();
        _source.PlayOneShot(GetRandomClip());

        foreach (var collider in _colliders)
            collider.enabled = false;

        _amountAnimator.PlayExtract(_resourceType, _amount);
    }

    private AudioClip GetRandomClip() => _clips[UnityEngine.Random.Range(0, _clips.Length)];
}