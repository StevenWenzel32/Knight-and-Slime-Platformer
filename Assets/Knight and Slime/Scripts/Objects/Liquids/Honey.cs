using Unity.VisualScripting;
using UnityEngine;

public class Honey : Liquid
{
    // unique actions when collected
    public override void Absorb(GameObject absorber) {
        base.Absorb(absorber);
        // turn on the slime ability to climbWalls
        slime.SetClimbWalls(true);
        Debug.Log("Slime climbWall state: " + GetComponent<Slime>().GetClimbWalls());
    }
}
