using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Predvolená hodnota
    private float direction;
    private bool hit;
    private float maxLifetime = 5f;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;

        // Pohyb Projectilu
        float movementSpeed = speed * Time.deltaTime * direction;
        // Použitie transform.Translate
        // transform.Translate(movementSpeed, 0, 0); 
        // Alebo priama úprava pozície, ktorá je lepšia pre konzistenciu s kódom
        transform.position += new Vector3(speed * Time.deltaTime * direction, 0, 0);


        // Kontrola životnosti
        lifetime += Time.deltaTime;
        if (lifetime > maxLifetime) Deactivate(); // Použijeme Deactivate, namiesto priameho volania SetActive

        // Debug.Log($"Speed: {speed}, Direction: {direction}, Hit: {hit}"); // Zakomentované pre èistú konzolu
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nastavenie hit stavu a spustenie animácie explózie
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }

    public void SetDirection(float _direction)
    {
        // Reset stavu strely pre jej opätovné použitie z poolu
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        // Správne otoèenie grafiky strely
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX; // Otoèí sa iba os X

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

        // Debug.Log("Direction:" + direction); // Zakomentované pre èistú konzolu
    }

    private void Deactivate()
    {
        // Metóda volaná po uplynutí maxLifetime (alebo z animácie 'explode')
        gameObject.SetActive(false);
    }
}