using System;
using UnityEngine;

public interface IReadOnlyResource
{
    public int Amount { get; }
    public ResourceType Type { get; }
    public Sprite Sprite { get; }

    public event Action<int> AmountChanged;
}
