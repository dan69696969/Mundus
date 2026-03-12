using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Nastavení")]
    public float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("Zvuky")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;

    public Transform respawnPoint;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            if (hurtSound != null) audioSource.PlayOneShot(hurtSound);
        }
        else
        {
            if (!dead)
                Die();
        }
    }

    private void Die()
    {
        dead = true;
        anim.SetTrigger("die");
        if (deathSound != null) audioSource.PlayOneShot(deathSound);
        if (GetComponent<PlayerMovement>() != null)
            GetComponent<PlayerMovement>().enabled = false;

        // Restart levelu po 1 vteøinì (aby se stihla animace smrti)
        Invoke("RestartLevel", 1f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Respawn()
    {
        currentHealth = startingHealth;
        dead = false;
        anim.ResetTrigger("die");
        anim.ResetTrigger("hurt");
        anim.Play("Idle", 0, 0f);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (respawnPoint != null)
            transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y, 0f);

        if (GetComponent<PlayerMovement>() != null)
            GetComponent<PlayerMovement>().enabled = true;
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
}