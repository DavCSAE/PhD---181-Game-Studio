using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int currHealth;

    [SerializeField] SkinnedMeshRenderer bodySMR;
    Material playerBodyMat;


    bool isFlashingRed;
    float flashRedCurrTime;
    float flashRedTimeLength = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;

        playerBodyMat = bodySMR.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            TakeDamage(1);
        }

        HandleFlashingRed();
    }

    public void TakeDamage(int amount)
    {
        currHealth -= amount;

        if (currHealth <= 0)
        {
            currHealth = 0;

            // Player died
            Death();
        }

        // Make player flash red
        FlashRed();

        // Update Health UI
        HudUI.Singleton.DamageHeart();
    }

    void FlashRed()
    {
        isFlashingRed = true;
        flashRedCurrTime = 0f;

        playerBodyMat.SetFloat("_isTakingDamage", 1f);
    }

    void HandleFlashingRed()
    {
        if (!isFlashingRed) return;

        flashRedCurrTime += Time.deltaTime;

        if (flashRedCurrTime >= flashRedTimeLength)
        {
            StopFlashingRed();
        }
    }

    void StopFlashingRed()
    {
        isFlashingRed = false;

        playerBodyMat.SetFloat("_isTakingDamage", 0f);
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currHealth;
    }

    void Death()
    {
        PlayerEvents.TriggerPlayerDeathEvent();
    }

}
