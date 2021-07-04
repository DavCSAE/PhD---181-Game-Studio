using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Singleton;

    [SerializeField] GameObject pauseMenu;
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

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PrepareToOpenPauseMenu();
        }
    }

    public void PrepareToOpenPauseMenu()
    {
        BlackScreen.AfterFadeCallback callback = OpenPauseMenu;
        BlackScreen.Singleton.FadeToBlack(callback);
    }

    void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);

        BlackScreen.Singleton.FadeFromBlack();
    }

    public void PrepareToClosePauseMenu()
    {
        BlackScreen.AfterFadeCallback callback = ClosePauseMenu;
        BlackScreen.Singleton.FadeToBlack(callback);
    }
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);

        BlackScreen.Singleton.FadeFromBlack();
    }
}
