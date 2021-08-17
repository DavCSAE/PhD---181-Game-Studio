using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Singleton;
    
    void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayerAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPlayerAbilities()
    {
        if (!GameManager.Singleton) return;
        GameManager.Singleton.LoadPlayerAbilities();
    }
}
