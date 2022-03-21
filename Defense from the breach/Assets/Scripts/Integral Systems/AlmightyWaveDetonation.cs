using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmightyWaveDetonation : MonoBehaviour
{
    public float detonationTime;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePause.paused)
        {
            return;
        }

        if(detonationTime > 0)
        {
            detonationTime -= GamePause.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<AdvancedEnemyAI>())
        {
            //Damage the enemy here
        }
    }
}
