using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivedItemUI : MonoBehaviour
{
    public static ReceivedItemUI Singleton;

    [SerializeField] GameObject canvas;

    [SerializeField] GameObject maskText;

    GameObject activeText;

    bool showingUI;

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowMaskText()
    {
        canvas.SetActive(true);

        maskText.SetActive(true);

        activeText = maskText;

        InteractionUI.Singleton.ShowNextPopUp();

        PlayerEvents.NextDialogueEvent += HideAllText;
    }

    public void HideAllText()
    {
        canvas.SetActive(false);

        activeText.SetActive(false);

        InteractionUI.Singleton.HidePopUp();

        PlayerEvents.NextDialogueEvent -= HideAllText;
    }
}
