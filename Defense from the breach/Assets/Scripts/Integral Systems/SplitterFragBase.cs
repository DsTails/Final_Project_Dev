using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterFragBase : MonoBehaviour
{
    Rigidbody rb;
    public float detonateTime;
    [SerializeField] GameObject fragPiecePrefab;
    [SerializeField] Transform[] fragPieceSpawns;
    bool hasSpawnedFrags;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(detonateTime > 0)
        {
            detonateTime -= GamePause.deltaTime;
        }
        else
        {
            if (!hasSpawnedFrags)
            {
                StartCoroutine(SpawnFragPieces());
                hasSpawnedFrags = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.GetComponent<AdvancedEnemyAI>())
        {
            StartCoroutine(SpawnFragPieces());
        }
    }

    IEnumerator SpawnFragPieces()
    {
        for(int i = 0; i < fragPieceSpawns.Length; i++)
        {
            int fragLimit = 0;

            if (fragLimit != 1)
            {
                GameObject fragPiece = Instantiate(fragPiecePrefab, fragPieceSpawns[i].position, fragPieceSpawns[i].rotation);
                fragLimit = 1;
            }
            

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
