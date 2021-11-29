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
    float defaultSpeed;
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
    float defaultJumpForce;
    public float jumpTimer;
    float jumpTime;
    int jumps;
    bool isJumping;

    Vector3 moveVector;

    [Header("Shooting Mechanic")]
    [SerializeField]
    Transform firepoint;
    public GameObject testBullet;
    public float rateOfFire;
    float fireDelay;

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

        transform.Rotate(transform.rotation.x, Input.GetAxis("Mouse X"), transform.rotation.z);

        //Attacks (Shooting)
        if(fireDelay > 0)
        {
            fireDelay -= GamePause.deltaTime;
        }


        if (Input.GetButton("Fire1") && fireDelay <= 0)
        {
            
            GameObject newBullet = Instantiate(testBullet, firepoint.position, firepoint.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = transform.forward * 18;
            fireDelay = rateOfFire;
        }

        //Punch
        if(health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            health = maxHealth;
        }
    }

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

        //Experimental: Super Jump Ability
        if (Input.GetKeyDown(KeyCode.O))
        {
            if(selectedJump == "")
            {
                Debug.Log("NO JUMP ACTIVE");
            } else if(selectedJump == "RocketJump")
            {
                Debug.Log("ROCKET JUMP ACTIVE");
                rb.velocity = Vector3.up * jumpForce * 2.2f;
                speed = speed / 4;
                Invoke("resetSpeed", 2f);
            } else if(selectedJump == "ZeroGravJump")
            {
                rb.useGravity = false;
                jumpForce *= 1.5f;
                Invoke("RestoreGrav", 4f);
            } else if(selectedJump == "Jump")
            {
                rb.velocity = Vector3.up * jumpForce;
            }
        }
    }

    private void LateUpdate()
    {
        //rb.velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.z * speed);
        transform.localPosition += moveVector;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(playerFeetPos.position, detectRadius);
    }

    public void RestoreGrav()
    {
        rb.useGravity = true;
    }

    public void setJump(string jumpName)
    {
        Debug.Log("Setting Jump to " + jumpName);
        selectedJump = jumpName;
    }

    public void hurtPlayer(float damage)
    {
        health -= damage;
    }

    void resetSpeed()
    {
        speed = defaultSpeed;
    }
}
