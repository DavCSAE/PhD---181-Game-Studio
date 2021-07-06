using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrepareToSpawn()
    {
        BlackScreen.AfterFadeCallback callback = InitialiseSpawn;
        BlackScreen.Singleton.FadeToBlack(InitialiseSpawn);

        
    }

    void InitialiseSpawn()
    {
        //BlackScreen.Singleton.FadeFromBlack()
    }
}
