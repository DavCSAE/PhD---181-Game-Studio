using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    Player player;

    [SerializeField] GameObject mask;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject wings;

    private void OnEnable()
    {
        PlayerEvents.UnlockMaskEvent += ShowMask;
    }

    private void OnDisable()
    {
        PlayerEvents.UnlockMaskEvent -= ShowMask;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();

        SetupPlayerAppearance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupPlayerAppearance()
    {
        if (player.isMaskUnlocked) ShowMask();

        if (player.isSwordUnlocked) ShowSword();

        if (player.areWingsUnlocked) ShowWings();
    }

    public void ShowMask()
    {
        mask.SetActive(true);
    }

    public void ShowSword()
    {
        sword.SetActive(true);
    }

    public void ShowWings()
    {
        wings.SetActive(true);
    }
}
