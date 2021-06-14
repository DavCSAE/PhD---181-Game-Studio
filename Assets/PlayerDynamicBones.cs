using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDynamicBones : MonoBehaviour
{
    Player player;

    [SerializeField] DynamicBone tailBone;
    [SerializeField] DynamicBone earBoneLeft;
    [SerializeField] DynamicBone earBoneRight;

    bool inert;

    private void Start()
    {
        player = transform.parent.GetComponentInChildren<Player>();
    }

    private void Update()
    {
        HandleInertness();
    }

    void HandleInertness()
    {
        if (!inert && player.movement.onPlatform)
        {
            tailBone.m_Inert = 1f;
            tailBone.UpdateParameters();
            earBoneLeft.m_Inert = 1f;
            earBoneLeft.UpdateParameters();
            earBoneRight.m_Inert = 1f;
            earBoneRight.UpdateParameters();



            inert = true;
        }
        else if (inert && !player.movement.onPlatform)
        {
            tailBone.m_Inert = 0f;
            tailBone.UpdateParameters();
            earBoneLeft.m_Inert = 0f;
            earBoneLeft.UpdateParameters();
            earBoneRight.m_Inert = 0f;
            earBoneRight.UpdateParameters();

            inert = false;
        }
    }
}
