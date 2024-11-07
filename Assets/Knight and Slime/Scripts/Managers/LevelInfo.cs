using UnityEngine;

// using a class instead of a struct because it is better on the memory when used inside of large arrays
// in the future will want to add more levels and it's unknown how many there will be 
public class LevelInfo{
    public int gems = 0;
    public int stars = 0;
    public int score = 0;
    public float time = 0;
    // probably need this to let players access the proper levels
    public bool locked = true;
}
