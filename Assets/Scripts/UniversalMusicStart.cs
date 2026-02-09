using UnityEngine;

// Tento øádek zajistí, že skript bude pøímo u Audio Source
[RequireComponent(typeof(AudioSource))]
public class UniversalMusicStart : MonoBehaviour
{
    [Header("Nastavení startu")]
    public float startTime = 0f; // Tady v Inspektoru napíšeš tu 9

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource.clip != null)
        {
            // Nastavíme èas a pustíme
            audioSource.time = Mathf.Clamp(startTime, 0f, audioSource.clip.length);
            audioSource.Play();
        }
    }
}