using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class IntroSceneOpeningCutscene : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) Skip();

        if (Gamepad.current.buttonEast.wasPressedThisFrame) Skip();
    }


    public void SetBlackScreen()
    {
        BlackScreen.Singleton.SetToBlack();
    }

    public void FadeFromBlack()
    {
        BlackScreen.Singleton.FadeFromBlack();
    }

    public void SpawnPlayer()
    {
        Player.Singleton.spawning.PrepareToSpawn();
    }

    public void StayPreparedToSpawn()
    {
        Player.Singleton.spawning.StayPreparedToSpawn();
    }

    public void LockCameraControl()
    {
        FreeLookAddOn.Singleton.Lock();
    }

    public void UnlockCameraControl()
    {
        FreeLookAddOn.Singleton.Unlock();
    }

    public void FreezePlayerMovement()
    {
        Player.Singleton.movement.Freeze();
    }

    public void UnfreezePlayerMovement()
    {
        Player.Singleton.movement.Unfreeze();
    }

    void Skip()
    {
        playableDirector.time = 54f;
    }

    public void StartCutsceneMusic()
    {
        print("start music");
        SoundManager.Singleton.Play("Opening Intro");
        FadeInCutsceneMusic();
    }

    public void FadeInCutsceneMusic()
    {
        print("call fade");
        SoundManager.Singleton.StartFadeCoroutine("Opening Intro", 5f, 0.25f);
    }

    public void FadeOutCutsceneMusic()
    {
        //SoundManager.Singleton.StartFadeCoroutine("Opening Intro", 5f, 0f);
    }
}
