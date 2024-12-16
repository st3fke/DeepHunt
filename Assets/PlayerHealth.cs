using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth healt;
    private void Awake()
    {
        if (!healt)
            healt = this;
        else if (healt != this)
            Destroy(this);
    }

    public Image heart1, heart2, heart3; // Heart UI images
    public Sprite fullHeart, emptyHeart; // Full and empty heart sprites
    public Animator playerAnimator; // Animator for the player
    public float pushBackForce = 5f; // Force to push the player away
    private int playerHealth = 3; // Player's initial health (3 hearts)
    public bool isHurt = false; // Prevent movement during hurt animation

    void Start()
    {
        UpdateHearts(playerHealth); // Initialize hearts on start
    }

    void Update()
    {
        if (isHurt) return; // Block player input during hurt animation

        // Handle player movement (if applicable)
        // Example: float moveX = Input.GetAxis("Horizontal");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mine"))
        {
            TakeDamage(other.transform.position); // Call TakeDamage and pass the mine's position
            TriggerMineExplosion(other.gameObject); // Handle the mine explosion
        }
    }

    void TakeDamage(Vector2 minePosition)
    {
        if (playerHealth > 0 && !isHurt)
        {
            playerHealth--; // Decrease health
            UpdateHearts(playerHealth); // Update heart UI

            // Trigger player hurt animation
            isHurt = true; // Disable movement during hurt animation
            playerAnimator.SetTrigger("isHurt");

            // Push the player away from the mine
            Vector2 pushDirection = (transform.position - (Vector3)minePosition).normalized;
            GetComponent<Rigidbody2D>().AddForce(pushDirection * pushBackForce, ForceMode2D.Impulse);

            if (playerHealth <= 0)
            {
                StartCoroutine(HandlePlayerDeath()); // Handle player death
            }
            else
            {
                StartCoroutine(ResetHurt()); // Reset isHurt after animation
            }
        }
    }

    void UpdateHearts(int health)
    {
        // Update heart images based on health
        heart1.sprite = (health >= 1) ? fullHeart : emptyHeart;
        heart2.sprite = (health >= 2) ? fullHeart : emptyHeart;
        heart3.sprite = (health >= 3) ? fullHeart : emptyHeart;
    }

    void TriggerMineExplosion(GameObject mine)
    {
        Animator mineAnimator = mine.GetComponent<Animator>();

        if (mineAnimator != null)
        {
            mineAnimator.SetTrigger("Explode"); // Trigger the mine explosion animation
            Destroy(mine, 0.5f); // Destroy the mine after the animation plays
        }
        else
        {
            Destroy(mine); // If no animator is found, destroy the mine immediately
        }
    }

    System.Collections.IEnumerator ResetHurt()
    {
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        isHurt = false; // Re-enable movement after animation ends
    }

    System.Collections.IEnumerator HandlePlayerDeath()
    {
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene("Smrt");
        Destroy(gameObject); // Destroy the player object after the animation finishes
    }
}
