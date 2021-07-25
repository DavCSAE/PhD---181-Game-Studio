using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    public Renderer renderer;

    private void OnEnable()
    {
        //ActivateTarget();
    }

    private void OnDisable()
    {
        //DeactivateTarget();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTarget()
    {
        PlayerTargetManager.Singleton.AddTarget(this);
    }

    public void DeactivateTarget()
    {

        PlayerTargetManager.Singleton.RemoveTarget(this);
    }
}
