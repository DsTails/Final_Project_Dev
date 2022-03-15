using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoDropped;

    //Add ammo type as an enum here and in the weapon script

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<WeaponTypes>())
        {
            WeaponTypes playerWeapon = other.gameObject.GetComponentInChildren<WeaponTypes>();
            playerWeapon.AmmoHeld += ammoDropped;
            //Trigger ammo method here
            Debug.Log("FOUND OBJECT");
            Destroy(this.gameObject);
        }
    }
}
