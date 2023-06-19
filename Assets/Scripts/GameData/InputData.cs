using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class InputData : ScriptableObject
{
    public float _minDistance;

    public float _maxTime;
    [Range(0, 1)]
    public float _directionThreshold;
}
