using Unity.VisualScripting;
using UnityEngine;

public class SuperJump : Liquid
{

    // unique actions when collected
    public override void Absorb(GameObject absorber) {
        base.Absorb(absorber);
        // turn on the slime ability to super jump
        slime.SetSuperJump(true);
        // turn on the super jump
        slime.GetComponent<SlimeController>.SuperJump();
        Debug.Log("Slime superJump state: " + GetComponent<Slime>().GetSuperJump());
    }
}
