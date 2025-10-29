using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backroundMove : MonoBehaviour
{
    // Pøetáhni sem Hráèe nebo Kameru. 
    // Skript bude sledovat tento objekt.
    [SerializeField] private Transform targetToFollow;

    // Jak moc se má pozadí hýbat.
    // (0, 0) = nehýbe se vùbec.
    // (1, 1) = hýbe se pøesnì s cílem (targetToFollow).
    // Pro parallax zkus tøeba (0.5, 0.2)
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    // Pozice cíle v minulém framu
    private Vector3 lastTargetPosition;

    void Start()
    {
        if (targetToFollow == null)
        {
            // Pokud jsi nic nepøetáhl, zkusíme najít hráèe se tagem "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetToFollow = player.transform;
            }
            else
            {
                // Pokud nenajdeme hráèe, vezmeme kameru
                targetToFollow = Camera.main.transform;
                Debug.LogWarning("BackroundMove: Cíl (Target) nebyl nastaven, používám Main Camera.");
            }
        }

        // Uložíme si startovní pozici cíle
        lastTargetPosition = targetToFollow.position;
    }

    // Používáme LateUpdate, abychom mìli jistotu, že se hráè/kamera už pohnuli
    void LateUpdate()
    {
        // O kolik se cíl posunul od posledního framu
        Vector3 deltaMovement = targetToFollow.position - lastTargetPosition;

        // Vypoèítáme pohyb pozadí
        Vector3 move = new Vector3(
            deltaMovement.x * parallaxEffectMultiplier.x,
            deltaMovement.y * parallaxEffectMultiplier.y,
            0 // Nehýbeme v Z ose
        );

        // APLIKUJEME POHYB NA POZADÍ (na tento objekt)
        transform.position += move;

        // Uložení pozice cíle pro pøíští frame
        lastTargetPosition = targetToFollow.position;
    }
}