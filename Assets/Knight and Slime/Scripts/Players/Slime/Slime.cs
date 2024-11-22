using UnityEngine;

public class Slime : PlayerBase
{
    // has the slime abosorbed a liquid 
    public bool liquidAbsorbed = false;
    // abilities granted by liquids; false by default; only one can be active unless alLiquid has been absorbed
    // derived from honey
    public bool climbWalls = false;

    // put Slime unique code here like abilities, death actions, and any unqiue debuggin messages
    protected override void OnPlayerDeath()
    {
        Debug.Log("Slime died. Starting game over screen");
        base.OnPlayerDeath();
    }

    public void SetClimbWalls(bool state){
        climbWalls = state;
    }

    public bool GetClimbWalls(){
        return climbWalls;
    }
}
