using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GUNS{
    PISTOL, 
    MACHINEGUN,
    SHOTGUN,
    GRENADE_LAUNCHER,
    ROCKET_LAUNCHER,
    RAILGUN
}


[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object/Weapon")]
public class WeaponData : ScriptableObject
{

    [Header("Physical model")]
    [SerializeField] public bool IsPhysical;
    [SerializeField] public bool IsPenitrate;
    [SerializeField] public bool IsAutomatic;

    [Header("General Settings")]
    [SerializeField] public GUNS gunType;
    [SerializeField] public bool IsActive;
    [SerializeField] public bool InfinitiveAmmo;
    [SerializeField] public int BasicDamage;
    [SerializeField] public int MagazineSize;
    [SerializeField] public int CurrentAmmo;
    [SerializeField] public float FireRate; // work only for automatic guns
    

    [Header("SFX sounds")]
    [SerializeField] public AudioClip ShootSound;
    [SerializeField] public AudioClip RunOutOfAmmoSound;
    
    // for raycast physical model
    [Header("Raycast shooting params")]
    [SerializeField] public int RaysPerShooot;
    [SerializeField] public float MaxYAxisScutter;
    [SerializeField] public float MaxXAxisScutter;

    [Header("Physic shooting params")]
    [SerializeField] public float BulletForse;
    [SerializeField] public float MaxShootDistance;

    
    [Header("Particles variables")]
    [SerializeField] public ParticleSystem muzzleFlash;
    [SerializeField] public TrailRenderer ShootTrail;

}
