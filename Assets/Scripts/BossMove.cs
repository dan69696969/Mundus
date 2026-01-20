using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BossMove : MonoBehaviour
{
    [Header("Pohyb a Útok")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int maxHealth = 5;

    [Header("Nastavení Otáčení")]
    [SerializeField] private float flipThreshold = 0.5f; // Nové: Jak daleko musí být hráč, aby se boss otočil

    [Header("Odkazy")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject finalPortal;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isDead = false;
    private float lastAttackTime;
    private int currentHealth;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Pojistka: Zmrazí rotaci, aby se boss nekutálel
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (finalPortal != null) finalPortal.SetActive(false);
    }

    void Update()
    {
        // Pokud je mrtvý, nic nedělej.
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        HandleFlip();

        if (distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            StopMoving();
            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    private void HandleFlip()
    {
        // --- OPRAVA SEKÁNÍ ---
        // Zjistíme rozdíl v X souřadnicích (vzdálenost vlevo/vpravo)
        float xDifference = player.position.x - transform.position.x;

        // Pokud je hráč moc blízko středu (v rozmezí -0.5 až 0.5), neotáčíme se.
        // Tím zabráníme blikání ze strany na stranu.
        if (Mathf.Abs(xDifference) < flipThreshold) return;

        // Samotné otáčení
        if (xDifference < 0 && isFacingRight)
        {
            Flip();
        }
        else if (xDifference > 0 && !isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void ChasePlayer()
    {
        if (IsPlayingAnimation("AttackBoss") || IsPlayingAnimation("Hurt"))
        {
            StopMoving();
            return;
        }

        anim.SetBool("Walk", true);

        float direction = (player.position.x > transform.position.x) ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    private void StopMoving()
    {
        anim.SetBool("Walk", false);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        anim.SetTrigger("Attack");
    }

    private bool IsPlayingAnimation(string stateName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Projectile"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("Hurt");
            StopMoving();
        }
    }

    private void Die()
    {
        isDead = true;

        // Vypneme logiku pohybu v Animatoru
        anim.SetBool("Walk", false);

        // Spustíme smrt
        anim.SetTrigger("Death");

        // Zastavíme pohyb
        rb.linearVelocity = Vector2.zero;

        // Ignorujeme gravitaci, aby nepropadl
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Vypneme kolizi
        GetComponent<Collider2D>().enabled = false;

        // Vypneme skript
        this.enabled = false;

        if (finalPortal != null) finalPortal.SetActive(true);
        Destroy(gameObject, 1f);
    }
}