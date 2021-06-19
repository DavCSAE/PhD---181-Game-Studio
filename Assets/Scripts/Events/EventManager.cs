using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Singleton;

    [HideInInspector]
    public PlayerEvents playerEvents;

    private void Awake()
    {
        Singleton = this;

        playerEvents = GetComponent<PlayerEvents>();
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
