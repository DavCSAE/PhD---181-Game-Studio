using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerTargeting : MonoBehaviour
{
    public Transform target;
    public bool isTargeting;
    [SerializeField] GameObject followTarget;
    [SerializeField] GameObject lookAtTarget;
    [SerializeField] GameObject freeLookCamTarget;


    public CinemachineVirtualCamera targetCam;
    bool isTargetCamBlendingOut;
    Vector3 endTargetCamPos;
    Vector3 endTargetCamRot;
    public float targetCamOffsetSpeed = 1f;
    enum OffsetDirections
    {
        left,
        right,
    }

    OffsetDirections offsetDirection;

    public GameObject targetingUI;
    public GameObject targetIcon;

    public Vector3 iconMinScale = Vector3.one * 0.75f;
    public Vector3 iconMaxScale = Vector3.one;
    public Vector3 scaleSpeed = Vector3.one / 3;
    private Vector3 currentScaleSpeed;

    private void OnEnable()
    {
        PlayerEvents.ToggleTargetingEvent += ToggleTarget;
    }

    private void OnDisable()
    {
        PlayerEvents.ToggleTargetingEvent -= ToggleTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScaleSpeed = scaleSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            //ToggleTarget();
        }

        HandleTargeting();
        HandleTargetIconScaling();

        HandleTargetCamOffset();
    }


    void HandleTargeting()
    {
        if (!isTargeting) return;

        Vector2 targetScreenPos = Camera.main.WorldToScreenPoint(target.position);
        targetIcon.transform.position = targetScreenPos;
    }

    void HandleTargetCamOffset()
    {
        if (!isTargeting) return;

        // Get movement input
        Vector2 movementInput = InputManager.Singleton.GetMoveInput();
        //print("moveInput: " + movementInput);

        // Get transposer component on target cam
        var transposer = targetCam.GetCinemachineComponent<CinemachineTransposer>();

        // Get current offset
        Vector3 currentOffset = transposer.m_FollowOffset;

        // Calculate current follow offset based on movement input
        Vector3 newFollowOffset = new Vector3(0, transposer.m_FollowOffset.y, transposer.m_FollowOffset.z);

        // If movement is to the left,
        if (movementInput.x < 0)
        {
            if (currentOffset.x < 1.25f)
            {
                newFollowOffset.x = Mathf.Lerp(currentOffset.x, 1.25f, targetCamOffsetSpeed * Time.deltaTime);

                if (newFollowOffset.x > 1.25f)
                {
                    newFollowOffset.x = 1.25f;
                }
            }
            else if (currentOffset.x >= 1.25f)
            {
                newFollowOffset.x = 1.25f;
            }

            offsetDirection = OffsetDirections.left;
        }

        // If movement is to the right
        else if (movementInput.x > 0)
        {
            if (currentOffset.x > -1.25f)
            {
                newFollowOffset.x = Mathf.Lerp(currentOffset.x, -1.25f, targetCamOffsetSpeed * Time.deltaTime);

                if (newFollowOffset.x < -1.25f)
                {
                    newFollowOffset.x = -1.25f;
                }
            }
            else if (currentOffset.x <= -1.25f)
            {
                newFollowOffset.x = -1.25f;
            }

            offsetDirection = OffsetDirections.right;
        }
        else if(movementInput.x == 0)
        {
            switch(offsetDirection)
            {
                case OffsetDirections.left:
                    if (currentOffset.x != 1.25f)
                    {
                        newFollowOffset.x = Mathf.Lerp(currentOffset.x, 1.25f, targetCamOffsetSpeed * 4 * Time.deltaTime);

                        if (newFollowOffset.x > 1.25f)
                        {
                            newFollowOffset.x = 1.25f;
                        }
                    }
                    break;

                case OffsetDirections.right:
                    if (currentOffset.x != -1.25f)
                    {
                        newFollowOffset.x = Mathf.Lerp(currentOffset.x, -1.25f, targetCamOffsetSpeed * 4 * Time.deltaTime);

                        if (newFollowOffset.x < -1.25f)
                        {
                            newFollowOffset.x = -1.25f;
                        }
                    }
                    break;
            }
        }

        transposer.m_FollowOffset = newFollowOffset;
    }

    void ToggleTarget()
    {
        print("toggle");

        isTargeting = !isTargeting;

        if (isTargeting) StartTargeting();
        else StopTargeting();
    }

    void StartTargeting()
    {
        targetCam.gameObject.SetActive(true);
        targetingUI.SetActive(true);

        //targetCam.Follow = followTarget.transform;
        targetCam.LookAt = lookAtTarget.transform;

        // Rotate player to look at target
        Vector3 targetPos = target.transform.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);

        var transposer = targetCam.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;

        CalculateTargetCameraStartSide();
    }

    void StopTargeting()
    {
        targetCam.gameObject.SetActive(false);
        targetingUI.SetActive(false);

        //targetCam.Follow = null;
        //targetCam.LookAt = freeLookCamTarget.transform;
        var transposer = targetCam.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
    }

    void CalculateTargetCameraStartSide()
    {
        Vector3 rightPoint = transform.position - transform.right;
        Vector3 leftPoint = transform.position + transform.right;

        Vector3 mainCamPos = Camera.main.transform.position;

        float distFromCamToRightPoint = Vector3.Distance(rightPoint, mainCamPos);
        float distFromCamToLeftPoint = Vector3.Distance(leftPoint, mainCamPos);

        if (distFromCamToRightPoint < distFromCamToLeftPoint)
        {
            offsetDirection = OffsetDirections.right;
        }
        else if (distFromCamToRightPoint > distFromCamToLeftPoint)
        {
            offsetDirection = OffsetDirections.left;
        }
        else
        {
            offsetDirection = (OffsetDirections)Random.Range(0, 2);
        }

    }

    void HandleTargetIconScaling()
    {
        if (!isTargeting) return;

        if (targetIcon.transform.localScale.x <= iconMinScale.x)
        {
            currentScaleSpeed *= -1;
            targetIcon.transform.localScale = iconMinScale;
        }
        if (targetIcon.transform.localScale.x >= iconMaxScale.x)
        {
            currentScaleSpeed *= -1;
            targetIcon.transform.localScale = iconMaxScale;
        }

        targetIcon.transform.localScale += currentScaleSpeed * Time.deltaTime;
    }


}
