using System.Drawing;
using UnityEngine;

public class Slime : PlayerBase
{
    // private vars
    // how big the slime is - affects other abilities
    private int size = 1;
    // how many items the slime can hold - affected by size
    private int itemLimit;
    // how many items the slime is holding
    private int itemsAbsorbed = 0;
    // has the slime abosorbed a liquid 
    private bool liquidAbsorbed = false;
    // abilities granted by liquids; false by default; only one can be active unless alLiquid has been absorbed
    // derived from honey
    private bool climbWalls = false;
    // derived from super jump -- Need a new liquid name ********
    private bool superJump = false;
    // temp solution for item tracking and return
    // later replace with an array of enums/words of the items absorbed by the slime *****
    private int keyCount = 0;
    
    protected override void Awake()
    {
        base.Awake();
        // set the inital item limit
        UpdateItemLimit();
    }

    // put Slime unique code here like abilities, death actions, and any unqiue debuggin messages
    protected override void OnPlayerDeath()
    {
        Debug.Log("Slime died. Starting game over screen");
        base.OnPlayerDeath();
    }

// getters, setters, counters, checks, and toggles

    // returns true if the slime has space for more items
    public bool CheckItemSpace(){
        if (itemsAbsorbed < itemLimit){
            return true;
        } else {
            return false;
        }
    }

    public void UpKeyCount(){
        keyCount++;
    }
 
    public void DownKeyCount(){
        keyCount--;
    }

    public void UpItemsAbsorbed(){
        itemsAbsorbed++;
    }
 
    public void DownItemsAbsorbed(){
        itemsAbsorbed--;
    }
 
    // might change the math behind the item limit later as the game changes ***
    public void UpdateItemLimit(){
        itemLimit = size;
    }

    public void SetSize(int s){
        size = s;
    }
 
    public int GetSize(){
        return size;
    }

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
