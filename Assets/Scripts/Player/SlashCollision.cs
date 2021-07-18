using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashCollision : MonoBehaviour
{
    [SerializeField] Slash slash;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Enemy enemy = other.GetComponent<Enemy>();
            slash.HitEnemy(enemy);
            print("hit enemy");
        }

        print("slash hit: " + other.name);
    }
}
