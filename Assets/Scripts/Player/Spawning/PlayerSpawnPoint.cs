using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    Animator anim;

    // Cinemachine Freelook axis positions so that camera
    // is in a good place for spawning
    [SerializeField] float camYAxis;
    [SerializeField] float camXAxis;

    [SerializeField] float playerYRot;

    public void OpenPortal()
    {
        GetComponent<GroundPortal>().OpenPortal();
    }

    public float GetCamYAxis()
    {
        return camYAxis;
    }

    public float GetCamXAxis()
    {
        return camXAxis;
    }

    public float GetPlayerYRot()
    {
        return playerYRot;
    }
}
