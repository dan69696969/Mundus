using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    [SerializeField] Projectile projectilePrefab;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }
    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        //Debug.Log("Player attack");
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x)); //negr

        // Instanicate
        /*Projectile p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        p.transform.parent = null; // very important
        
        float horizontalDirection = Mathf.Sign(transform.localScale.x);
        p.SetDirection(horizontalDirection);
        Vector2 directionVector = new Vector2(horizontalDirection, 0);
        p.GetComponent<Rigidbody2D>().AddForce(directionVector * 10, ForceMode2D.Impulse);*/
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
