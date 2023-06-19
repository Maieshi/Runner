using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WeaponData : ScriptableObject
{
    public GameObject WeaponPrefab;
    public GameObject ProjectilePrefab;
    public Transform ProjectileSocket;
    public float ProjectileSpeed;
    public float ProjectileRadius;
    public int WeaponDamage;
    public int CurrentInMagazine;
    public int MaxInMagazine;
    public int TotalAmmo;
    // ///////
    public float FireRate;
    public float ReloadSpeed;
    public int BulletsPerShot;
    public float SpreadRadius;
}
