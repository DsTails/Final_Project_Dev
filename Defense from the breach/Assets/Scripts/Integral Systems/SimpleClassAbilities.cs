using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleClassAbilities : MonoBehaviour
{
    #region Variables
    //Strings to hold the selected node for each group
    [Header("Selected Abilities")]
    public string selectedJump;
    public string selectedDodge;
    public string selectedBuff;
    //insert fourth ability string here

    //Cooldowns for each ability type
    [Header("Ability Cooldowns")]
    [SerializeField] float buffCooldown;
    public float speedBuffCooldown;
    public float jumpBuffCooldown;
    [SerializeField] float dodgeCooldown;
    public float sideStepCooldown;
    public float backPaceCooldown;

    //Timers for how long specific abilities last
    [Header("Ability Timers")]
    [SerializeField] float jumpTimer;
    public float rocketJumpTimer;
    public float gravJumpTimer;
    [SerializeField] float buffTimer;
    public float speedBuffTimer;
    public float jumpBuffTimer;

    //Properties for each of the abilities (Where applicable)
    [Header("Ability Properties")]
    public float rocketJumpHeight;
    public float ZeroGravityDuration;
    public float sideStepDistance;
    public float backPaceDistance;
    public float speedBuffMod = 1.2f;
    public float jumpBuffMod = 1.4f;
    public bool isDodging;
    public bool isSuperJump;

    //Read Only Arrays for the Abilities
    public readonly string[] dodgeAbilities = { "SideStepDodge", "BackPaceDodge" };
    public readonly string[] jumpAbilities = { "RocketJump", "ZeroGravJump", "Jump" };
    public readonly string[] buffAbilities = { "MoveSpeedBuff", "HeightBuff" };

    //player properties
    Rigidbody rb;
    Vector3 move;
    PlayerBase player;
    #endregion

    private void Start()
    {
        player = PlayerBase.instance;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForAbilityPress();
        checkForAbilityTimers();
        checkForAbilityCooldowns();
    }

    private void FixedUpdate()
    {
        move.x = Input.GetAxisRaw("Horizontal");
    }

    #region Checks
    void checkForAbilityPress()
    {
        if (Input.GetButtonDown("Dodge"))
        {
            if(dodgeCooldown <= 0) {
                
                DodgeAbility();
            }
        }
        else if (Input.GetButtonDown("Buff"))
        {
            if(buffCooldown <= 0)
            {
                BuffAbility();
            }
        }

        else if (Input.GetButtonDown("SuperJump"))
        {
            JumpAbility();
        }
    }

    void checkForAbilityTimers()
    {
        if(buffTimer > 0)
        {
            buffTimer -= GamePause.deltaTime;
        }
        else
        {
            switch (selectedBuff)
            {
                case "MoveSpeedBuff":
                    player.speedModifier = 1.0f;
                    break;
                case "HeightBuff":
                    player.jumpModifier = 1.0f;
                    break;
            }
        }
    }

    void checkForAbilityCooldowns()
    {
        if(buffCooldown > 0)
        {
            buffCooldown -= GamePause.deltaTime;
        }
        if(dodgeCooldown > 0)
        {
            dodgeCooldown -= GamePause.deltaTime;
        }
    }

    #endregion

    #region Dodge Abilities
    void DodgeAbility()
    {
        switch (selectedDodge)
        {
            case "SideStepDodge":
                StartCoroutine(SideStepDodgeAbility(move.x));
                break;
            case "BackPaceDodge":
                StartCoroutine(BackPaceDodgeAbility());
                break;
            default:
                break;
        }
    }

    IEnumerator SideStepDodgeAbility(float xMove)
    {
        if(xMove == 0)
        {
            xMove = 1;
        }

        isDodging = true;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);

        rb.AddForce((transform.right * xMove * sideStepDistance) * speedBuffMod, ForceMode.Impulse);
        yield return new WaitForSeconds(.3f);

        rb.velocity = Vector3.zero;
        isDodging = false;
    }

    IEnumerator BackPaceDodgeAbility()
    {
        isDodging = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce((-transform.forward * backPaceDistance) * speedBuffMod, ForceMode.Impulse);

        yield return new WaitForSeconds(.6f);

        rb.velocity = Vector3.zero;
        isDodging = false;
    }
    #endregion

    #region Buff Abilities
    void BuffAbility()
    {
        switch (selectedBuff)
        {
            case "MoveSpeedBuff":
                player.speedModifier = speedBuffMod;
                buffTimer = speedBuffTimer;
                buffCooldown = speedBuffCooldown;
                break;
            case "HeightBuff":
                player.jumpModifier = jumpBuffMod;
                buffTimer = jumpBuffTimer;
                buffCooldown = jumpBuffCooldown;
                break;
            default:
                break;
        }
    }
    #endregion

    #region Jump Abilities
    void JumpAbility()
    {
        switch (selectedJump)
        {
            case "RocketJump":
                rb.velocity = Vector3.up * player.jumpForce * rocketJumpHeight * jumpBuffMod;
                player.speed = player.speed / 4;
                Invoke("ResetSpeed", 2.2f);
                break;
            case "ZeroGravJump":
                rb.useGravity = false;
                Invoke("ResetGravity", ZeroGravityDuration);
                break;
            case "Jump":
                rb.velocity = Vector3.up * player.jumpForce * jumpBuffMod;
                break;
            default:
                //No jump active, do nothing
                break;
        }
    }

    void ResetGravity()
    {
        rb.useGravity = true;
    }

    void ResetSpeed()
    {
        player.speed = player.defaultSpeed;
    }
    #endregion

    #region Ability Changes
    public void setAbility(string abilityToSet)
    {
        if (abilityToSet.Contains("Jump")){
            selectedJump = abilityToSet;
        } else if (abilityToSet.Contains("Dodge"))
        {
           
            selectedDodge = abilityToSet;
        } else if (abilityToSet.Contains("Buff"))
        {
            selectedBuff = abilityToSet;
        }
    }
    #endregion
}
