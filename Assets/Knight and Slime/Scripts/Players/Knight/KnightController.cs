using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class KnightController : PlayerControllerBase
{
    // very basic player until knight unique abilties are added in
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // call the base collsion
        base.OnCollisionEnter2D(collision);

        // check if collision with a key object
        
    }
}
