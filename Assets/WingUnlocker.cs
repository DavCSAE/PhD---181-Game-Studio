using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingUnlocker : MonoBehaviour
{

    [SerializeField] bool unlockOnStart;

    // Start is called before the first frame update
    void Start()
    {
        if (unlockOnStart) GetComponent<Player>().UnlockWings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
