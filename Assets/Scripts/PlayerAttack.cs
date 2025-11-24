using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Pre sledovanie naèítania scény
using System.Linq; // Pre pouitie .Where(), .Select() a .ToArray()

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f; // Predvolená hodnota, ak nie je nastavená
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private Animator anim;
    // PlayerMovement sa u nepouíva na kontrolu útoku
    private float cooldownTimer = Mathf.Infinity;

    // [SerializeField] Projectile projectilePrefab; // Zakomentované, lebo pouívate object pooling

    private void Awake()
    {
        anim = GetComponent<Animator>();

        // Registrácia udalosti pre automatické priradenie fireballov pri zmene scény
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Okamité priradenie pri štarte, ak PlayerAttack preíva cez scény (DontDestroyOnLoad)
        AssignFireballs();
    }

    private void OnDestroy()
    {
        // Odhlásenie z udalosti
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Voláme metódu na priradenie fireballov po naèítaní scény
        AssignFireballs();
    }

    private void AssignFireballs()
    {
        // Nájdeme FireballHolder v scéne
        GameObject fireballHolder = GameObject.Find("FireballHolder");

        if (fireballHolder != null)
        {
            // Získame VŠETKY Transform potomkov, vrátane neaktívnych (true)
            // Filtrujeme rodièa, vyberieme GameObject a prekonvertujeme na pole
            fireballs = fireballHolder.GetComponentsInChildren<Transform>(true)
                .Where(t => t.gameObject != fireballHolder)
                .Select(t => t.gameObject)
                .ToArray();

            Debug.Log($"[PlayerAttack] Úspešne priradenıch {fireballs.Length} fireballov zo scény.");
        }
        else
        {
            fireballs = new GameObject[0];
            Debug.LogWarning("[PlayerAttack] Objekt 'FireballHolder' sa nenašiel v aktuálnej scéne. Útok nebude fungova.");
        }
    }

    private void Update()
    {
        // Útok je povolenı, ak dríme ¾avé tlaèidlo myši a cooldown vypršal
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown)
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        if (fireballs == null || fireballs.Length == 0)
        {
            Debug.LogWarning("Fireballs nie sú priradené, útok nemôe prebehnú.");
            return;
        }

        int fireballIndex = FindFireball();

        // Ak sa pool vyèerpá (vráti 0), a ten objekt je stále aktívny, prepíšeme ho
        if (fireballs[fireballIndex] != null)
        {
            GameObject fireball = fireballs[fireballIndex];

            // 1. Nastavíme pozíciu
            fireball.transform.position = firePoint.position;

            // 2. Nastavíme smer strely
            // Mathf.Sign(transform.localScale.x) vráti 1 pre smer doprava a -1 pre smer do¾ava
            fireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    private int FindFireball()
    {
        if (fireballs == null || fireballs.Length == 0) return 0;

        // Prejdeme pool a nájdeme neaktívny objekt
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (fireballs[i] != null && !fireballs[i].activeInHierarchy)
                return i;
        }
        // Ak sú všetky aktívne, vrátime index 0 (najstarší fireball bude prepísanı/vrátenı)
        return 0;
    }
}