using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWeapon : MonoBehaviour
{
    [SerializeField] Vector3 recoil, originalRotation;
    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            AddRecoil();
        }

        else if (Input.GetButtonUp("Fire1"))
        {
            ResetRecoil();
        }
    }

    void AddRecoil()
    {
        transform.localEulerAngles += recoil;
    }

    void ResetRecoil()
    {
        transform.localEulerAngles = originalRotation;
    }
}
