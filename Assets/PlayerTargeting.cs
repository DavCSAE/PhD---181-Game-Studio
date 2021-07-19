using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTargeting : MonoBehaviour
{
    public Transform target;
    public bool isTargeting;

    public GameObject targetCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            ToggleTarget();
        }

        //HandleTargeting();
    }

    void HandleTargeting()
    {
        //if (!isTargeting) return;


    }

    void ToggleTarget()
    {
        isTargeting = !isTargeting;

        if (isTargeting) StartTargeting();
        else StopTargeting();
    }

    void StartTargeting()
    {
        targetCam.SetActive(true);
    }

    void StopTargeting()
    {
        targetCam.SetActive(false);
    }

}
