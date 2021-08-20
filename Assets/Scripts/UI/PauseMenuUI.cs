using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Singleton;

    public bool isOpen = false;

    [SerializeField] GameObject pauseMenu;
    Animator anim;

    private void Awake()
    {
        Singleton = this;
    }

    private void OnEnable()
    {
        PlayerEvents.PauseMenuButtonEvent += TogglePauseMenu;
    }

    private void OnDisable()
    {
        PlayerEvents.PauseMenuButtonEvent -= TogglePauseMenu;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void TogglePauseMenu()
    {
        return;

        if (isOpen == false)
        {
            PrepareToOpenPauseMenu();
        }
        else
        {
            PrepareToClosePauseMenu();
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

        isOpen = true;

        Cursor.visible = true;
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

        isOpen = false;

        Cursor.visible = false;
    }
}
