using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDoor : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject portalEffect;
    [SerializeField] Material mat1;
    [SerializeField] Material mat2;


    [SerializeField] bool isColourChanging;
    float startValue = 10.7f;
    float endValue = 36.32f;
    float currentValue = 10.7f;
    [SerializeField] float speed = 1f;

    bool isOpen;
    Collider enterPortalCollider;

    private void OnDisable()
    {
        InitializeMaterials();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        InitializeMaterials();

        enterPortalCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleColourChange();
    }

    void InitializeMaterials()
    {

        mat1.SetFloat("_ChangeAmount", startValue);
        mat2.SetFloat("_ChangeAmount", startValue);
    }


    void StartChangingDoorColour()
    {
        isColourChanging = true;

        currentValue = startValue;
    }

    void HandleColourChange()
    {
        if (!isColourChanging) return;

        currentValue += Time.deltaTime * speed;

        if (currentValue >= endValue)
        {
            currentValue = endValue;

            isColourChanging = false;

            TriggerOpenDoorAnimation();
            
        }

        mat1.SetFloat("_ChangeAmount", currentValue);
        mat2.SetFloat("_ChangeAmount", currentValue);

    }

    void TriggerOpenDoorAnimation()
    {
        anim.SetBool("isDoorOpen", true);
        portalEffect.SetActive(true);

        isOpen = true;
        enterPortalCollider.enabled = true;
    }

    public void Corrupt()
    {
        StartChangingDoorColour();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            LevelLoader.Singleton.LoadLevel(2);
        }
    }
}
