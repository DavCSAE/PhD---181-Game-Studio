using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Player>())
        {
            LevelLoader.Singleton.LoadLevel(2);
        }
    }
}
