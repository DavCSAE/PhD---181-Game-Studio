using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent triggerFunction;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            triggerFunction.Invoke();

            gameObject.SetActive(false);
        }
    }
}
