using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [Header("Nastavení pohybu")]
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;

    [Header("Nastavení poškození (Pila)")]
    [SerializeField] private float damage; // Kolik ubere jedno "kousnutí" (napø. 1)
    [SerializeField] private float damageInterval = 0.5f; // Jak èasto pila ubírá život

    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    private float nextDamageTime; // Pomocná promìnná pro èasování

    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        // Klasický pohyb pily tam a zpìt
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            else
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge)
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            else
                movingLeft = true;
        }
    }

    // Když hráè do pily vejde
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DealDamage(collision);
        }
    }

    // Když hráè v pile zùstane (klíèové pro pilu!)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Pokud už uplynul nastavený interval, ublížíme znovu
            if (Time.time >= nextDamageTime)
            {
                DealDamage(collision);
            }
        }
    }

    private void DealDamage(Collider2D collision)
    {
        Health playerHealth = collision.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            // Nastavíme èas, kdy mùže pila ublížit pøíštì
            nextDamageTime = Time.time + damageInterval;
        }
    }
}