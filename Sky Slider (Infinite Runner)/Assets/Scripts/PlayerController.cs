using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // X Velocity
    public float speed;

    // Y Velocity
    public float jumpForce;

    public float sprintSpeed;

    // Current velocity of player
    private float moveInput;

    // Boolean Of if Facing Right
    private bool facingRight = true;

    // Boolean of if player is on ground
    private bool isGrounded;

    // An empty object that is child of player and stays just below the sprite to check ground
    public Transform groundCheck;

    // Radius of a circle to check around
    public float checkRadius;

    // The player will only be able to jump off this layer, allows for collision with objects in future without being able to jump off them
    public LayerMask whatIsGround;

    // Amount of jumps the player has left to use
    private int extraJumps;

    // Maximum amount of jumps the player can do in the air before falling 
    public int extraJumpsValue;

    public ParticleSystem dust;

    private Rigidbody2D rb;

    public float mx;

    private bool isSprinting = false;

    [SerializeField] private HealthController _healthController;

    public AudioSource source;
    public AudioClip clip;

    private bool isPlaying = false;

    public Animator animator;

    public bool isDead = false;

    public Transform player;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called for Physics based actions
    void FixedUpdate()
    {

        if (isDead) {
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        
        moveInput = Input.GetAxisRaw("Horizontal");

        float currentMoveSpeed = moveInput * speed;

        animator.SetFloat("Speed", Mathf.Abs(currentMoveSpeed));
        if (currentMoveSpeed < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (currentMoveSpeed > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if(!isSprinting)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }

    }

    // Update that is called 1 time per Frame
    void Update()
    {

        if (isDead) {
            if (isPlaying) {
            isPlaying = false;
            source.Stop();
            }
            return;
        }

        if (player.transform.position.y < -35) {
            _healthController.playerHealth = 0;
            _healthController.UpdateHealth();
        }


        if (_healthController.playerHealth <= 1 && !isPlaying)
        {
            source.clip = clip;
            source.Play();
            isPlaying = true;
        }

        if (_healthController.playerHealth >= 2)
        {
            isPlaying = false;
            source.Stop();
        }
        
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            CreateDust();
            isSprinting = true;
            moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * speed * sprintSpeed, rb.velocity.y);
            


        }
        else 
        {
            isSprinting = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            CreateDust();
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded)
        {
            CreateDust();
            rb.velocity = Vector2.up * jumpForce;
        }

        

    }

    void Flip()
    {
        
    }

    void CreateDust()
    {
        dust.Play();
    }


}
