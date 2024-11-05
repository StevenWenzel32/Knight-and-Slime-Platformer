using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeController : PlayerControllerBase
{
    // Public variables -- the options in unity
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void HandleHorizontalMovement(float horizontalInput)
    {
        base.HandleHorizontalMovement(horizontalInput);
    }

    protected override void TriggerDisableAnimation()
    {

    }

}
