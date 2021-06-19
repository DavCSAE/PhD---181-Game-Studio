using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    // Event for playing end cutscene
    public delegate void Jump();
    public static event Jump JumpEvent;
    // Function to do end of game cutscene
    // Function is 'static' - accessible from anywhere
    public static void TriggerJumpEvent()
    {
        // Make sure event has subscribers
        JumpEvent?.Invoke();
    }
}
