using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    // Singleton
    public static HudUI Singleton;

    // HUD UI STUFF
    [SerializeField] List<Image> heartsImages = new List<Image>();
    Image fullHeart;
    Image emptyHeart;

    // Set Singleton
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

    void SetHeartImages()
    {
        
    }
}
