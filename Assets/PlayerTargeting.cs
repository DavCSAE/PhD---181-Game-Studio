using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerTargeting : MonoBehaviour
{
    Player player;

    public Transform target;
    public bool isTargeting;
    public bool isSwappingTarget;
    bool hasSwappedTarget;

    [SerializeField] float maxTargetDistance = 30f;

    [SerializeField] CinemachineTargetGroup targetGroup1;
    [SerializeField] CinemachineTargetGroup targetGroup2;

    [SerializeField] GameObject followTarget;
    [SerializeField] GameObject lookAtTarget;
    [SerializeField] GameObject freeLookCamTarget;


    public CinemachineVirtualCamera targetCam1;
    public CinemachineVirtualCamera targetCam2;
    CinemachineVirtualCamera currentTargetCam;

    bool isCam1Active;
    bool isCam2Active;


    public float targetCamOffsetSpeed = 1f;

    enum Directions
    {
        none,
        left,
        right,
    }

    [SerializeField] Directions offsetDirection;
    [SerializeField] Directions swapDirection;

    public GameObject targetingUI;
    public GameObject targetIcon;

    public Vector3 iconMinScale = Vector3.one * 0.75f;
    public Vector3 iconMaxScale = Vector3.one;
    public Vector3 scaleSpeed = Vector3.one / 3;
    private Vector3 currentScaleSpeed;



    // GIZMOS
    Vector3 start;
    Vector3 end;
    Vector3 obstacleHitPoint;
    bool isDrawingCameraToTargetLineGizmo;

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
        player = GetComponent<Player>();

        currentScaleSpeed = scaleSpeed;

        currentTargetCam = targetCam1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            //ToggleTarget();
        }

        HandleTargetIconPosition();
        HandleTargetIconScaling();

        HandleTargetSwapping();
        HandleTargetCamOffset();

    }


    void HandleTargetIconPosition()
    {
        if (!isTargeting) return;

        Vector2 targetScreenPos = Camera.main.WorldToScreenPoint(target.position);
        targetIcon.transform.position = targetScreenPos;
    }

    void ChangeCurrentTargetCam()
    {

        currentTargetCam.gameObject.SetActive(false);

        // If both cams aren't active
        if (!isCam1Active && !isCam2Active)
        {
            currentTargetCam = targetCam1;
            isCam1Active = true;
            currentTargetCam.gameObject.SetActive(true);

            var transposer = currentTargetCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
            return;
        }
        // If cam 1 is active
        if (isCam1Active)
        {
            currentTargetCam = targetCam2;
            isCam1Active = false;
            isCam2Active = true;
            currentTargetCam.gameObject.SetActive(true);

            var transposer = currentTargetCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
            return;
        }

        // If cam 2 is active
        else if (isCam2Active)
        {
            currentTargetCam = targetCam1;
            isCam2Active = false;
            isCam1Active = true;
            currentTargetCam.gameObject.SetActive(true);

            var transposer = currentTargetCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
            return;
        }


    }

    void HandleTargetCamOffset()
    {
        if (!isTargeting) return;

        // Get movement input
        Vector2 movementInput = InputManager.Singleton.GetMoveInput();
        //print("moveInput: " + movementInput);

        // Get transposer component on target cam
        var transposer = currentTargetCam.GetCinemachineComponent<CinemachineTransposer>();

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

                if (newFollowOffset.x > 1.249f)
                {
                    newFollowOffset.x = 1.25f;
                }
            }
            else if (currentOffset.x >= 1.249f)
            {
                newFollowOffset.x = 1.25f;
            }
            else if (currentOffset.x == 1.25f)
            {
                newFollowOffset.x = 1.25f;
            }

            offsetDirection = Directions.left;
        }

        // If movement is to the right
        else if (movementInput.x > 0)
        {
            if (currentOffset.x > -1.25f)
            {
                newFollowOffset.x = Mathf.Lerp(currentOffset.x, -1.25f, targetCamOffsetSpeed * Time.deltaTime);

                if (newFollowOffset.x < -1.249f)
                {
                    newFollowOffset.x = -1.25f;
                }
            }
            else if (currentOffset.x <= -1.249f)
            {
                newFollowOffset.x = -1.25f;
            }
            else if (currentOffset.x == -1.25f)
            {
                newFollowOffset.x = -1.25f;
            }

            offsetDirection = Directions.right;
        }
        else if(movementInput.x == 0)
        {
            switch(offsetDirection)
            {
                case Directions.left:
                    if (currentOffset.x != 1.25f)
                    {
                        newFollowOffset.x = Mathf.Lerp(currentOffset.x, 1.25f, targetCamOffsetSpeed * 4 * Time.deltaTime);

                        if (newFollowOffset.x > 1.249f)
                        {
                            newFollowOffset.x = 1.25f;
                        }
                    }
                    else
                    {
                        newFollowOffset.x = currentOffset.x;
                    }
                    break;

                case Directions.right:
                    if (currentOffset.x != -1.25f)
                    {
                        newFollowOffset.x = Mathf.Lerp(currentOffset.x, -1.25f, targetCamOffsetSpeed * 4 * Time.deltaTime);

                        if (newFollowOffset.x < -1.249f)
                        {
                            newFollowOffset.x = -1.25f;
                        }
                    }
                    else
                    {
                        newFollowOffset.x = currentOffset.x;
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
        FindTarget();

        // If no target, then return
        if (target == null) return;

        //ChangeCurrentTargetCam();

        targetingUI.SetActive(true);

        //targetCam.Follow = followTarget.transform;

        // Rotate player to look at target
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;

        // Store start look at pos
        Vector3 startLookAtPos = transform.position + transform.forward;

        CalculateTargetCameraStartSide();

        var transposer = currentTargetCam.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;


        // Update state
        isTargeting = true;

        // Prevent swapping right after starting to target
        hasSwappedTarget = true;
    }

    void StopTargeting()
    {
        currentTargetCam.gameObject.SetActive(false);
        targetingUI.SetActive(false);

        //targetCam.Follow = null;
        //targetCam.LookAt = freeLookCamTarget.transform;
        var transposer = currentTargetCam.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;

        // Update state
        isTargeting = false;

        target = null;
    }

    void SetNewTarget(Transform newTarget)
    {
        target = newTarget;

        if (target == null)
        {
            isTargeting = false;
            return;
        }

        

        if (isTargeting)
        {
            ChangeCurrentTargetCam();

            if (isCam1Active)
            {
                targetGroup1.m_Targets[1].target = target;
            }
            else if (isCam2Active)
            {
                targetGroup2.m_Targets[1].target = target;
            }
        }
        else
        {
            isCam1Active = true;
            currentTargetCam = targetCam1;
            targetGroup1.m_Targets[1].target = target;
            targetCam1.gameObject.SetActive(true);
        }

        //isTargeting = true;

        // Start rotating to target
        player.movement.StartRotatingToTarget();
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
            offsetDirection = Directions.right;
        }
        else if (distFromCamToRightPoint > distFromCamToLeftPoint)
        {
            offsetDirection = Directions.left;
        }
        else
        {
            offsetDirection = (Directions)Random.Range(0, 2);
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


    void FindTarget()
    {
        // Declare a new Transform variable that will be used to reference the found target
        Transform newTarget = null;

        // Get a list of all the potential targets
        List<Transform> potentialTargets = new List<Transform>(PlayerTargetManager.Singleton.targets);

        // Prepare a list of doubleCheckTargets
        List<Transform> doubleCheckTargets = new List<Transform>();

        // If there is already a target, remove it from list of potential targets
        if (target) potentialTargets.Remove(target);

        // Set max physical distance
        float physicalDistance = maxTargetDistance;

        // Set max screen distance from centre of screen
        float screenDistance = 999999999f;

        // Get the position of the centre of the creen
        Vector2 screenMidPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        // Current targets screen pos
        Vector2 currentTargetScreenPos = Vector2.zero;

        // If swapping targets, get current targets screen pos
        if (isTargeting && isSwappingTarget)
        {
            currentTargetScreenPos = Camera.main.WorldToScreenPoint(target.position);
        }

        // Check each target in the list of potential targets
        foreach (Transform potentialTarget in potentialTargets)
        {
            // Get potential target's screen position
            Vector2 potentialTargetScreenPos = Camera.main.WorldToScreenPoint(potentialTarget.position);

            // If potential target screen pos is off screen, then skip
            if (CheckIfScreenPositionIsOffScreen(potentialTargetScreenPos)) continue;

            // Get the distance between potential target and player
            float targetDistanceToPlayer = Vector3.Distance(transform.position, potentialTarget.position);

            // Get the distance between potential target screen position and centre of screen
            float targetDistanceToCentreOfScreen = Vector2.Distance(potentialTargetScreenPos, screenMidPoint);

            // If target is not on the screen, then skip target
            if (!potentialTarget.GetComponent<Renderer>().isVisible)
            {
                continue;
            }

            // If target is out of range, then skip target
            if (targetDistanceToPlayer > physicalDistance * 1.2f + 1.5f)
            {
                continue;
            }

            

            // If already targeting
            if (isTargeting && isSwappingTarget)
            {
                // If swapping targets
                if (isSwappingTarget)
                {
                    // Handle swapping based on swap direction
                    switch (swapDirection)
                    {
                        case Directions.left:
                            // If potential target is not in swap direction, then skip
                            if (potentialTargetScreenPos.x > currentTargetScreenPos.x)
                            {
                                continue;
                            }
                            // If potential target is in swap direction
                            else
                            {
                                // Get distance on screen from potential target to current target
                                float screenDistBetweenPotTargetAndCurrTarget = Vector2.Distance(potentialTargetScreenPos, currentTargetScreenPos);

                                // If screen distance between potential and curr targets is smaller than current shortest screen dist
                                if (screenDistBetweenPotTargetAndCurrTarget < screenDistance)
                                {
                                    // If potential target has closer physical distance to player
                                    if (targetDistanceToPlayer < physicalDistance)
                                    {
                                        newTarget = potentialTarget;

                                        physicalDistance = targetDistanceToPlayer;
                                        screenDistance = screenDistBetweenPotTargetAndCurrTarget;
                                    }
                                    // If potential target is slightly further away from player
                                    else if (targetDistanceToPlayer < physicalDistance * 1.2f + 1.5f)
                                    {
                                        newTarget = potentialTarget;

                                        physicalDistance = targetDistanceToPlayer;
                                        screenDistance = screenDistBetweenPotTargetAndCurrTarget;
                                    }
                                    // If potential target is much further away from player
                                    else
                                    {
                                        //if (newTarget) doubleCheckTargets.Add(newTarget);
                                    }
                                }
                                // If potential target screen pos is further away from curr target screen pos than current closest
                                else
                                {
                                    // If potential target has a much closer physical distance to player
                                    if (targetDistanceToPlayer < physicalDistance / 1.5f)
                                    {
                                        newTarget = potentialTarget;

                                        physicalDistance = targetDistanceToPlayer;
                                        screenDistance = screenDistBetweenPotTargetAndCurrTarget;
                                    }
                                }
                            }
                            break;
                        case Directions.right:
                            // If potential target is not in swap direction, then skip
                            if (potentialTargetScreenPos.x < currentTargetScreenPos.x)
                            {
                                continue;
                            }
                            // If potential target is in swap direction
                            else
                            {
                                // Get distance on screen from potential target to current target
                                float screenDistBetweenPotTargetAndCurrTarget = Vector2.Distance(potentialTargetScreenPos, currentTargetScreenPos);

                                // If screen distance between potential and curr targets is smaller than current shortest screen dist
                                if (screenDistBetweenPotTargetAndCurrTarget < screenDistance)
                                {
                                    // If potential target has closer physical distance to player
                                    if (targetDistanceToPlayer < physicalDistance)
                                    {
                                        newTarget = potentialTarget;

                                        physicalDistance = targetDistanceToPlayer;
                                        screenDistance = screenDistBetweenPotTargetAndCurrTarget;
                                    }
                                    // If potential target is slightly further away from player
                                    else if (targetDistanceToPlayer < physicalDistance * 1.2f + 1.5f)
                                    {
                                        newTarget = potentialTarget;

                                        physicalDistance = targetDistanceToPlayer;
                                        screenDistance = screenDistBetweenPotTargetAndCurrTarget;
                                    }
                                    // If potential target is much further away from player
                                    else
                                    {
                                        //if (newTarget) doubleCheckTargets.Add(newTarget);
                                    }
                                }
                                // If potential target screen pos is further away from curr target screen pos than current closest
                                else
                                {
                                    // If potential target has a much closer physical distance to player
                                    if (targetDistanceToPlayer < physicalDistance / 1.5f)
                                    {
                                        newTarget = potentialTarget;

                                        physicalDistance = targetDistanceToPlayer;
                                        screenDistance = screenDistBetweenPotTargetAndCurrTarget;
                                    }
                                }
                            }
                            break;
                    }
                }
            } 
            // If not already targeting (Just started targeting)
            else
            {
                if (CheckIfTargetIsBlocked(potentialTarget))
                {
                    continue;
                }

                // If potential target is closer to centre of screen than current closest
                if (targetDistanceToCentreOfScreen < screenDistance)
                {
                    // If potential target has closer physical distance to player
                    if (targetDistanceToPlayer < physicalDistance)
                    {
                        newTarget = potentialTarget;

                        physicalDistance = targetDistanceToPlayer;
                        screenDistance = targetDistanceToCentreOfScreen;
                    }
                    // If potential target is slightly further away from player
                    else if (targetDistanceToPlayer < physicalDistance * 1.2f + 1.5f)
                    {
                        newTarget = potentialTarget;

                        physicalDistance = targetDistanceToPlayer;
                        screenDistance = targetDistanceToCentreOfScreen;
                    }
                    // If potential target is much further away from player
                    else
                    {
                        //if (newTarget) doubleCheckTargets.Add(newTarget);
                    }
                }
                // If potential target is further away from centre of screen than current closest
                else
                {
                    // If potential target has a much closer physical distance to player
                    if (targetDistanceToPlayer < physicalDistance / 2)
                    {
                        newTarget = potentialTarget;

                        physicalDistance = targetDistanceToPlayer;
                        screenDistance = targetDistanceToCentreOfScreen;
                    }
                }
            }
        }


        if (newTarget)
        {
            print(newTarget.name);
            SetNewTarget(newTarget);
        }

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

        swapDirection = Directions.none;

        // Don't do anything if no targets to swap to
        if (PlayerTargetManager.Singleton.targets.Count <= 1) return;


        // If lookInput is to the left
        if (lookInput.x < 0)
        {
            // Update swapping state
            isSwappingTarget = true;

            // Update Swap Direction
            swapDirection = Directions.left;

            // Try find target to the left
            FindTarget();

        }
        // If lookInput is to the right
        else if (lookInput.x > 0)
        {
            // Update swapping state
            isSwappingTarget = true;

            // Update Swap Direction
            swapDirection = Directions.right;

            // Try find target to the right
            FindTarget();
        }


        // Update states
        hasSwappedTarget = true;
        isSwappingTarget = true;
    }

    bool CheckIfTargetIsBlocked(Transform targetToCheck)
    {
        bool result = false;

        string[] layersToMask = { "Player", "Enemy" };

        LayerMask mask = LayerMask.GetMask(layersToMask);
        mask = ~mask;

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 dirFromCamToTarget = targetToCheck.position - cameraPos;

        float distance = Vector3.Distance(cameraPos, targetToCheck.position);

        RaycastHit hit;



        if (Physics.Raycast(cameraPos, dirFromCamToTarget.normalized, out hit, distance, mask))
        {
            result = true;


            StartDrawingCameraToTargetLineGizmo(cameraPos, cameraPos + dirFromCamToTarget.normalized * distance, hit.point);
        }

        return result;
    }

    bool CheckIfScreenPositionIsOffScreen(Vector2 screenPos)
    {
        bool result = false;

        if (screenPos.x > Screen.width
            || screenPos.x < 0
            || screenPos.y > Screen.height
            || screenPos.y < 0)
        {
            result = true;
        }

        return result;
    }

    void StartDrawingCameraToTargetLineGizmo(Vector3 camTransform, Vector3 targetTransform, Vector3 hitPoint)
    {
        
        start = camTransform;
        end= targetTransform;
        obstacleHitPoint = hitPoint;

        isDrawingCameraToTargetLineGizmo = true;
    }

    void OnDrawGizmos()
    {

        if (isDrawingCameraToTargetLineGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(obstacleHitPoint, 0.25f);
        }
    }


}
