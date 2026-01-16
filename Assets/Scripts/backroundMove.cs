using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backroundMove : MonoBehaviour
{
    [SerializeField] private Transform targetToFollow;

    [SerializeField] private Vector2 parallaxEffectMultiplier;

    
    private Vector3 lastTargetPosition;

    void Start()
    {
        if (targetToFollow == null)
        {
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetToFollow = player.transform;
            }
            else
            {
               
                targetToFollow = Camera.main.transform;
                Debug.LogWarning("BackroundMove: Cíl (Target) nebyl nastaven, používám Main Camera.");
            }
        }
        lastTargetPosition = targetToFollow.position;
    }
    void LateUpdate()
    {  
        Vector3 deltaMovement = targetToFollow.position - lastTargetPosition;  
        Vector3 move = new Vector3(
            deltaMovement.x * parallaxEffectMultiplier.x,
            deltaMovement.y * parallaxEffectMultiplier.y,
            0 
        );     
        transform.position += move;      
        lastTargetPosition = targetToFollow.position;
    }
}