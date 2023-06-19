using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public Transform PlayerSpawnpoint;

    public Transform PlatfromSpawnpoint;

    public float DistanceBeetweenLines;

    public bool IsGameStarted;

    public List<string> InitDeactiavatedSystems;

    public static float currentRotation = 0;

}
