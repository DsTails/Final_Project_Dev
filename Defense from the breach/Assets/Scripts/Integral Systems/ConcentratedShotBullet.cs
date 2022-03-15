using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentratedShotBullet : BulletObject
{
    PlayerBase ownerPlayer;
    Rigidbody rb;
    float concentratedShotModifier;
    float shotSpeed;

    private void Start()
    {
        ownerPlayer = PlayerBase.instance;
        rb = GetComponent<Rigidbody>();
    }

    public void setDamage(int bulletDamage, float concentratedModifier)
    {
        damage = bulletDamage * concentratedModifier;
    }

    public void setSpeed()
    {
        rb.velocity = transform.forward * shotSpeed;
    }

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<AdvancedEnemyAI>())
        {
            AdvancedEnemyAI targetEnemy = other.gameObject.GetComponent<AdvancedEnemyAI>();
            //Damage enemy (In the advanced enemy AI script, have the checks to determine whether or not the enemy has armour that can be damaged, or to damage them directly
            targetEnemy.TakeDamage(damage, true);

            //Increase damage on hit in player script
            ownerPlayer.GetComponent<DynamicClassAbilities>().increaseConcentratedDamage();
            ownerPlayer.GetComponent<DynamicClassAbilities>().increaseMissileBuffStack();

        }
    }
}
