using Unity.VisualScripting;
using UnityEngine;

public class Gems : Collectible2D
{
    // score value if wanted
    public float scoreValue;
    // get gem counter
    private GemCounter gemCounter;

    protected override void Awake(){
        base.Awake();
        // looks for the first object in the hiearchy 
        gemCounter = FindObjectOfType<GemCounter>();
    }

    // unique actions when collected
    protected override void Collect() {
        base.Collect();
        // up the gem count
        gemCounter.UpGemCount();
    }
}
