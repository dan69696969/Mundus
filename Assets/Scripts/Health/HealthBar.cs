using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
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
            currenthealthBar.fillAmount = playerHealth.currentHealth / playerHealth.startingHealth;

            if (!Application.isPlaying) UpdateVisualScale();
        }
    }

    public void UpdateVisualScale()
    {
        if (playerHealth != null && totalhealthBar != null)
        {
            totalhealthBar.fillAmount = 1f;
        }
    }
}