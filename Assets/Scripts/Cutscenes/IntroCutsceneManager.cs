using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroCutsceneManager : MonoBehaviour
{
    [SerializeField] PlayableDirector startCutscene;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetBlackScreen()
    {
        BlackScreen.Singleton.SetToBlack();
    }

    public void FadeFromBlack()
    {
        BlackScreen.Singleton.FadeFromBlack();
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
}
