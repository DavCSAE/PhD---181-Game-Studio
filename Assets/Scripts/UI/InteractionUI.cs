using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Singleton;

    Animator anim;

    [SerializeField] GameObject talkPopUp;

    GameObject currentPopUp;

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTalkPopUp()
    {
        talkPopUp.SetActive(true);
        currentPopUp = talkPopUp;
    }

    public void HidePopUp()
    {
        currentPopUp.SetActive(false);
        currentPopUp = null;
    }
}
