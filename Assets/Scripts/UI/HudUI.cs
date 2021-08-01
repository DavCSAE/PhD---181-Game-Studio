using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    // Singleton
    public static HudUI Singleton;

    int maxHearts;
    int currentHearts;

    [SerializeField] List<Image> heartsImages = new List<Image>();
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    List<Image> activeHearts = new List<Image>();

    Image damagableHeart;

    // Set Singleton
    private void Awake()
    {
        Singleton = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        SetUpHearts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetUpHearts()
    {
        int maxHealth = Player.Singleton.stats.GetMaxHealth();

        for (int i = 0; i < maxHealth; i++)
        {
            activeHearts.Add(heartsImages[i]);
            heartsImages[i].gameObject.SetActive(true);
        }

        damagableHeart = activeHearts[activeHearts.Count - 1];
    }

    public void UpdateHearts()
    {
        int currentHealth = Player.Singleton.stats.GetCurrentHealth();
        int maxHealth = Player.Singleton.stats.GetMaxHealth();

        for (int i = 0; i < activeHearts.Count; i++)
        {
            if (i < currentHealth)
            {
                activeHearts[i].sprite = fullHeart;
            }
            else if (i >= currentHealth && i < maxHealth)
            {
                activeHearts[i].sprite = emptyHeart;
            }
        }

        damagableHeart = activeHearts[currentHealth - 1];
    }

    public void DamageHeart()
    {
        // Set damageable heart image to empty heart
        damagableHeart.sprite = emptyHeart;

        // Return if there are no more hearts (all hearts empty)
        if (damagableHeart == activeHearts[0]) return;

        // Set damageable heart to the next damageable heart (left of current)
        damagableHeart = heartsImages[activeHearts.IndexOf(damagableHeart) - 1];
    }
}
