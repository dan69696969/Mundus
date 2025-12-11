using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
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
            //iframes
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

        GetComponent<PlayerMovement>().enabled = false;
    }

    public void Respawn()
    {
        currentHealth = startingHealth;

        GetComponent<PlayerMovement>().enabled = true;

        dead = false;

        anim.ResetTrigger("die");
        anim.ResetTrigger("hurt");

        transform.position = respawnPoint.position;

        anim.Play("Idle", 0, 0f);
    }


    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
}
