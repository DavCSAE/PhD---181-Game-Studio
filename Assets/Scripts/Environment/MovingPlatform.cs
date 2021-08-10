using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 startPosition;
    [SerializeField] Vector3 endPosition;

    [SerializeField] float timeToTakeMovingFromStartToEnd;

    bool movingToEnd = true;
    float timeOfCurrentMovement = 0f;


    // Start is called before the first frame update
    void Start()
    {
        InitializePlatform();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void InitializePlatform()
    {
        transform.localPosition = startPosition;
    }

    void HandleMovement()
    {
        if (movingToEnd)
        {
            timeOfCurrentMovement += Time.deltaTime;

            if (timeOfCurrentMovement >= timeToTakeMovingFromStartToEnd)
            {
                // Reached end

                transform.localPosition = endPosition;
                timeOfCurrentMovement = 0f;
                movingToEnd = false;
            }
            else
            {
                Vector3 currentPosition = startPosition + ((endPosition - startPosition) * (timeOfCurrentMovement / timeToTakeMovingFromStartToEnd));
                transform.localPosition = currentPosition;
            }
        }
        else
        {
            timeOfCurrentMovement += Time.deltaTime;

            if (timeOfCurrentMovement >= timeToTakeMovingFromStartToEnd)
            {
                // Reached start

                transform.localPosition = startPosition;
                timeOfCurrentMovement = 0f;
                movingToEnd = true;
            }
            else
            {
                Vector3 currentPosition = endPosition + ((startPosition - endPosition) * (timeOfCurrentMovement / timeToTakeMovingFromStartToEnd));
                transform.localPosition = currentPosition;
            }
        }
    }
}
