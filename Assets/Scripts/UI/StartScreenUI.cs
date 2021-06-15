using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenUI : MonoBehaviour
{
    public static StartScreenUI Singleton;

    [SerializeField] GameObject startScreen;

    private void Awake()
    {
        Singleton = this;
    }

    public void CloseStartScreen()
    {
        startScreen.SetActive(false);
    }
}
