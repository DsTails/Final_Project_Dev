using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags] public enum Weapon_Type
{
    AutoShot = 0,
    BurstShot = 1,
    SingleShot = 2,
    Burst_AutoShot = 4
}

public class WeaponTypes : MonoBehaviour
{
    public Weapon_Type type;
    public double RoundsPerMin;
    //Only necessary if the weapon type is a burst weapon
    public int burstRounds;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform muzzleEnd;
    public float bulletSpeed = 30;

    public int MaxAmmoCount;
    [SerializeField]int AmmoCount;

    public float reloadSpeed;
    public readonly float maxReloadSpeed = 100;
    
    [SerializeField] float rateOfReload;

    [SerializeField] double RoundsPerSec;
    [SerializeField] double fireRate;
    
    double burstFireRate;
    
    double chosenFireRate;
    [SerializeField] float recoilTime;
    [SerializeField] float stability;
    [SerializeField] Vector3 deviationVal;

    [SerializeField] Vector3 recoil;
    Vector3 recoilStart, recoilOrigin;
    [SerializeField] Vector3 recoilPeak;

    // Start is called before the first frame update
    void Start()
    {
        RoundsPerSec = System.Math.Round((RoundsPerMin / 60), 2);
        fireRate = (1 / RoundsPerSec);
        fireRate = System.Math.Round(fireRate, 2);
        burstFireRate = fireRate + .2f;
        //burstFireRateVal = burstFireRate;
        //fireRateVal = fireRate;
        AmmoCount = MaxAmmoCount;
        rateOfReload = (reloadSpeed / maxReloadSpeed) * 5;
        recoilOrigin = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(chosenFireRate > 0)
        {
            chosenFireRate -= Time.deltaTime;
        }

        checkForFireInput();
        checkForReloadInput();
    }

    #region Input Checks
    private void checkForFireInput()
    {
        //Check the weapon types

        if(type == Weapon_Type.BurstShot || type == Weapon_Type.SingleShot)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (chosenFireRate <= 0)
                {
                    if (AmmoCount > 0)
                    {
                        fireWeapon(type);
                        if (type == Weapon_Type.SingleShot)
                        {
                            chosenFireRate = fireRate;
                        }
                        else
                        {
                            chosenFireRate = burstFireRate;
                        }
                    }
                    else
                    {
                        StartCoroutine(reloadWeapon());
                    }
                }
            }
            
        } else if(type == Weapon_Type.AutoShot || type == Weapon_Type.Burst_AutoShot)
        {
            
            if(Input.GetButton("Fire1"))
            {
                if (chosenFireRate <= 0)
                {
                    if (AmmoCount > 0)
                    {
                        fireWeapon(type);

                        if(type == Weapon_Type.AutoShot)
                        {
                            chosenFireRate = fireRate;
                        }
                        else
                        {
                            chosenFireRate = burstFireRate;
                        }

                        
                    }
                    else
                    {
                        StartCoroutine(reloadWeapon());
                    }
                }
            }
        }
    }

    void checkForReloadInput()
    {
        if (Input.GetButtonDown("Reload"))
        {
            StartCoroutine(reloadWeapon());
        }
    }
    #endregion

    #region Firing Weapon

    void fireWeapon(Weapon_Type typing)
    {
        if(typing == Weapon_Type.BurstShot || typing == Weapon_Type.Burst_AutoShot)
        {
            double bulletRate = fireRate / burstRounds;
            float rate = Convert.ToSingle(bulletRate);
            StartCoroutine(burstFire(rate));
        }

        else if(typing == Weapon_Type.SingleShot || typing == Weapon_Type.AutoShot)
        {
            createBullet();
            AddRecoil();
        }
    }

    public IEnumerator burstFire(float rate)
    {
        for(int i = 0; i < burstRounds; i++)
        {
            createBullet();
            yield return new WaitForSeconds(rate);
        }
    }
    #endregion

    #region Recoil Methods

    void AddRecoil()
    {
        var recoilStart = transform.localEulerAngles;
        
        var recoilProduct = transform.localEulerAngles + recoil;

        /*var recoilEnd = new Vector3(
            Mathf.LerpAngle(recoilStart.x, recoilProduct.x, 3 * Time.deltaTime),
            Mathf.LerpAngle(recoilStart.y, recoilProduct.y, 3 * Time.deltaTime),
            recoilStart.z
            );*/

        StartCoroutine(recoilTransition(recoilStart, recoilProduct));

        ResetRecoil();

    }

    void ResetRecoil()
    {
        
        transform.localEulerAngles = recoilOrigin;
    }

    #endregion

    #region Utility Methods
    public IEnumerator reloadWeapon()
    {
        yield return new WaitForSeconds(rateOfReload);
        AmmoCount = MaxAmmoCount;
    }

    void createBullet()
    {
        GameObject shootBullet = Instantiate(bulletPrefab, muzzleEnd.position, muzzleEnd.rotation);
        shootBullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        AmmoCount -= 1;
    }

    public IEnumerator recoilTransition(Vector3 recoilStart, Vector3 recoilProduct)
    {
        
        float TimePercentage = 0f;
        while(TimePercentage < 1)
        {
            TimePercentage += Time.deltaTime / recoilTime;
            transform.localEulerAngles = Vector3.Lerp(recoilStart, recoilProduct, TimePercentage);
            recoilPeak = transform.localEulerAngles;
            yield return null;
        }

        

        yield return new WaitForSeconds(.15f);
        StartCoroutine(recoilResetTransition(recoilPeak, recoilProduct));

        
    }

    

    public IEnumerator recoilResetTransition(Vector3 currentRecoil, Vector3 recoilResetPos)
    {
        
        float TimePercentage = 1f;
        while(TimePercentage > 0)
        {
            TimePercentage -= Time.deltaTime / recoilTime;
            transform.localEulerAngles = Vector3.Lerp(currentRecoil, recoilResetPos, TimePercentage);
            yield return null;
        }
    }

    #endregion
}
