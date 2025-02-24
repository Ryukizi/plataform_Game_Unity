using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float JumpForce = 10f;
    private Rigidbody2D rb;
    private float moveInput;
    private Animator animator;
    private bool isRunning = false;
    private bool isJumping = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        animator.SetBool("isRunning", isRunning);

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
            Debug.Log("Jump");
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        rb.linearVelocity= new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);


        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (Mathf.Abs(rb.linearVelocity.y) > 0.02f)
        {
            isJumping = true;
            animator.SetBool("isJumping", true);
        }
        else
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }

    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        if (hit.collider != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);

            animator.SetBool("isJumping", false);
        }
    }
}