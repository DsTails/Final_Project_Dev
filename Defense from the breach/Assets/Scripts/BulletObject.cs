using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    [SerializeField]
    float destroyTime;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void setDamage(int damage)
    {

    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;

        if(destroyTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<EnemyAI>())
        {
            EnemyAI hurtEnemy = other.gameObject.GetComponent<EnemyAI>();
            hurtEnemy.hurtEnemy(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.GetComponent<PlayerBase>())
        {
            PlayerBase.instance.hurtPlayer(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
