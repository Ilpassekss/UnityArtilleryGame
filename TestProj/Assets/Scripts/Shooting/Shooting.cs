using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public class Shooting : MonoBehaviour
{

    private float RaycastRange = 100f;

    [SerializeField] private Camera CameraPoint;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private GameObject BulletPrefab;
    private float TimeSinceLastShot;
    private Animator GunAnimator;

    // Start is called before the first frame update
    void Start()
    {
        GunAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGun(GameObject weaponObject)
    {
        if(weaponData.IsActive == true)
        {
            Debug.Log("Weapon have been initialized earlier" + weaponData.gunType + " " + weaponData.IsActive);
            return;
        }
        else
        {
            // init game obj
            weaponObject.SetActive(true);
            // set weapon data flag
            weaponData.IsActive = true;
            // update ammo count
            weaponData.CurrentAmmo = weaponData.MagazineSize;
            // player can shoot immidiately after equipment
            TimeSinceLastShot = ( 60f / (weaponData.FireRate));

            

            Debug.Log("Weapon was initialized" + weaponData.gunType);
        }  
    }  

    public void DestroyGun(GameObject weaponObject)
    {
        if (weaponData.IsActive)
        {
        
            weaponObject.SetActive(false);

            weaponData.IsActive = false;

            Debug.Log("Weapon was destroyed " + weaponData.gunType);
        }
        else
        {
            Debug.Log("Weapon has already been destroyed earlier " + weaponData.gunType + " " + weaponData.IsActive);
        }
    }

    public bool IsAnimationPlaying()
    {
        return GunAnimator != null && GunAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f && GunAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Shooting");
    }

    public void Shoot(bool shootHold, bool shootPressed)
    {
        

        if(weaponData.IsPhysical && shootPressed && CanShoot()){

            PhysicalShoot();

            TimeSinceLastShot += Time.deltaTime;
        }else{
            TimeSinceLastShot += Time.deltaTime;

            if (weaponData.IsAutomatic && shootHold && CanShoot())
            {
                RaycastShoot();  
            }
            else if (!weaponData.IsAutomatic && shootPressed && CanShoot() )
            {
                if(weaponData.IsPenitrate == true)
                {
                    PenetrateRaycastShoot();
                }
                else
                {
                    RaycastShoot();
                }   
            }
        }
    }

    private bool CanShoot() => TimeSinceLastShot >= (60f / (weaponData.FireRate) );

    private void RaycastShoot()
    {
        if (weaponData.RunOutOfAmmoSound != null && weaponData.CurrentAmmo <= 0)
        {
            AudioManager.AudioManagerInstance.PlaySound(weaponData.RunOutOfAmmoSound);
            return;
        }
        else if (weaponData.CurrentAmmo <= 0)
        {
            return;
        }

        if (weaponData.ShootSound != null && weaponData.CurrentAmmo > 0)
            AudioManager.AudioManagerInstance.PlaySound(weaponData.ShootSound);

        TimeSinceLastShot = 0f;
        
        if(!weaponData.InfinitiveAmmo)
            weaponData.CurrentAmmo--;

        for (int i = 0; i < weaponData.RaysPerShooot; i++)
        {
            Vector3 direction = CalculateScatteredDirection();

            RaycastHit hit;
            Vector3 hitPoint = CameraPoint.transform.position + direction * RaycastRange;

            if (Physics.Raycast(CameraPoint.transform.position, direction, out hit, RaycastRange))
            {
                hitPoint = hit.point;
                Debug.DrawRay(CameraPoint.transform.position, direction * hit.distance, Color.green);
                Debug.Log($"Hit in: {hit.transform.name}");

                // 
                HealthStats targetHealth = hit.transform.GetComponent<HealthStats>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(weaponData.BasicDamage);
                }
            }

            
            TrailRenderer trailRenderer = Instantiate(weaponData.ShootTrail, BulletSpawnPoint.position, Quaternion.identity);
            trailRenderer.AddPosition(BulletSpawnPoint.transform.position);

            // 
            StartCoroutine(AnimateTrail(trailRenderer, BulletSpawnPoint.transform.position, hitPoint));
        }

        PlayAnimation();
        if (weaponData.gunType == GUNS.PISTOL || weaponData.gunType == GUNS.MACHINEGUN)
            ExtractMuff();
    }

    private void PenetrateRaycastShoot()
    {
         if(weaponData.RunOutOfAmmoSound != null && weaponData.CurrentAmmo <= 0){
            AudioManager.AudioManagerInstance.PlaySound(weaponData.RunOutOfAmmoSound);
            return;
        }else if(weaponData.CurrentAmmo <= 0){
            return;
        } 

        if(weaponData.ShootSound != null && weaponData.CurrentAmmo > 0)
            AudioManager.AudioManagerInstance.PlaySound(weaponData.ShootSound);
        
        TimeSinceLastShot = 0f;

        if(!weaponData.InfinitiveAmmo)
            weaponData.CurrentAmmo--;

        for (int i = 0; i < weaponData.RaysPerShooot; i++)
        {
            Vector3 direction = CalculateScatteredDirection();

            RaycastHit[] hits = Physics.RaycastAll(CameraPoint.transform.position, direction, RaycastRange);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance)); // Сортируем по расстоянию (от ближнего к дальнему)

            
            foreach (RaycastHit hit in hits)
            {
                Debug.DrawRay(CameraPoint.transform.position, direction * hit.distance, Color.green);
                Debug.Log($"Hit in: {hit.transform.name}");

                if (!hit.transform) return;

                // draw bullet trail

                // take damage
                HealthStats targetHealth = hit.transform.GetComponent<HealthStats>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(weaponData.BasicDamage);
                }
            }
        }

        PlayAnimation();
        if (weaponData.gunType == GUNS.PISTOL || weaponData.gunType == GUNS.MACHINEGUN)
            ExtractMuff();
    }
 

    private IEnumerator AnimateTrail(TrailRenderer trail, Vector3 startPoint, Vector3 endPoint)
    {
        float time = 0;
        float duration = 0.05f; 
        while (time < duration)
        {
            trail.transform.position = Vector3.Lerp(startPoint, endPoint, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        trail.transform.position = endPoint;

        // Дать время на показ трасера и уничтожить
        yield return new WaitForSeconds(trail.time);
        Destroy(trail.gameObject);
    }

    
    private Vector3 CalculateScatteredDirection()
    {
        float scatterX = Random.Range(-weaponData.MaxXAxisScutter, weaponData.MaxXAxisScutter);
        float scatterY = Random.Range(-weaponData.MaxYAxisScutter, weaponData.MaxYAxisScutter);

        Vector3 scatterDirection = Quaternion.Euler(scatterY, scatterX, 0) * CameraPoint.transform.forward;
        return scatterDirection;
    }
 
    private void PlayAnimation()
    {
        if(GunAnimator != null)
        {
            GunAnimator.SetTrigger("IsShoot");
        }
    }
 
    private void ExtractMuff()
    {
        ExtractMuffs extractMuffs = GetComponent<ExtractMuffs>();
        extractMuffs.SpawnAndLaunch();
    }
    
    

    private void PhysicalShoot()
    {
        if(BulletPrefab == null || BulletSpawnPoint == null || CameraPoint == null)
        {
            Debug.LogWarning("Missing references in PhysicalShooting script!");
            return;
        }
        if(weaponData.RunOutOfAmmoSound != null && weaponData.CurrentAmmo <= 0){
            AudioManager.AudioManagerInstance.PlaySound(weaponData.RunOutOfAmmoSound);
            return;
        }else if(weaponData.CurrentAmmo <= 0){
            return;
        }

        if(weaponData.ShootSound != null && weaponData.CurrentAmmo > 0)
            AudioManager.AudioManagerInstance.PlaySound(weaponData.ShootSound);
        
        TimeSinceLastShot = 0f;
        if(!weaponData.InfinitiveAmmo)
            weaponData.CurrentAmmo--;
            
       
        
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, BulletSpawnPoint.rotation);
        
        
        Debug.Log($"Bullet spawned at: {BulletSpawnPoint.position}, Rotation: {BulletSpawnPoint.rotation.eulerAngles}");
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Bullet prefab must have a Rigidbody component!");
            return;
        }
        
        Vector3 shootDirection = BulletSpawnPoint.forward;
        
        Debug.Log($"Shoot direction: {shootDirection}, Magnitude: {shootDirection.magnitude}");
        
        rb.velocity = shootDirection * weaponData.BulletForse;
        PlayAnimation();
    }




}
