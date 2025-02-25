using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform waypointA;
    public Transform waypointB;
    public float movementSpeed = 2.0f;
    private Animator animator;
    private bool isWalking = false;
    public int enemyHealth = 50;
    public float attackInterval = 1f;
    private Transform currentTarget;
    private Rigidbody2D rb;
    private Vector3 scale;
    private Coroutine attackCoroutine;
    public float fadeDuration = 1f;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentTarget = waypointA;
        scale = transform.localScale;
        Debug.Log("Enemy Health: " + enemyHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ZoneAttack"))
        {
            Debug.Log("Enemy is entry Attack Zone");
        }

        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
        {
            player = other.GetComponentInParent<PlayerController>();
        }
        if (player != null)
        {
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackPlayer(player));
            }
        }else
        {
            Debug.LogWarning("Player Controller not found in object with tag ZoneAttack");
        }

        if (other.CompareTag("AttackZone"))
        {
            Debug.Log("Enemy is Attacked");
            EnemyTakeDamage(10);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("ZoneAttack"))
        {
            Debug.Log("Enemy is exit Attack Zone");
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator AttackPlayer(PlayerController player)
    {
        while (true)
        {
            player.TakeDamage(10);
            animator.SetTrigger("Attack");
            Debug.Log("Enemy is attacking player...");
            yield return new WaitForSeconds(attackInterval);
        }
    }
    private void MoveTowardsTarget ()
    {
        Vector3 curTargetHorizontal = new Vector2(currentTarget.position.x, transform.position.y);
        Vector2 direction = (curTargetHorizontal - transform.position).normalized;

        transform.position += (Vector3)direction * movementSpeed * Time.deltaTime;
        if (Vector2.Distance(curTargetHorizontal, transform.position) <= 0.2f)
        {
            SwitchTarget();
        }
        UpdateAnimation();
    }
    private void SwitchTarget()
    {
        if (currentTarget == waypointA)
        {
            currentTarget = waypointB;
            Flip();
        }
        else
        {
            currentTarget = waypointA;
            transform.localScale = scale;
        }
    }
    private void UpdateAnimation()
    {
        isWalking = Vector2.Distance(currentTarget.position, transform.position) > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }
    private void Flip()
    {
        Vector3 flippedScale = scale;
        flippedScale.x *= -1;
        transform.localScale = flippedScale;
    }

    public void EnemyTakeDamage(int damage)
    {
        enemyHealth -= damage;
        animator.SetBool("InDamage", true);
        Debug.Log($"take damage {damage} + off damage. Player Health acctualy is {enemyHealth}");

        StartCoroutine(ResetDamageAnimation());

        if (enemyHealth <= 0)
        {
            Debug.Log("Enemy is Dead");
            StartCoroutine(FadeOutAndDestroy());
        }
    }
        private IEnumerator ResetDamageAnimation()
        {
        yield return new WaitForSeconds(1f);
        animator.SetBool("InDamage", false);
        }

    private IEnumerator FadeOutAndDestroy()
    {
        float startAlfa = spriteRenderer.color.a;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;  
        while (progress < 1.0f)
        {
            Color tmpColor = spriteRenderer.color;
            spriteRenderer.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlfa, 0, progress));
            progress += rate * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
