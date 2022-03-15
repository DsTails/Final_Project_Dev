using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum classType
{
    Simple,
    Dynamic
}

public class PlayerBase : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float defaultSpeed;
    Vector3 moveInput;

    [Header("Ground Checks")]
    [SerializeField]
    Transform playerFeetPos;
    public float detectRadius;
    public LayerMask whatIsGround;
    bool isGrounded;

    [Header("Jump Variables")]
    public int jumpCount;
    public float jumpForce;
    public float rocketJumpMult;
    float defaultJumpForce;
    public float jumpTimer;
    float jumpTime;
    int jumps;
    bool isJumping;

    Vector3 moveVector;

    [Header("Buff Modifiers")]
    public float speedModifier;
    public float jumpModifier;

    SimpleClassAbilities SimpleAbilities;
    DynamicClassAbilities DynamicAbilities;

    [Header("General Stats")]
    public float health;
    public float maxHealth;


    //Singleton Structure
    public static PlayerBase instance;

    [Header("Class System")]
    public classType selectedClass = classType.Simple;
    public float superJumpTimer;
    float superJumpTime;
    public string selectedJump;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        defaultSpeed = speed;
        SimpleAbilities = GetComponent<SimpleClassAbilities>();
        DynamicAbilities = GetComponent<DynamicClassAbilities>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GamePause.paused)
        {
            return;
        }
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Vertical");

        isGrounded = Physics.CheckSphere(playerFeetPos.position, detectRadius, whatIsGround);

        moveVector = (transform.forward * moveInput.z * speed) + (transform.right * moveInput.x * speed);
        moveVector *= GamePause.deltaTime;

        
    }

    private void Update()
    {
        if (GamePause.paused)
        {
            return;
        }
        checkForJump();

        //PlayerRotation Based on Mouse Movement

        //transform.Rotate(transform.rotation.x, Input.GetAxis("Mouse X"), transform.rotation.z);

        

        //Punch
        if(health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            health = maxHealth;
        }

        transform.position += moveVector;
    }

    #region Jump

    public void checkForJump()
    {
        //Vertical Movement (Jumps)
        if (Input.GetButtonDown("Jump") && jumps > 0)
        {
            isJumping = true;
            jumpTime = jumpTimer;
            rb.velocity = Vector3.up * jumpForce;
            jumps--;
        }


        else if (Input.GetButtonDown("Jump") && isGrounded && jumps == 0)
        {
            isJumping = true;
            jumpTime = jumpTimer;
            rb.velocity = Vector3.up * jumpForce;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if(jumpTime > 0)
            {
                rb.velocity = Vector3.up * jumpForce;
                jumpTime -= GamePause.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (isGrounded)
        {
            jumps = jumpCount;
        }
        
    }

    #endregion

    

    #region Ability/Class Setting
    public void activateClass(classType selectedClass)
    {
        if(selectedClass == classType.Simple)
        {
            SimpleAbilities.enabled = true;
            DynamicAbilities.enabled = false;
        } else if(selectedClass == classType.Dynamic)
        {
            SimpleAbilities.enabled = false;
            DynamicAbilities.enabled = true;
        }
    }


    public void setAbility(string abilityName)
    {
        if(selectedClass == classType.Simple)
        {
            SimpleAbilities.setAbility(abilityName);
        }
        else
        {
            //Set the dynamic ability
        }
        
        
    }

    #endregion

    #region Value Changes
    public void hurtPlayer(float damage)
    {
        health -= damage;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(playerFeetPos.position, detectRadius);
    }

}
