using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleClassAbilities : MonoBehaviour
{
    Vector3 move;
    float moveSpeed = 3;
    public string dodgeAbility = "BackPace";
    public string buffAbility = "SpeedBuff";
    bool isDodging;

    Rigidbody rb;

    public float sideStepDistance, backPaceDistance;
    public float speedModifier = 1.0f, jumpModifier = 1.0f;

    public float buffTimer;
    public float speedBuffTimer, speedBuffCooldown;
    public float jumpBuffTimer, jumpBuffCooldown, buffCoolDown;

    public readonly string[] dodgeAbilities = { "BackPace", "SideStep" };
    public readonly string[] moveBuffs = { "SpeedBuff", "JumpBuff" };

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        move = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!isDodging)
            {
                DodgeAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeAbility();
        }

        checkBuff();

        rb.velocity = (transform.right * move.x * moveSpeed) + (transform.forward * move.z * moveSpeed);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
    }

    private void FixedUpdate()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        move = new Vector3(xMove, 0, zMove).normalized;
    }

    

    void ChangeAbility()
    {
        foreach(string ability in dodgeAbilities)
        {
            if(dodgeAbility != ability)
            {
                dodgeAbility = ability;
                break;
            }
        }
    }

    #region Dodge Abilities
    void DodgeAbility()
    {
        switch (dodgeAbility)
        {
            case "SideStep":
                StartCoroutine(SideStepDodge(move.x));
                break;
            case "BackPace":
                StartCoroutine(BackPaceDodge());
                break;
            default:
                break;
        }
    }


    IEnumerator SideStepDodge(float xMove)
    {
        
        if(xMove == 0)
        {
            xMove = 1;
            Debug.Log(xMove);
        }

        isDodging = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);

        rb.AddForce((transform.right * sideStepDistance * xMove) * speedModifier, ForceMode.Impulse);
        yield return new WaitForSeconds(.4f);
        rb.velocity = new Vector3(0f, 0f, 0f);
        isDodging = false;
    }

    IEnumerator BackPaceDodge()
    {
        isDodging = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce((-transform.forward * backPaceDistance) * speedModifier, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);

        rb.velocity = new Vector3(0f, 0f, 0f);

        isDodging = false;
    }
    #endregion

    #region Buffs

    void checkBuff()
    {
        if(buffTimer > 0)
        {
            buffTimer -= Time.deltaTime;
        }

        if(buffCoolDown > 0)
        {
            buffCoolDown -= Time.deltaTime;
        }

        if(buffTimer <= 0)
        {
            switch (buffAbility)
            {
                case "SpeedBuff":
                    speedModifier = 1f;
                    break;
                case "JumpBuff":
                    jumpModifier = 1f;
                    break;
                default:
                    break;
            }

            if (Input.GetKeyDown(KeyCode.C) && buffCoolDown <= 0)
            {
                MoveBuff();
            }

        }
    }

    public void MoveBuff()
    {
        switch (buffAbility)
        {
            case "SpeedBuff":
                //Increase move speed and set timer
                speedModifier = 1.2f;
                buffTimer = speedBuffTimer;
                buffCoolDown = speedBuffCooldown;
                break;
            case "JumpBuff":
                //Increase jump height and set timer
                jumpModifier = 1.3f;
                buffTimer = jumpBuffTimer;
                buffCoolDown = jumpBuffCooldown;
                break;
            default:
                break;
        }
    }

    #endregion
}
