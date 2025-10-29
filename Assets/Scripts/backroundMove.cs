using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backroundMove : MonoBehaviour
{
    // P�et�hni sem Hr��e nebo Kameru. 
    // Skript bude sledovat tento objekt.
    [SerializeField] private Transform targetToFollow;

    // Jak moc se m� pozad� h�bat.
    // (0, 0) = neh�be se v�bec.
    // (1, 1) = h�be se p�esn� s c�lem (targetToFollow).
    // Pro parallax zkus t�eba (0.5, 0.2)
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    // Pozice c�le v minul�m framu
    private Vector3 lastTargetPosition;

    void Start()
    {
        if (targetToFollow == null)
        {
            // Pokud jsi nic nep�et�hl, zkus�me naj�t hr��e se tagem "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetToFollow = player.transform;
            }
            else
            {
                // Pokud nenajdeme hr��e, vezmeme kameru
                targetToFollow = Camera.main.transform;
                Debug.LogWarning("BackroundMove: C�l (Target) nebyl nastaven, pou��v�m Main Camera.");
            }
        }

        // Ulo��me si startovn� pozici c�le
        lastTargetPosition = targetToFollow.position;
    }

    // Pou��v�me LateUpdate, abychom m�li jistotu, �e se hr��/kamera u� pohnuli
    void LateUpdate()
    {
        // O kolik se c�l posunul od posledn�ho framu
        Vector3 deltaMovement = targetToFollow.position - lastTargetPosition;

        // Vypo��t�me pohyb pozad�
        Vector3 move = new Vector3(
            deltaMovement.x * parallaxEffectMultiplier.x,
            deltaMovement.y * parallaxEffectMultiplier.y,
            0 // Neh�beme v Z ose
        );

        // APLIKUJEME POHYB NA POZAD� (na tento objekt)
        transform.position += move;

        // Ulo�en� pozice c�le pro p��t� frame
        lastTargetPosition = targetToFollow.position;
    }
}