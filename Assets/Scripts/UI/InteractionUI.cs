using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Singleton;

    [SerializeField] InteractionIcons icons;



    Animator anim;

    [SerializeField] GameObject talkPopUp;
    [SerializeField] Image talkIcon;

    [SerializeField] GameObject nextPopUp;
    [SerializeField] Image nextIcon;

    GameObject currentPopUp;

    private void Awake()
    {
        Singleton = this;
    }

    private void OnEnable()
    {
        PlayerEvents.InputDeviceChangeEvent += UpdateIcons;

        PlayerEvents.StartDialogueEvent += ShowNextPopUp;

    }

    private void OnDisable()
    {
        PlayerEvents.InputDeviceChangeEvent -= UpdateIcons;

        PlayerEvents.StartDialogueEvent -= ShowNextPopUp;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTalkPopUp()
    {
        HidePopUp();

        talkPopUp.SetActive(true);
        currentPopUp = talkPopUp;
    }

    public void ShowNextPopUp()
    {
        HidePopUp();

        nextPopUp.SetActive(true);
        currentPopUp = nextPopUp;

    }

    public void HidePopUp()
    {
        if (currentPopUp == null) return;

        currentPopUp.SetActive(false);
        currentPopUp = null;

    }

    void UpdateIcons()
    {
        string currDevice = InputManager.Singleton.GetCurrentDevice();


        if (currDevice == "keyboard")
        {
            talkIcon.sprite = icons.eIcon;
            nextIcon.sprite = icons.eIcon;
        }
        else if (currDevice == "gamepad")
        {
            talkIcon.sprite = icons.northButtonIcon;
            nextIcon.sprite = icons.southButtonIcon;
        }
    }
}
