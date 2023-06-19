using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TriggerHandler))]
public class ObstaclePrefabData : AbstractObjectData
{
    public bool IsRotatesToNormal;

    public ObstacleSize Size;
}

public enum ObstacleSize
{
    Small,
    Medium,
    Large
}