using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{

    [SerializeField] Tutorial tutorial;

    [SerializeField] UnityEvent activationFunction;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            activationFunction.Invoke();

            gameObject.SetActive(false);
        }
    }

}
