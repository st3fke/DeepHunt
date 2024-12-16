using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float swimSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    private float dashTime;
    private float dashCooldown = 2f;
    private float lastDashTime = -10f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 originalScale;
    public Collider2D idleCollider;
    public Collider2D swimCollider;

    // Dash cooldown bar
    public Slider dashCooldownBar;

    // Score
    public TMP_Text scoreText;
    private int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        rb.gravityScale = 0f;

        dashCooldownBar.value = 0f;
        score = GameManager.Score; // Učitaj trenutni score iz GameManager-a
        scoreText.text = score.ToString();
    }

    void Update()
    {
        if (isDashing) return;
        if (PlayerHealth.healt.isHurt) return;
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(moveX * moveSpeed, moveY * swimSpeed);

        if (moveX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveX) * originalScale.x, originalScale.y, originalScale.z);
        }

        if (moveX != 0 || moveY != 0)
        {
            if(moveY == 0 || (moveX !=0 && moveY != 0))
            {
                animator.SetBool("isSwimming", true);
            }
            else if(moveY != 0 && moveX == 0)
            {
                animator.SetBool("isSwimming", false);
            }
            idleCollider.enabled = false;
            swimCollider.enabled = true;
        }
        else
        {
            animator.SetBool("isSwimming", false);
            idleCollider.enabled = true;
            swimCollider.enabled = false;
        }

        if (Time.time < lastDashTime + dashCooldown)
        {
            float timeRemaining = Mathf.Max(0, lastDashTime + dashCooldown - Time.time);
            dashCooldownBar.value = 1 - (timeRemaining / dashCooldown);
        }
        else
        {
            dashCooldownBar.value = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash(moveX, moveY);
        }
    }

    void StartDash(float moveX, float moveY)
    {
        isDashing = true;
        dashTime = dashDuration;
        lastDashTime = Time.time;

        animator.SetTrigger("Dash");

        Vector2 dashDirection = new Vector2(moveX, moveY).normalized;

        if (dashDirection == Vector2.zero)
        {
            dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        rb.velocity = dashDirection * dashSpeed;

        InvokeRepeating(nameof(CheckDashCollision), 0f, 0.05f);
        Invoke(nameof(EndDash), dashDuration);
    }

    void CheckDashCollision()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Destroy(hit.gameObject);
                IncreaseScore();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NextLevel") && GameManager.Score == 3)
        {
            GameManager.Score = score; // Sačuvaj trenutni score u GameManager
            SceneManager.LoadScene("DrugiNivo");
        }
        if (other.CompareTag("FinishLevel") && GameManager.Score == 7)
        {
            GameManager.Score = score; // Sačuvaj score u GameManager
            SceneManager.LoadScene("Kraj");
        }
    }

    void EndDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero;
        CancelInvoke(nameof(CheckDashCollision));
    }

    private void IncreaseScore()
    {
        score++;
        GameManager.Score = score; // Ažuriraj globalni score
        scoreText.text = score.ToString();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
