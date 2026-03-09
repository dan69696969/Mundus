using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Zvuky")]
    [SerializeField] private AudioClip attackSound;
    private AudioSource audioSource;

    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        AssignFireballs();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignFireballs();
    }

    private void AssignFireballs()
    {
        GameObject fireballHolder = GameObject.Find("FireballHolder");
        if (fireballHolder != null)
        {
            fireballs = fireballHolder.GetComponentsInChildren<Transform>(true)
                .Where(t => t.gameObject != fireballHolder)
                .Select(t => t.gameObject)
                .ToArray();
            Debug.Log($"[PlayerAttack] ⁄speöne priraden˝ch {fireballs.Length} fireballov zo scÈny.");
        }
        else
        {
            fireballs = new GameObject[0];
            Debug.LogWarning("[PlayerAttack] Objekt 'FireballHolder' sa nenaöiel v aktu·lnej scÈne. ⁄tok nebude fungovaù.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown)
            Attack();
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        if (attackSound != null) audioSource.PlayOneShot(attackSound);

        if (fireballs == null || fireballs.Length == 0)
        {
            Debug.LogWarning("Fireballs nie s˙ priradenÈ, ˙tok nemÙûe prebehn˙ù.");
            return;
        }

        int fireballIndex = FindFireball();
        if (fireballs[fireballIndex] != null)
        {
            GameObject fireball = fireballs[fireballIndex];
            fireball.transform.position = firePoint.position;
            fireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    private int FindFireball()
    {
        if (fireballs == null || fireballs.Length == 0) return 0;
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (fireballs[i] != null && !fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}