using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterFragPiece : MonoBehaviour
{
    Rigidbody rb;
    public float fragPieceDamage = 10f;
    public float fragMoveSpeed, fragHeight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.parent = null;
        rb.velocity = transform.forward * fragMoveSpeed + transform.up * fragHeight;
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
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
