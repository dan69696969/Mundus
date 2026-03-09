using UnityEngine;
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
    }

    public void Respawn()
    {
        currentHealth = startingHealth;
        dead = false;

        anim.ResetTrigger("die");
        anim.ResetTrigger("hurt");
        anim.Play("Idle", 0, 0f);
        if (GetComponent<PlayerMovement>() != null)
            GetComponent<PlayerMovement>().enabled = true;
        if (respawnPoint != null)
            transform.position = respawnPoint.position;
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
}