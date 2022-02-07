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

    // public float sprintSpeed;

    // Current velocity of player
    private float moveInput;

    // Boolean Of if Facing Right
    private bool facingRight = true;

    // What value to slow down by when in air and not actively moving
    public float airDragMultiplier = 0.975f;

    // Boolean of if player is on ground
    private bool isGrounded;

    // Boolean of if player is against wall
    private bool isTouchingWall;

    // An empty object that is child of player and stays just below the sprite to check ground
    public Transform groundCheck;

    // An empty object that is child of player and raycasts to wall to check if touching the wall
    public Transform wallCheck;

    // Radius of a circle to check around
    public float checkRadius;

    // Length of which to check for wall
    public float wallCheckDistance;

    // Bool of if it is wallsliding
    private bool isWallSliding;

    // Speed at which the player slides down the wall
    public float WallSlideSpeed;

    // The player will only be able to jump off this layer, allows for collision with objects in future without being able to jump off them
    public LayerMask whatIsGround;

    // Amount of jumps the player has left to use
    private int extraJumps;

    // Maximum amount of jumps the player can do in the air before falling 
    public int extraJumpsValue;

    public ParticleSystem dust;

    // Player Rigidbody
    private Rigidbody2D rb;

    public float mx;

    // private bool isSprinting = false;

    [SerializeField] private HealthController _healthController;

    public AudioSource source;
    public AudioClip clip;

    private bool isPlaying = false;

    // Animator for the player
    public Animator animator;

    public bool isDead = false;

    public Transform player;

    private float currentMoveSpeed;

    private bool isDashing;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;
    private float dashTimeLeft;
    private float lastDash = -100f;

    private int direction;

    private bool isJump = false;
    private bool isSprint = false;
    private bool isDash = false;



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
        animatePlayer();
        applyMovement();
    }

    // Update that is called 1 time per Frame
    void Update()
    {
        checkIfWallSliding();

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

        //if (Input.GetButton("Sprint") || Input.GetAxis("Sprint") > 0) {
        //    isSprint = !isSprint;
        //}
        //if (isSprint)
        //{
        //    CreateDust();
        //    isSprinting = true;
        //    moveInput = Input.GetAxisRaw("Horizontal");
        //    rb.velocity = new Vector2(moveInput * speed * sprintSpeed, rb.velocity.y);
        //    isSprint = !isSprint;
        //}

        //else 
        //{
        //    isSprinting = false;
        //}

        if (Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded) {
            isJump = !isJump;
        }

        // if (isJump)
        // {
        //     CreateDust();
        //     rb.velocity = Vector2.up * jumpForce;
        //     extraJumps--;
        // }

        

        if (isJump)
        {
            CreateDust();
            rb.velocity = Vector2.up * jumpForce;
            isJump = false;
        }



        if (Input.GetButtonDown("Dash")) {
            if (Time.time >= lastDash + dashCoolDown) {
                AttemptToDash();
            }
            isDash = !isDash;
        }

        CheckDash();
        

    }

    private void AttemptToDash() {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
    }

    private void CheckDash() {
        if (isDashing) {
            if (dashTimeLeft > 0) {
                //canMove = false;
                //canFlip = false;
                //rb.velocity = new Vector2((speed + dashSpeed) * direction, 0);
                rb.AddForce(new Vector2(dashSpeed, 0f), ForceMode2D.Force);
                Debug.Log(rb.velocity.x);
                dashTimeLeft -= Time.deltaTime;
            }
        }

        if (dashTimeLeft <= 0) { //|| isTouchingWall
            isDashing = false;
            //canMove = true;
            //canFlip = true;
        }
    }

    void applyMovement()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        moveInput = Input.GetAxisRaw("Horizontal");

        currentMoveSpeed = moveInput * speed;

        if (currentMoveSpeed < 0 && facingRight)
        {
            Flip(true);
            direction = -1;
        }
        else if (currentMoveSpeed > 0 && !facingRight)
        {
            Flip(false);
            direction = 1;
        }
        
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);


        if (!isGrounded && !isWallSliding && moveInput == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }

        if (isWallSliding)
        {
            if (rb.velocity.y < -WallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -WallSlideSpeed);
            }
        }
    }

    // Flips the character in the direction of the velocity
    void Flip(bool b)
    {
        if (!isWallSliding)
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
            facingRight = !facingRight;
        }
        
    }

    // Sets the parameters for the animator
    void animatePlayer()
    {
        animator.SetFloat("Speed", Mathf.Abs(currentMoveSpeed));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isWallSliding", isWallSliding);
    }

    void CreateDust()
    {
        dust.Play();
    }

    void checkIfWallSliding()
    {
        if (isTouchingWall && isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;

        }
        else
        {
            isWallSliding = false;
        }
    }


}
