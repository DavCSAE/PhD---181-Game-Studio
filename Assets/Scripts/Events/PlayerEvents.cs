using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    // Event for player jump
    public delegate void Jump();
    public static event Jump JumpEvent;

    public static void TriggerJumpEvent()
    {
        // If event has subscribers, run event
        JumpEvent?.Invoke();
    }

    // Event for player dash
    public delegate void Dash();
    public static event Dash DashEvent;

    public static void TriggerDashEvent()
    {
        // If event has subscribers, run event
        DashEvent?.Invoke();
    }

}
