using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // TENTO ØÁDEK zajistí, že se UI mìní i bez zapnutí hry
public class HealthBar : MonoBehaviour
{
    private Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        playerHealth = FindObjectOfType<Health>();
        UpdateVisualScale();
    }

    private void Update()
    {
        if (playerHealth != null && currenthealthBar != null)
        {
            // Pokud jsi u hráèe nastavil napø. 3 srdce, fillAmount bude 0.3
            currenthealthBar.fillAmount = playerHealth.currentHealth / 10f;

            // Zároveò hlídáme, aby i pozadí (šedá srdce) odpovídalo nastavení
            if (!Application.isPlaying) UpdateVisualScale();
        }
    }

    public void UpdateVisualScale()
    {
        if (playerHealth != null && totalhealthBar != null)
        {
            totalhealthBar.fillAmount = playerHealth.startingHealth / 10f;
        }
    }
}