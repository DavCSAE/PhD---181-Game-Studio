using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMusicTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            SoundManager.Singleton.Stop("Opening Intro");
            SoundManager.Singleton.Play("Epic");
        }
    }
}
