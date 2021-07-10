using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class IntroSceneGetMaskCutscene : MonoBehaviour
{

    PlayableDirector getMaskCutscene;


    [SerializeField] GameObject virtualCamera;

    [SerializeField] Vector3 playerStartPos;
    [SerializeField] Vector3 playerStartRot;

    // Start is called before the first frame update
    void Start()
    {
        getMaskCutscene = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeForCutscene()
    {
        BlackScreen.Singleton.FadeToBlack(StartCutscene, 1f);

        InputManager.Singleton.FreezeMoveInput();
        InputManager.Singleton.FreezeJumpInput();
        InputManager.Singleton.FreezeAttackInput();

        FreeLookAddOn.Singleton.Lock();
    }

    void StartCutscene()
    {
        // Set players position in front of bird
        SetPlayerStartPos();

        // Set players rotation to be facing bird
        SetPlayerStartRot();

        // Enable virtual camera
        virtualCamera.SetActive(true);

        // Play cutscene timeline
        getMaskCutscene.Play();

        // Fade from black screen
        BlackScreen.Singleton.FadeFromBlack();
    }

    void SetPlayerStartPos()
    {
        Player.Singleton.transform.position = playerStartPos;
    }

    void SetPlayerStartRot()
    {
        Player.Singleton.transform.localEulerAngles = playerStartRot;
    }
}
