using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Nastavení efektu")]
    [Range(1.01f, 1.2f)]
    public float hoverMultiplier = 1.05f; // Zvìtšení o 5 %
    public float speed = 15f;            // Rychlost animace

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        // Uložíme si poèáteèní velikost tlaèítka z editoru
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Plynulý pøechod (Lerp)
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cíl je pùvodní velikost * násobitel (napø. 1.05)
        targetScale = originalScale * hoverMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Návrat k pùvodní velikosti
        targetScale = originalScale;
    }
}