using UnityEngine;

// using a class instead of a struct because it is better on the memory when used inside of large arrays
// in the future will want to add more levels and it's unknown how many there will be 
// handles the stuff for only 1 level, 1 levels data 
public class LevelInfo{
    public int gems = 0;
    public int stars = 0;
    public int score = 0;
    public float time = 0;
    public bool locked = true;

    // helpers
    // toggle the levels lock state
    public void ToggleLock(){
        locked = !locked;
    }

    // getters and setters
}

    
