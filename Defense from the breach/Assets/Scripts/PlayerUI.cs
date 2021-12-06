using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    Image playerHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerHealthBar.fillAmount = PlayerBase.instance.health / PlayerBase.instance.maxHealth;

        if(playerHealthBar.fillAmount < .4f)
        {
            playerHealthBar.color = Color.red;
        }
        else
        {
            playerHealthBar.color = Color.green;
        }
    }
}
