using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Player player;

    public void OnCollisionEnter(Collision collision)
    {
        if (player.movement.isDashing)
        {
            player.movement.isDashing = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
