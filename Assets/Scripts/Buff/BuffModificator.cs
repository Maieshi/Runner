using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff/Modificator")]
public class BuffModificator : ScriptableObject
{
    [SerializeField]
    private float _value;

    public float Value => _value;

    public OperationType Operation;
    public ModificatorType Type;
    public bool IsBlocker;
    public float Duration;
}
public enum OperationType
{
    Add,
    Mutiply
}


public enum ModificatorType
{
    Temporary,
    Permanent
}