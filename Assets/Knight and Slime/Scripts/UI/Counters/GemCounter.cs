using TMPro;
using UnityEngine;

// for updating the gem counter at the top of the screen for the player
public class GemCounter : MonoBehaviour
{
    // only have one instance ever in the scene, can get else where but can only set here -- singleton -- if you use don't destroy on load it will last across scenes
    public static GemCounter instance {get; private set;}

    // Total Gems in level -- usually 4
    public int gemsToCollect;
    // Gems that have been picked up
    public int gemsCollected;
    private TMP_Text text;

    private void Awake()
    {
        // singleton stuff
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
        }
        
        text = GetComponent<TMP_Text>();
        text.text = 0 + "/" + gemsToCollect;
    }

   // add gem to the total count of gems collected and change the gem counter in the top of the screen
    public void UpGemCount(){
        // update the counter
        gemsCollected++;
        // set the text to the gem count and format it
        text.text = gemsCollected + "/" + gemsToCollect;
        ScoreCounter.instance.UpdateScore();
    }
}
