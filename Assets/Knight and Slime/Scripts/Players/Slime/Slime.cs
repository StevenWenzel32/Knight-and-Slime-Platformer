using UnityEngine;

public class Slime : PlayerBase
{
    // has the slime abosorbed a liquid 
    public bool liquidAbsorbed = false;
    // abilities granted by liquids; false by default; only one can be active unless alLiquid has been absorbed
    // derived from honey
    public bool climbWalls = false;
    // derived from super jump -- Need a new liquid name ********
    public bool superJump = false;

    // put Slime unique code here like abilities, death actions, and any unqiue debuggin messages
    protected override void OnPlayerDeath()
    {
        Debug.Log("Slime died. Starting game over screen");
        base.OnPlayerDeath();
    }

// getters and setters and toggles
    public void SetLiquidAbsorbed(bool state){
        liquidAbsorbed = state;
    }

    public bool GetLiquidAbsorbed(){
        return liquidAbsorbed;
    }

    public void SetClimbWalls(bool state){
        climbWalls = state;
    }

    public bool GetClimbWalls(){
        return climbWalls;
    }

    public void SetSuperJump(bool state){
        superJump = state;
    }

    public bool GetSuperJump(){
        return superJump;
    }
}
