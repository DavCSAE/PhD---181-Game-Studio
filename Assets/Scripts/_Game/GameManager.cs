using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    bool isMaskUnlocked;
    bool isSwordUnlocked;
    bool isDashUnlocked;
    bool areWingsUnlocked;

    void Awake()
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

    public void PrepareToStartGame()
    {
        Cursor.visible = false;

        BlackScreen.AfterFadeCallback method = StartGame;
        BlackScreen.Singleton.FadeToBlack(method);
    }

    void StartGame()
    {
        InputManager.Singleton.EnableInputs();
        //StartScreenUI.Singleton.CloseStartScreen();

        LevelLoader.Singleton.LoadLevel(1);

        BlackScreen.Singleton.FadeFromBlack();
    }

    public void QuitGame()
    {
        Application.Quit();
    }



    // Unlocking player abilities

    public void UnlockMask()
    {
        isMaskUnlocked = true;
    }

    public void UnlockSword()
    {
        isSwordUnlocked = true;
    }

    public void UnlockDash()
    {
        isDashUnlocked = true;
    }

    public void UnlockWings()
    {
        areWingsUnlocked = true;
    }

    public void LoadPlayerAbilities()
    {
        Player player = Player.Singleton;

        if (isMaskUnlocked) player.UnlockMask();
        if (isSwordUnlocked) player.UnlockSword();
        if (isDashUnlocked) player.UnlockDash();
        if (areWingsUnlocked) player.UnlockWings();
    }
}
