using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : PlayerBase
{
    // public vars
    // list for item tracking and return. used for activating and moving items
    public List<Collectible2D> itemsAbsorbed;
    // the slimes inventory UI
    public GameObject inventory;

    // private vars
    // how big the slime is - affects other abilities
    private int size = 1;
    // how many items the slime can hold - affected by size
    private int itemLimit;
    // how many items the slime is holding
    private int numItemsAbsorbed = 0;
    // has the slime abosorbed a liquid 
    private bool liquidAbsorbed = false;
    // abilities granted by liquids; false by default; only one can be active unless alLiquid has been absorbed
    // derived from honey
    private bool climbWalls = false;
    // derived from super jump -- Need a new liquid name ********
    private bool superJump = false;
    
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
        if (numItemsAbsorbed < itemLimit){
            return true;
        } else {
            return false;
        }
    }

    public void UpItemsAbsorbed(){
        numItemsAbsorbed++;
    }
 
    public void DownItemsAbsorbed(){
        numItemsAbsorbed--;
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

    // add the item to the Slime's inventory UI
    public void AddToInventoryUI(Collectible2D item){
        // find the items index number
        int index = itemsAbsorbed.IndexOf(item);
        // ref the inventory slot image
        Image slot = inventory.transform.Find($"Slot {index} Background").Find($"Item").GetComponent<Image>();
        // assign items image
        slot.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
        // copy color
        Color solid = slot.color;
        // make solid
        solid.a = 1;
        // change color 
        slot.color = solid;
    }

    // find an item in the slime's Inventory UI and remove it 
    public void RemoveFromInventoryUI(Collectible2D item){
        // find the items index number
        int index = itemsAbsorbed.IndexOf(item);
        // ref the inventory slot image
        Image slot = inventory.transform.Find($"Slot {index} Background").Find($"Item").GetComponent<Image>();
        // set image to null
        slot.sprite = null;
        // copy color
        Color solid = slot.color;
        // make see through
        solid.a = 0;
        // change color 
        slot.color = solid;
    }
}
