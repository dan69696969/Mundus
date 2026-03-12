using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BossMove : MonoBehaviour
{
    [Header("Pohyb a Útok")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float damageToPlayer = 1f; // Nastav v Inspektoru pro každého bosse

    [Header("Nastavení Otáčení")]
    [SerializeField] private float flipThreshold = 0.5f;

    [Header("Odkazy")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject finalPortal;

    [Header("Objekt k aktivaci")]
    [SerializeField] private GameObject objektPoSmrti;

    [Header("Nastavení Hudby")]
    [SerializeField] private AudioSource hudbaLevelu;
    [SerializeField] private AudioSource hudbaPoSmrti;

    [Header("Zvuky")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float attackSoundDelay = 0.1f;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isDead = false;
    private float lastAttackTime;
    private int currentHealth;
    private bool isFacingRight = true;
    private bool hasDealtDamageThisAttack = false;

    [SerializeField] Image Hpbar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;

        if (Hpbar != null) Hpbar.fillAmount = (float)currentHealth / maxHealth;

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
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        HandleFlip();

        if (distanceToPlayer > attackRange)
        {
            ChasePlayer();
            hasDealtDamageThisAttack = false;
        }
        else
        {
            StopMoving();
            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }

        // Zranění hráče během útoku
        if (IsPlayingAnimation("AttackBoss") && !hasDealtDamageThisAttack)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);
            if (distToPlayer <= attackRange)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageToPlayer);
                    hasDealtDamageThisAttack = true;
                }
            }
        }

        if (!IsPlayingAnimation("AttackBoss"))
            hasDealtDamageThisAttack = false;
    }

    private void HandleFlip()
    {
        float xDifference = player.position.x - transform.position.x;
        if (Mathf.Abs(xDifference) < flipThreshold) return;

        if (xDifference < 0 && isFacingRight) Flip();
        else if (xDifference > 0 && !isFacingRight) Flip();
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
        StartCoroutine(PlayAttackSoundDelayed());
    }

    private IEnumerator PlayAttackSoundDelayed()
    {
        yield return new WaitForSeconds(attackSoundDelay);
        if (attackSound != null && audioSource != null)
            audioSource.PlayOneShot(attackSound);
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
            if (hitSound != null && audioSource != null)
                audioSource.PlayOneShot(hitSound);
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (Hpbar != null) Hpbar.fillAmount = (float)currentHealth / maxHealth;

        if (currentHealth <= 0) Die();
        else
        {
            anim.SetTrigger("Hurt");
            StopMoving();
        }
    }

    private void Die()
    {
        isDead = true;

        anim.SetBool("Walk", false);
        anim.SetTrigger("Death");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound);

        if (objektPoSmrti != null)
            objektPoSmrti.SetActive(true);
        else
            Debug.LogError("POZOR! Nemáš přiřazený Objekt Po Smrti (druhého bosse) v Inspektoru!");

        if (finalPortal != null) finalPortal.SetActive(true);

        if (hudbaLevelu != null) hudbaLevelu.Stop();
        if (hudbaPoSmrti != null) hudbaPoSmrti.Play();

        this.enabled = false;
        Destroy(gameObject, 1f);
    }
}