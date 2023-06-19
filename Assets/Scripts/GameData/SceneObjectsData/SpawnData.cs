using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class SpawnData : ScriptableObject
{
    public List<ObjectData> PlatformData;

    public List<ObjectData> ObstacleData;

    public List<ObjectData> BoosterData;

    public FloorPrefabData StartPlatform;

    public FloorPrefabData FinishPlatform;

    public TransitionPlatformData TransitionPlatform;

    public int MinPlatformAmount;

    public int MaxPlatformAmount;

    public int MinObstaclesDistance;

    [Range(1, 100)]
    public int ObstacleSpawnProbability;

    public int MinBoostDistance;

    [Range(1, 100)]
    public int BoostSpawnProbability;
    [Range(3, 10)]
    public int MaxTransitions;

    [Range(2, 5)]
    public int SpawnBorder;
}
[System.Serializable]
public class ObjectData
{
    public AbstractObjectData PrefabData;

    public float Probability;
}
