using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBarrageProj : MonoBehaviour
{
    public float missileDamageBase;
    public float missileDamage;
    public float lockOnTime;
    PlayerBase ownerPlayer;
    Transform targetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        ownerPlayer = PlayerBase.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDamage(float damageModifier)
    {
        missileDamage = (missileDamageBase * damageModifier);
    }

    public void SetTarget()
    {

    }

    public void MissileHoming()
    {
        Invoke("SetTarget", lockOnTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<AdvancedEnemyAI>())
        {
            //Damage the enemy (Will likely need to change the damage script to accommodate for more than one enemy type/ammo type
            //Need to check this
            ownerPlayer.GetComponent<DynamicClassAbilities>().increaseProjectilesModifier();
        }
    }
}
