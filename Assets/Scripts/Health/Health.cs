using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Nastavení")]
    // ZDE V UNITY NAPIŠ POÈET ŽIVOTÙ (napø. 3 nebo 5)
    // Pokud je tu 0, hráè hned umøe. Zkontroluj to v Inspectoru!
    public float startingHealth; 

    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    
    public Transform respawnPoint;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
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
        
        // Vypnutí pohybu pøi smrti
        if(GetComponent<PlayerMovement>() != null)
            GetComponent<PlayerMovement>().enabled = false;
    }

    public void Respawn()
    {
        currentHealth = startingHealth;
        dead = false;
        
        anim.ResetTrigger("die");
        anim.ResetTrigger("hurt");
        anim.Play("Idle", 0, 0f);

        if(GetComponent<PlayerMovement>() != null)
            GetComponent<PlayerMovement>().enabled = true;

        if (respawnPoint != null)
            transform.position = respawnPoint.position;
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
}