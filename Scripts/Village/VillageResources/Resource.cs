using System;
using UnityEngine;

[Serializable]
public class Resource : IReadOnlyResource
{
    [SerializeField] private ResourceType _type;
    [SerializeField] private Sprite _sprite;

    public int Amount { get; private set; }
    public ResourceType Type => _type;
    public Sprite Sprite => _sprite;

    public event Action<int> AmountChanged;

    public bool CanGive(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        return Amount >= amount;
    }

    public void Give(int amount)
    {
        if (CanGive(amount) == false)
            throw new InvalidOperationException();

        Amount -= amount;
        AmountChanged?.Invoke(Amount);
    }

    public void Add(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        Amount += amount;
        AmountChanged?.Invoke(Amount);
    }

    public void SetAmount(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException();

        Amount = amount;
        AmountChanged?.Invoke(Amount);
    }
}