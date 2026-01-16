using UnityEngine;

public class EnemyTwo : MonoBehaviour
{
    [SerializeField] private float speed = 2f;      // Jak rychle se bude hýbat
    [SerializeField] private float distance = 3f;   // Jak daleko od startu vyjede nahoru a dolù

    private Vector3 startPos;

    void Start()
    {
        // Uložíme si startovní pozici, aby objekt vìdìl, kolem èeho kmitat
        startPos = transform.position;
    }

    void Update()
    {
        // Výpoèet nové pozice Y
        // Mathf.Sin vytvoøí vlnu, která jde od -1 do 1
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * distance;

        // Nastavíme novou pozici (X a Z zùstávají stejné)
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}