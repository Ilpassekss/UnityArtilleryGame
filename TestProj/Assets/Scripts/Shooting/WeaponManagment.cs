using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;



public class WeaponManagement : MonoBehaviour
{
    [SerializeField] List<GameObject> weapons;
    private GameObject currentWeapon;
    private Shooting activeShootingScript;

    PlayerInputControls input;

    void Start()
    {
        input = PlayerManager.instance.input;
        EquipWeapon(0);
    }

    void OnDestroy()
    {
        activeShootingScript.DestroyGun(currentWeapon);
    }

    public void Update()
    {
        if (activeShootingScript != null)
        {
           
            activeShootingScript.Shoot(input.playerInput.ShootHold, input.playerInput.ShootPress);
        }
        
    }

    public void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count)
        {
            Debug.LogWarning("Invalid weapon index: " + weaponIndex);
            return;
        }

        if (currentWeapon == weapons[weaponIndex])
        {
            Debug.Log("Weapon is already equipped: " + currentWeapon.name);
            return;
        }

        if (currentWeapon != null)
        {
            activeShootingScript = currentWeapon.GetComponent<Shooting>();

            // Check animation playing
            if (activeShootingScript != null && activeShootingScript.IsAnimationPlaying())
            {
                Debug.Log("Cannot switch weapon while shooting animation is playing.");
                return;
            }

            activeShootingScript.DestroyGun(currentWeapon);
        }

        currentWeapon = weapons[weaponIndex];
        activeShootingScript = currentWeapon.GetComponent<Shooting>();
        activeShootingScript.InitGun(currentWeapon);
    }
}

