using UnityEngine;

public class PortalEffect : MonoBehaviour
{
    [Header("Nastaven� rotace")]
    [Tooltip("Rychlost, jakou se port�l ot���.")]
    public float rotationSpeed = 20f;

    [Header("Nastaven� pulzov�n�")]
    [Tooltip("Jak moc se m� port�l zv�t�ovat a zmen�ovat (nap�. 0.1 pro 10% zm�nu).")]
    public float scaleAmount = 0.08f;

    [Tooltip("Rychlost pulzuj�c�ho efektu.")]
    public float scaleSpeed = 2f;

    // Ulo��me si p�vodn� velikost port�lu
    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        // Na za��tku si zapamatujeme p�vodn� velikost objektu
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // --- ROTACE ---
        // Ot���me objektem kolem jeho Z osy (ide�ln� pro 2D pohled)
        // Time.deltaTime zaji��uje, �e rotace bude plynul� a nez�visl� na po�tu sn�mk� za sekundu (FPS)
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // --- PULZOV�N� (ZM�NA VELIKOSTI) ---
        // Pou�ijeme sinusovou funkci, kter� vrac� hodnoty mezi -1 a 1 v plynul� vln�.
        // Time.time je �as od spu�t�n� hry, tak�e se hodnota sinusovky neust�le m�n�.
        float scaleFactor = 1.0f + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;

        // Aplikujeme vypo��tanou zm�nu na p�vodn� velikost objektu
        transform.localScale = initialScale * scaleFactor;
    }
}