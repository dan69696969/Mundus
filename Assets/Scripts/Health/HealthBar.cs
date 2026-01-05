using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        playerHealth = FindObjectOfType<Health>();
    }

    private void Update()
    {
        if (playerHealth != null)
        {
            // Pøeèteme si, kolik jsi nastavil v Health skriptu
            float maxHealth = playerHealth.startingHealth;

            // Jediná pojistka proti pádu hry (dìlení nulou), pokud bys zapomnìl nastavit životy úplnì
            if (maxHealth <= 0) maxHealth = 3f;

            currenthealthBar.fillAmount = playerHealth.currentHealth / maxHealth;
        }
    }
}