using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeIdleAnimations : MonoBehaviour
{
    Animator anim;


    [SerializeField] bool canIdle;
    [SerializeField] bool isSitting;
    [SerializeField] bool isLying;

    [SerializeField] Collider defaultColl;
    [SerializeField] Collider sitColl;
    [SerializeField] Collider lyingColl;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleSitting();
        HandleLying();
    }

    void HandleSitting()
    {
        if (!canIdle) return;

        bool currSitState = anim.GetBool("isSitting");

        if (isSitting && !currSitState)
        {
            anim.SetBool("isSitting", true);

            SetActiveCollider(sitColl);
        }
        else if (!isSitting && currSitState)
        {
            anim.SetBool("isSitting", false);

            SetActiveCollider(defaultColl);
        }
    }

    void HandleLying()
    {
        if (!canIdle) return;

        bool currLyingState = anim.GetBool("isLying");

        if (isLying && !currLyingState)
        {
            anim.SetBool("isLying", true);

            SetActiveCollider(lyingColl);
        }
        else if (!isLying && currLyingState)
        {
            anim.SetBool("isLying", false);

            SetActiveCollider(defaultColl);
        }
    }

    void SetActiveCollider(Collider coll)
    {
        if (defaultColl != coll) defaultColl.enabled = false;
        if (sitColl != coll) sitColl.enabled = false;
        if (lyingColl != coll) lyingColl.enabled = false;

        coll.enabled = true;
    }

    public void StopIdling()
    {
        if (isLying) isLying = false;
        if (isSitting) isSitting = false;

        canIdle = false;
    }

    public void AllowIdling()
    {
        canIdle = true;
    }
}
