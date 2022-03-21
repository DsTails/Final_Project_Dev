using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmightyWaveBase : MonoBehaviour
{
    public float projectileSpeed;
    public float baseDamage;
    public GameObject detonationObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.GetComponent<AdvancedEnemyAI>())
        {
            GameObject detonation = Instantiate(detonationObject, transform.position, transform.rotation);
            if (other.gameObject.GetComponent<AdvancedEnemyAI>())
            {
                //Damage the enemy here
            }
            Destroy(this.gameObject);
        }
    }
}
