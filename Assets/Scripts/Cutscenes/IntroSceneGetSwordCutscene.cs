using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class IntroSceneGetSwordCutscene : MonoBehaviour
{
    PlayableDirector getSwordCutscene;

    [SerializeField] GameObject virtualCamera;

    [SerializeField] Vector3 playerStartPos;
    [SerializeField] Vector3 playerStartRot;

    [SerializeField] GameObject swordPickUp;

    [SerializeField] EnemySpawner enemySpawner;

    [SerializeField] PortalDoor door;

    // Start is called before the first frame update
    void Start()
    {
        getSwordCutscene = GetComponent<PlayableDirector>();
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
        getSwordCutscene.Play();

        // Fade from black screen
        BlackScreen.Singleton.FadeFromBlack();

        GetSword();
    }

    void SetPlayerStartPos()
    {
        Player.Singleton.transform.position = playerStartPos;
    }

    void SetPlayerStartRot()
    {
        Player.Singleton.transform.localEulerAngles = playerStartRot;
    }

    public void GetSword()
    {
        // Start animation
        Player.Singleton.animations.StartReceiveItemAnim();

        
        // Unlock sword
        Player.Singleton.UnlockSword();

        // Hide the sword on the platform
        swordPickUp.SetActive(false);

        // Show receive item UI
        ReceivedItemUI.Singleton.ShowSwordText();

        PlayerEvents.NextDialogueEvent += GottenSword;
    }

    public void GottenSword()
    {
        PlayerEvents.NextDialogueEvent -= GottenSword;

        Player.Singleton.animations.StopReceiveItemAnim();



        door.Corrupt();
        ExitCutscene();
    }

    void ExitCutscene()
    {
        InputManager.Singleton.UnFreezeMoveInput();
        InputManager.Singleton.UnFreezeJumpInput();
        InputManager.Singleton.UnFreezeAttackInput();

        FreeLookAddOn.Singleton.Unlock();

        // Disable virtual camera
        virtualCamera.SetActive(false);

        enemySpawner.SpawnEnemies();
    }
}
