using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningOrb : MonoBehaviour
{
    public float speed;
    public float damage;

    //Get the animator
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
            if (other.gameObject.GetComponent<AdvancedEnemyAI>())
            {

            }
            Destroy(gameObject);
        }
    }
}
