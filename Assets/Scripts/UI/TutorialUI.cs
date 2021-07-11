using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Singleton;

    Animator anim;

    private void Awake()
    {
        Singleton = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void ShowPopUp()
    {
        anim.SetBool("isShowing", true);
    }

    public void HidePopUp()
    {
        anim.SetBool("isShowing", false);
    }
}
