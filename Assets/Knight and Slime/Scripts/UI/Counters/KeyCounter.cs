using TMPro;
using UnityEngine;

// for updating the gem counter at the top of the screen for the player
public class KeyCounter : MonoBehaviour
{
    // only have one instance ever in the scene, can get else where but can only set here -- singleton -- if you use don't destroy on load it will last across scenes
    public static KeyCounter instance {get; private set;}
    // get the knight
    private Knight knight;
    private int keysCollected = 0;
    private TMP_Text text;

    private void Awake()
    {
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
        
        // looks for the first object in the hiearchy 
        knight = FindObjectOfType<Knight>();

        text = GetComponent<TMP_Text>();
        text.text = "X" + keysCollected;
    }

    // add gem to the total count of gems collected and change the gem counter in the top of the screen
    public void UpKeyCount(){
        keysCollected++;
        text.text = "X" + keysCollected;
    }

    public void DownKeyCount(){
        // check if there are 0 keys
        if (keysCollected != 0){
            keysCollected--;
            // check if there are now 0 keys
            if (keysCollected == 0){
                // mark that the knight has no keys
                knight.SetHasKey(false);
            }
        }
        
        text.text = "X" + keysCollected;
    }
}
