using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicClassAbilities : MonoBehaviour
{
    [Header("Selected Abilities")]
    public string selectedProjectile = "Ordnance - Spinning Orb";
    public string buffOrMissile = "Concentrated Spec - Missile Barrage";
    public string selectedStatBoost = "Strengthen - Weapon Damage";
    

    [Header("Ability Cooldowns")]
    public float projectileCooldown;
    public float almightyWaveCooldown;
    public float splitterCooldown;
    public float spinningOrbCooldown;
    public float concentratedShotCooldown;


    [Header("Ability Timers")]
    public float concentratedShotTimer;
    public float projectileBuffTimer;
    public float weaponBuffTimer;


    [Header("Ability Properties")]
    public float concentratedShotMultiplier;
    public float concentratedRateVal;
    public float concentratedRateOfFire;
    [SerializeField] GameObject concentratedBullet;
    public float weaponBuffVal;
    public float projectileModifier;
    [SerializeField] GameObject[] selectedProjectileObject;
    [SerializeField] GameObject projectileObject;
    public int missileStacks;
    public float missileDelay;
    public float weaponBuffModifier;

    [Header("Required Prefabs")]
    public GameObject missileObject;

    [Header("Object Creation Properties")]
    [SerializeField] Transform missileLauncherOne;
    [SerializeField] Transform missileLauncherTwo;

    [Header("Timer Booleans")]
    bool projBuffTimer;
    public bool concentratedShotActive;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePause.paused)
        {
            return;
        }
        CheckForConcentratedShot();
        CheckForProjectiles();
    }

    #region Checks for Abilities
    void CheckForConcentratedShot()
    {
        //If the ability is active, then check for input
        if(concentratedShotCooldown <= 0)
        {
            //Check for the input to activate this ability
        }
        else
        {
            concentratedShotCooldown -= GamePause.deltaTime;
        }

        if (concentratedShotActive && concentratedShotTimer > 0)
        {

            if (Input.GetButtonDown("fire"))
            {
                if (concentratedRateOfFire <= 0)
                {
                    ConcentratedShot();
                }
            }
        }
    }

    void CheckForProjectiles()
    {
        //
        if (Input.GetButtonDown("Projectile"))
        {
            if (projectileCooldown <= 0)
            {
                CreateProjectile();
            }
        }
        //Have switch case for selected ability etc.
    }
    #endregion

    #region Offensive Abilities
    public void ConcentratedShot()
    {
        GameObject shotBullet = Instantiate(concentratedBullet, GetComponentInChildren<WeaponTypes>().muzzleEnd.position, GetComponentInChildren<WeaponTypes>().muzzleEnd.rotation);
        shotBullet.GetComponent<ConcentratedShotBullet>().setDamage(20, concentratedShotMultiplier);
        shotBullet.GetComponent<ConcentratedShotBullet>().setSpeed();
    }

    public void FireMissiles()
    {
        StartCoroutine(MissileBarrage(missileStacks));
        missileStacks = 0;

    }
    public IEnumerator MissileBarrage(int missileStack)
    {
        while(missileStack > 0)
        {
            GameObject missile = Instantiate(missileObject, missileLauncherOne.position, missileLauncherOne.rotation);
            missile.GetComponent<MissileBarrageProj>().SetDamage(projectileModifier);
            //Set the velocity of the object
            missileStack--;
            yield return new WaitForSeconds(missileDelay);

            if(missileStack <= 0)
            {
                continue;
            }
            else
            {
                missile = Instantiate(missileObject, missileLauncherTwo.position, missileLauncherTwo.rotation);
                //Set speed, damage, etc
                missileStack--;

                //Either check again, or simply leave it to the loop condition
            }
        }
        projBuffTimer = true;
        missileStacks = 0;
        //Attack ends, start timer for projectile buff
    }
    #endregion

    #region Projectiles and Bombs
    void CreateProjectile()
    {
        //To Do: Create position where the projectile is thrown from
        GameObject spawnedProjectile = Instantiate(projectileObject);

        switch (selectedProjectile)
        {
            case "Ordnance - Spinning Orb":
                projectileCooldown = spinningOrbCooldown;
                break;
            case "Ordnance - Splitter Frag":
                projectileCooldown = splitterCooldown;
                break;
            case "Ordnance - Almighty Wave":
                projectileCooldown = almightyWaveCooldown;
                break;
        }
    }
    #endregion

    #region Post Ability Buffs

    #endregion

    #region Ability Hit Methods
    public void increaseConcentratedDamage()
    {
        concentratedShotMultiplier += .2f;
    }

    public void increaseMissileBuffStack()
    {
        //Use a string to determine whether a certain ability (Missile stacks or damage buff) has been selected
        //This will be used to determine whether to increase
        switch (buffOrMissile)
        {
            case "AttackBuff":
                weaponBuffVal += .2f;
                break;
            case "MissileStack":
                missileStacks += 1;
                break;
            default:
                break;
        }
    }

    public void increaseProjectilesModifier()
    {
        projectileModifier += .2f;
    }
    #endregion
}
