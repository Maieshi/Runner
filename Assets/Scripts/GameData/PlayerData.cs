using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buff;
[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public GameObject PlayerPrefab;

    public BuffTarget PlayerForwardSpeed;

    public IntBuffTarget PlayerHp;


    public float PlayerHorizontalSpeed;

    public float PlayerGravityScale;

    public AnimationCurve PlayerJumpCurve;

    public static RotationDirection CurrentDiretion;

    public float PlayerJumpDuration;
    public BuffTarget PlayerJumpSpeed;

    public float PlayerCameraSensitivity;
}
