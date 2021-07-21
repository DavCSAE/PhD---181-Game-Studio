using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerTargeting : MonoBehaviour
{
    public Transform target;
    public bool isTargeting;
    bool hasSwappedTarget;

    [SerializeField] CinemachineTargetGroup targetGroup;

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

        HandleTargetSwapping();
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


        if (!isTargeting) StartTargeting();
        else StopTargeting();
    }

    void StartTargeting()
    {
        // Find target
        Transform newTarget = FindInitialTarget();

        // If no target, then return
        if (newTarget == null) return;

        SetNewTarget(newTarget);

        targetCam.gameObject.SetActive(true);
        targetingUI.SetActive(true);

        //targetCam.Follow = followTarget.transform;

        // Rotate player to look at target
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);

        var transposer = targetCam.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;

        CalculateTargetCameraStartSide();

        // Update state
        isTargeting = true;
    }

    void StopTargeting()
    {
        targetCam.gameObject.SetActive(false);
        targetingUI.SetActive(false);

        //targetCam.Follow = null;
        //targetCam.LookAt = freeLookCamTarget.transform;
        var transposer = targetCam.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;

        // Update state
        isTargeting = false;
    }

    void SetNewTarget(Transform newTarget)
    {
        target = newTarget;


        // Update targetgroup
        targetGroup.m_Targets[1].target = target;

        
        targetCam.LookAt = target;
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

    Transform FindInitialTarget()
    {
        Transform newTarget = null;

        List<Transform> targets = PlayerTargetManager.Singleton.targets;

        float physicalDistance = 999999999f;
        float screenDistance = 999999999f;

        Vector2 screenMidPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        foreach (Transform potentialTarget in targets)
        {
            // Distance between potential target and player
            float targetDistanceToPlayer = Vector3.Distance(transform.position, potentialTarget.position);

            // Distance between potential target and centre of screen
            float targetDistanceToCentreOfScreen = Vector2.Distance(Camera.main.WorldToScreenPoint(potentialTarget.position), screenMidPoint);

            // Check if view blocked

            if (targetDistanceToPlayer < physicalDistance)
            {
                physicalDistance = targetDistanceToPlayer;
                screenDistance = targetDistanceToCentreOfScreen;

                newTarget = potentialTarget;

            }

            bool isPotentialTargetScreenPosCloser = (targetDistanceToCentreOfScreen < screenDistance);
            bool isPotentialTargetPhysicalDistanceCloseToCurrent = targetDistanceToPlayer < physicalDistance * 2f;

            if (isPotentialTargetScreenPosCloser && isPotentialTargetPhysicalDistanceCloseToCurrent)
            {
                physicalDistance = targetDistanceToPlayer;
                screenDistance = targetDistanceToCentreOfScreen;

                newTarget = potentialTarget;
            }
        }

        print(newTarget.name);

        return newTarget;
    }

    void HandleTargetSwapping()
    {
        // Get look Input
        Vector2 lookInput = InputManager.Singleton.GetLookInput();

        // If look input is not large enough, return
        if (Mathf.Abs(lookInput.x) < 0.25f)
        {
            // Reset state if lookInput is small
            if (hasSwappedTarget) hasSwappedTarget = false;
            return;
        }

        // Don't do anything if just swapped (to prevent rapid swapping)
        if (hasSwappedTarget) return;

        // Don't do anything if not targeting
        if (!isTargeting) return;

        // Don't do anything if no targets to swap to
        if (PlayerTargetManager.Singleton.targets.Count <= 1) return;


        // If lookInput is to the left
        if (lookInput.x < 0)
        {
            // Try find target to the left
            FindTargetToLeftOfCurrent();
        }
        // If lookInput is to the right
        else if (lookInput.x > 0)
        {
            // Try find target to the right
            FindTargetToRightOfCurrent();
        }


        // Update state
        hasSwappedTarget = true;
    }

    void FindTargetToLeftOfCurrent()
    {
        print("FIND LEFT TARGET");

        Transform newTarget = null;

        List<Transform> targets = new List<Transform>(PlayerTargetManager.Singleton.targets);
        targets.Remove(target);

        float physicalDistance = 999999999f;
        float screenDistance = 999999999f;

        Vector2 screenMidPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Vector2 currentTargetScreenPos = Camera.main.WorldToScreenPoint(target.position);

        foreach (Transform potentialTarget in targets)
        {
            if (potentialTarget == target) break;

            // Get potential target's screen position
            Vector2 potentialTargetScreenPos = Camera.main.WorldToScreenPoint(potentialTarget.position);

            // Make sure target's screen position is to the left of current target
            if (potentialTargetScreenPos.x > currentTargetScreenPos.x) continue;

            // Distance between potential target and player
            float targetDistanceToPlayer = Vector3.Distance(transform.position, potentialTarget.position);

            // Distance between potential target and centre of screen
            float targetDistanceToCentreOfScreen = Vector2.Distance(potentialTargetScreenPos, screenMidPoint);

            // Check if view blocked

            if (targetDistanceToPlayer < physicalDistance)
            {
                physicalDistance = targetDistanceToPlayer;

                newTarget = potentialTarget;

                print(newTarget.name);
            }
        }

        // If suitable target found
        if (newTarget != null)
        {
            // Set as new target
            SetNewTarget(newTarget);
        }
    }

    void FindTargetToRightOfCurrent()
    {
        print("FIND Right TARGET");

        Transform newTarget = null;

        List<Transform> targets = new List<Transform>(PlayerTargetManager.Singleton.targets);
        targets.Remove(target);

        float physicalDistance = 999999999f;
        float screenDistance = 999999999f;

        Vector2 screenMidPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Vector2 currentTargetScreenPos = Camera.main.WorldToScreenPoint(target.position);

        foreach (Transform potentialTarget in targets)
        {
            if (potentialTarget == target) break;

            // Get potential target's screen position
            Vector2 potentialTargetScreenPos = Camera.main.WorldToScreenPoint(potentialTarget.position);

            // Make sure target's screen position is to the left of current target
            if (potentialTargetScreenPos.x < currentTargetScreenPos.x) continue;

            // Distance between potential target and player
            float targetDistanceToPlayer = Vector3.Distance(transform.position, potentialTarget.position);

            // Distance between potential target and centre of screen
            float targetDistanceToCentreOfScreen = Vector2.Distance(potentialTargetScreenPos, screenMidPoint);

            // Check if view blocked

            if (targetDistanceToPlayer < physicalDistance)
            {
                physicalDistance = targetDistanceToPlayer;

                newTarget = potentialTarget;

                print(newTarget.name);
            }
        }

        // If suitable target found
        if (newTarget != null)
        {
            // Set as new target
            SetNewTarget(newTarget);
        }
    }

    void SwapTarget()
    {

    }


}
