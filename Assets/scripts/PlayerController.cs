using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float JumpForce = 10f;
    private Rigidbody2D rb;
    private float moveInput;
    private Animator animator;
    private bool isRunning = false;
    private bool isJumping = false;
    public int playerHealth = 100;
    public Slider lifeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("InDamage", false);
        Debug.Log("Player Health: " + playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        float Shift = Input.GetAxisRaw("Fire3");
        if (Shift > 0)
        {
            moveInput *= 2;
        }

        if (moveInput != 0)
        {
            isRunning = true;
            animator.SetBool("isJumping", false);
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

        lifeSlider.value = playerHealth * 0.01f;

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            Debug.Log("Player get powerUp");
            if (playerHealth < 100)
            {
                playerHealth += 10;
            }
            else
            {
               Debug.Log($"Player Health is full. Player Health acctualy is {playerHealth}");
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);


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
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        animator.SetBool("InDamage", true);
        Debug.Log($"take damage {damage} + off damage. Player Health acctualy is {playerHealth}" );

        StartCoroutine(ResetDamageAnimation());

        if (playerHealth <= 0)
        {
           Debug.Log("Player is dead");
            SceneManager.LoadScene(2);
            //Game Over
        }
    }
    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("InDamage", false);
    }
}
