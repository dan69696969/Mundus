using UnityEngine;

public class PortalEffect : MonoBehaviour
{
    [Header("Nastavení rotace")]
    [Tooltip("Rychlost, jakou se portál otáèí.")]
    public float rotationSpeed = 20f;

    [Header("Nastavení pulzování")]
    [Tooltip("Jak moc se má portál zvìtšovat a zmenšovat (napø. 0.1 pro 10% zmìnu).")]
    public float scaleAmount = 0.08f;

    [Tooltip("Rychlost pulzujícího efektu.")]
    public float scaleSpeed = 2f;

    // Uloíme si pùvodní velikost portálu
    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        // Na zaèátku si zapamatujeme pùvodní velikost objektu
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // --- ROTACE ---
        // Otáèíme objektem kolem jeho Z osy (ideální pro 2D pohled)
        // Time.deltaTime zajišuje, e rotace bude plynulá a nezávislá na poètu snímkù za sekundu (FPS)
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // --- PULZOVÁNÍ (ZMÌNA VELIKOSTI) ---
        // Pouijeme sinusovou funkci, která vrací hodnoty mezi -1 a 1 v plynulé vlnì.
        // Time.time je èas od spuštìní hry, take se hodnota sinusovky neustále mìní.
        float scaleFactor = 1.0f + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;

        // Aplikujeme vypoèítanou zmìnu na pùvodní velikost objektu
        transform.localScale = initialScale * scaleFactor;
    }
}