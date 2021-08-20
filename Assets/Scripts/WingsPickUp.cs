using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Player>())
        {
            c.GetComponent<Player>().UnlockWings();
            Destroy(gameObject);

            ReceivedItemUI.Singleton.ShowDashText();
        }
    }
}
