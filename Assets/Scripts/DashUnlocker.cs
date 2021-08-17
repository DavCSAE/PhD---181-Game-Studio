using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUnlocker : MonoBehaviour
{
    [SerializeField] bool unlockOnStart;

    // Start is called before the first frame update
    void Start()
    {
        if (unlockOnStart) GetComponent<Player>().UnlockDash();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
