using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    public Renderer renderer;

    private void OnEnable()
    {
        PlayerTargetManager.Singleton.AddTarget(transform);
    }

    private void OnDisable()
    {

        PlayerTargetManager.Singleton.RemoveTarget(transform);
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
