using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReceivableItems : MonoBehaviour
{
    [SerializeField] GameObject mask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMask()
    {
        mask.SetActive(true);
    }

    public void HideMask()
    {
        mask.SetActive(false);
    }
}
