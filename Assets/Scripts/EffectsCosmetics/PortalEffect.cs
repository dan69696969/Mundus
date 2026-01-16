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
    private Vector3 initialScale;
    void Start()
    {
        initialScale = transform.localScale;
    }
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        float scaleFactor = 1.0f + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
        transform.localScale = initialScale * scaleFactor;
    }
}