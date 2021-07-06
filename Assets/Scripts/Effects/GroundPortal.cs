using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPortal : MonoBehaviour
{
    Animator anim;
    MeshRenderer mr;
    [SerializeField] bool isAnimating;
    [SerializeField] int tile;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePortalAnimation();
    }

    void HandlePortalAnimation()
    {
        if (isAnimating)
        {
            mr.sharedMaterial.SetFloat("_Tile", tile);
        }
    }

    void OpenPortal()
    {
        anim.SetBool("portalActivated", true);
    }

    public void PortalOpened()
    {
        //anim.SetBool("portalActivated", false);
    }
}
