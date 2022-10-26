using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayerMove : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCol;
    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private float moveSpeed, jumpF, jumpFAirFactor, jumpT, airSpeedMult, fallMult, lowJumpMult;
    [SerializeField] private float jumpBufferTime = 0.2f, coyoteTime = 0.2f, extraHeightGrounded = 0.1f;
    [SerializeField] private int nJumps = 1;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private ParticleSystem landTakeoffParticles, walkParticles;

    private RaycastHit2D groundHit;
    private Rigidbody2D rb;
    private Animator anim;
    private float inputX;
    private Vector3 startPos;
    private bool grounded = false, coyoteActive = false, prevGrounded = false, canJump = true, faceRight = true, isWalking = false, prevWalking = false;
    private int availableJumps;
    private float bufferJumpT = -1000f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        availableJumps = nJumps;
        startPos = transform.position;
    }

    void Update()
    {
        IsGrounded();
        JumpHandle();
        LateralMovement();
        FlipHandle();

        if (transform.position.y < -7.0f) transform.position = startPos; //Move player to startPos if he falls
    }

    private void LateralMovement()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        if (grounded) rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
        else rb.velocity = new Vector2(inputX * moveSpeed * airSpeedMult, rb.velocity.y);


        prevWalking = isWalking;
        isWalking = Mathf.Abs(inputX) > 0f && grounded;
        if (prevWalking && !isWalking) walkParticles.Stop();
        else if (!prevWalking && isWalking) walkParticles.Play();
        anim.SetFloat("speed", isWalking ? 1 : 0);

    }

    private void JumpHandle()
    {
        bool jumpJustPressed = Input.GetButtonDown("Jump");
        bool jumpPressed = Input.GetButton("Jump");
        if (!grounded && jumpJustPressed) bufferJumpT = Time.time;
        if (jumpJustPressed && (availableJumps > 0 || coyoteActive)) //Jump when pressed
        {
            Jump();
        }
        if (rb.velocity.y < 0) //Increase downwards speed when falling
        {
            rb.velocity -= Vector2.down * Physics2D.gravity.y * fallMult * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpPressed) //Increase fall when going up and not pressing jump button
        {
            rb.velocity -= Vector2.down * Physics2D.gravity.y * lowJumpMult * Time.deltaTime;
        }
    }

    private void Jump()
    {
        landTakeoffParticles.Play();
        StartCoroutine(JumpCoolDown());
        if(grounded) rb.velocity = new Vector2(rb.velocity.x, jumpF);
        else rb.velocity = new Vector2(rb.velocity.x, jumpF * jumpFAirFactor);
    }

    public void IsGrounded()
    {
        prevGrounded = grounded;
        groundHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, extraHeightGrounded, groundMask);
        grounded = rb.velocity.y > 0.1f ? false : groundHit.collider != null;

        if (grounded)
        {
            if (canJump) availableJumps = nJumps;
            if (!prevGrounded) //Land
            {
                if (Time.time - bufferJumpT <= jumpBufferTime) Jump();
            }
        }
        if (!grounded && prevGrounded) //TakeOff
        {
            if (canJump) StartCoroutine(CoyoteTiming());
            landTakeoffParticles.Play();
        }
        
        if (!canJump) grounded = false;
        anim.SetBool("grounded", grounded);
    }

    private void FlipHandle()
    {
        if (faceRight && inputX < 0) Flip();
        else if (!faceRight && inputX > 0) Flip();
    }

    private void Flip()
    {
        faceRight = !faceRight;
        mainSprite.flipX = !faceRight;
    }

    IEnumerator JumpCoolDown()
    {
        availableJumps--;
        canJump = false;
        yield return new WaitForSeconds(jumpT);
        canJump = true;
    }

    IEnumerator CoyoteTiming()
    {
        coyoteActive = true;
        yield return new WaitForSeconds(coyoteTime);
        coyoteActive = false;
        if (availableJumps == nJumps) --availableJumps;
    }
}