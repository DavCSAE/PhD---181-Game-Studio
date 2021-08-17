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

    [SerializeField] GameObject maskPickUp;

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

    public void GetMask()
    {
        // Start animation
        Player.Singleton.animations.StartReceiveItemAnim();

        // Rotate player toward camera
        Transform playerTransform = Player.Singleton.transform;
        playerTransform.localEulerAngles = new Vector3(
            playerTransform.localEulerAngles.x,
            324.75f,
            playerTransform.localEulerAngles.z);

        // Show mask in hands
        Player.Singleton.receivables.ShowMask();

        // Hide the mask on the platform
        maskPickUp.SetActive(false);

        // Show receive item UI
        ReceivedItemUI.Singleton.ShowMaskText();

        PlayerEvents.NextDialogueEvent += GottenMask;
    }

    public void GottenMask()
    {
        PlayerEvents.NextDialogueEvent -= GottenMask;

        Player.Singleton.animations.StopReceiveItemAnim();

        //PlayerEvents.TriggerUnlockMaskEvent();

        Player.Singleton.UnlockMask();

        // Hide mask that player is holding in hands
        Player.Singleton.receivables.HideMask();

        // Rotate player toward bird
        Transform playerTransform = Player.Singleton.transform;
        playerTransform.localEulerAngles = new Vector3(
            playerTransform.localEulerAngles.x,
            180,
            playerTransform.localEulerAngles.z);

        //GetDash();
        ExitCutscene();
    }

    void GetDash()
    {
        // Start animation
        Player.Singleton.animations.StartReceiveItemAnim();

        // Unlock Dash
        Player.Singleton.UnlockDash();

        // Show receive item UI
        ReceivedItemUI.Singleton.ShowDashText();

        PlayerEvents.NextDialogueEvent += GottenDash;
    }

    void GottenDash()
    {
        PlayerEvents.NextDialogueEvent -= GottenMask;

        Player.Singleton.animations.StopReceiveItemAnim();

        GetWings();
    }

    void GetWings()
    {
        // Start animation
        Player.Singleton.animations.StartReceiveItemAnim();

        // Unlock Wings
        Player.Singleton.UnlockWings();

        // Show receive item UI
        ReceivedItemUI.Singleton.ShowWingsText();

        PlayerEvents.NextDialogueEvent += GottenWings;
    }

    void GottenWings()
    {
        PlayerEvents.NextDialogueEvent -= GottenWings;

        Player.Singleton.animations.StopReceiveItemAnim();

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
    }

    public void PauseCutscene()
    {
        getMaskCutscene.playableGraph.GetRootPlayable(0).SetSpeed(0);

    }

    public void ContinueCutscene()
    {
        getMaskCutscene.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}
