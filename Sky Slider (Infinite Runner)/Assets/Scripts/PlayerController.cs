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

    public int Health = 3;

    public bool isDashing;

    public float mx;

    public float dashDistance = 100f;

    private void OnTriggerEnter2D(Collider2D other) {
        Health -= 1;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called for Physics based actions
    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);


        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (!facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput < 0)
        {
            Flip();
        }


    }

    // Update that is called 1 time per Frame
    void Update()
    {
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
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

        if (Health <= 0) {
            SceneManager.LoadScene("Tutorial");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if (moveInput > 0) {
                StartCoroutine(Dash(1f));
            }

            else {
                StartCoroutine(Dash(-1f));
            }
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        CreateDust();
        // Commented out because rock doesn't need to switch directions with sprite
        //Vector3 Scaler = transform.localScale;
        //Scaler.x *= -1;
        //transform.localScale = Scaler;
    }

    void CreateDust()
    {
        dust.Play();
    }

    IEnumerator Dash (float direction) {
        isDashing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.4f);
        isDashing = false;
        rb.gravityScale = gravity;
    }

}
