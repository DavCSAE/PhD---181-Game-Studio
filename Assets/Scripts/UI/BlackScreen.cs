using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    // SELF SINGLETON
    public static BlackScreen Singleton;

    // BlACK SCREEN IMAGE
    [SerializeField]
    Image blackScreen;

    // Timer
    float fadingTimer; // Current timer
    [SerializeField]float fadeLength; // Total length timer should go to
    bool isSpeedChanged;

    // FADING TO
    [SerializeField] bool isFadingTo;

    // FADING FROM
    [SerializeField] bool isFadingFrom;

    // AFTER FADE CALLBACK METHOD
    bool isCallback;

    public delegate void AfterFadeCallback();
    AfterFadeCallback afterFadeCallback;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        // Turn black screen off
        FadeFromBlack();

    }

    private void Update()
    {
        HandleFadingTo();
        HandleFadingFrom();
    }

    public void SetToBlack()
    {
        if (isFadingFrom) isFadingFrom = false;
        if (isFadingTo) isFadingTo = false;

        // Copy colour of black screen
        Color blackScreenCol = blackScreen.color;
        // Adjust alpha of colour
        blackScreenCol.a = 1f;
        // Set black screen to new colour
        blackScreen.color = blackScreenCol;

        blackScreen.gameObject.SetActive(true);
    }

    public void SetToTransparent()
    {
        if (isFadingFrom) isFadingFrom = false;
        if (isFadingTo) isFadingTo = false;

        // Copy colour of black screen
        Color blackScreenCol = blackScreen.color;
        // Adjust alpha of colour
        blackScreenCol.a = 0f;
        // Set black screen to new colour
        blackScreen.color = blackScreenCol;

        blackScreen.gameObject.SetActive(false);
    }

    public void FadeToBlack()
    {
        if (!isFadingFrom) 
        {
            isFadingTo = true;

            blackScreen.gameObject.SetActive(true);
        }
    }

    public void FadeToBlack(float timeInSeconds)
    {
        // Set speed
        fadeLength = timeInSeconds;
        isSpeedChanged = true;

        FadeToBlack();
    }

    public void FadeToBlack(AfterFadeCallback method)
    {
        // Save reference to function to run after fading to black screen
        afterFadeCallback = method;
        isCallback = true;

        FadeToBlack();
    }

    public void FadeToBlack(AfterFadeCallback method, float timeInSeconds)
    {
        // Set speed
        fadeLength = timeInSeconds;
        isSpeedChanged = true;

        FadeToBlack(method);
    }

    void HandleFadingTo()
    {
        // Return if not fading to black
        if (!isFadingTo) { return; }

        // Increase timer
        fadingTimer += Time.deltaTime;

        // If fading finished
        if (fadingTimer >= fadeLength)
        {
            fadingTimer = fadeLength;
            isFadingTo = false;

            FinishedFading();
        }

        // Copy colour of black screen
        Color blackScreenCol = blackScreen.color;
        // Adjust alpha of colour
        blackScreenCol.a = fadingTimer / fadeLength;
        // Set black screen to new colour
        blackScreen.color = blackScreenCol;
    }

    public void FadeFromBlack()
    {
        if (!isFadingTo)
        { 
            isFadingFrom = true;
        }
    }

    public void FadeFromBlack(float timeInSeconds)
    {
        // Set speed
        fadeLength = timeInSeconds;
        isSpeedChanged = true;

        FadeFromBlack();
    }

    void HandleFadingFrom()
    {
        // Return if not fading from black
        if (!isFadingFrom) { return; }

        // Increase timer
        fadingTimer += Time.deltaTime;

        // If fading finished
        if (fadingTimer >= fadeLength)
        {
            fadingTimer = fadeLength;
            isFadingFrom = false;

            blackScreen.gameObject.SetActive(false);

            FinishedFading();
        }

        // Copy colour of black screen
        Color blackScreenCol = blackScreen.color;
        // Adjust alpha of colour
        blackScreenCol.a = (fadeLength - fadingTimer) / fadeLength;
        // Set black screen to new colour
        blackScreen.color = blackScreenCol;
    }

    void FinishedFading()
    {
        if (isFadingFrom) 
        {
            isFadingFrom = false;
        }

        if (isFadingTo) 
        {
            isFadingTo = false;
        }

        // Reset animator speed
        if (isSpeedChanged)
        {
            fadeLength = 1f;
            isSpeedChanged = false;
        }

        // Reset timer
        fadingTimer = 0;

        HandleAfterFadeFunction();
    }

    void HandleAfterFadeFunction()
    {
        // If there is a function to run after fading to black, then run it
        if (isCallback)
        {
            // Run function that is meant to be run after fading to black
            afterFadeCallback();

            // Reset for next fade
            isCallback = false;
        }
    }
}
