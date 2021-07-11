using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int currHealth;

    [SerializeField] SkinnedMeshRenderer bodySMR;
    Material bodyMat;


    bool isFlashingRed;
    float flashRedCurrTime;
    float flashRedTimeLength = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;

        bodyMat = bodySMR.material;
    }

    // Update is called once per frame
    void Update()
    {
        HandleFlashingRed();
    }

    public void TakeDamage(int amount)
    {
        currHealth -= amount;

        if (currHealth <= 0)
        {
            currHealth = 0;

            // Enemy died
            Destroy(gameObject);
        }

        // Make enemy flash red
        FlashRed();

    }

    void FlashRed()
    {
        isFlashingRed = true;
        flashRedCurrTime = 0f;

        bodyMat.SetFloat("_isTakingDamage", 1f);
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

        bodyMat.SetFloat("_isTakingDamage", 0f);
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currHealth;
    }
}
