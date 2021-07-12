using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathUI : MonoBehaviour
{
    public static PlayerDeathUI Singleton;

    [SerializeField] GameObject menuRoot;

    Animator anim;

    private void OnEnable()
    {
        PlayerEvents.PlayerDeathEvent += FadeForDeathUI;
    }

    private void OnDisable()
    {
        PlayerEvents.PlayerDeathEvent += FadeForDeathUI;
    }

    // Start is called before the first frame update
    void Start()
    {
        Singleton = this;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FadeForDeathUI()
    {
        BlackScreen.Singleton.FadeToBlack(ShowDeathUI);

    }

    void ShowDeathUI()
    {
        menuRoot.SetActive(true);

        anim.SetBool("isShowing", true);
        
    }

    void HideDeathUI()
    {
        menuRoot.SetActive(false);

        anim.SetBool("isShowing", false);
    }
}
