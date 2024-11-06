using UnityEngine;

// level info class -- offers more flexability for seralizing than structs
// need to set the data in other scripts when the level ends -- inside levelManager
public class LevelInfo : MonoBehaviour{
    public int gems = 0;
    public int stars = 0;
    public int score = 0;
    public float time = 0;
    // probably need this to let players access the proper levels
    public bool locked = true;
}
