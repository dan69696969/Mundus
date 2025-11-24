using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneSpawn : MonoBehaviour
{
    void Start()
    {
        var player = FindObjectOfType<PlayerMovement>();

        if (player != null) player.transform.position = new Vector2(0, 0);
        else return;
    }

}
