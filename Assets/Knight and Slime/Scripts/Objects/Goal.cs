using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Animator anim;
    private bool knightHere = false;
    private bool slimeHere = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // for animation once made
    //    anim.SetBool("wave", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        // grab the tag of the object
        PlayerAtGoal(collision.tag);
        // check if both are present 
        if (knightHere && slimeHere){
            Debug.Log("Both players have reached the goal!");
            // start the end of level processes
            LevelManager.instance.LevelCompleted();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // grab the tag of the object
        PlayerLeftGoal(collision.tag);
    }

    // change the player here status to true
    public void PlayerAtGoal(string playerTag)
    {
        if (playerTag == "Knight")
        {
            Debug.Log("Knight has reached the goal!");
            knightHere = true;
        }
        else if (playerTag == "Slime")
        {
            Debug.Log("Slime has reached the goal!");
            slimeHere = true;
        }
    }

    // change the player here status to false
    public void PlayerLeftGoal(string playerTag)
    {
        if (playerTag == "Knight")
        {
            Debug.Log("Knight has left the goal!");
            knightHere = false;
        }
        else if (playerTag == "Slime")
        {
            Debug.Log("Slime has left the goal!");
            slimeHere = false;
        }
    }
}
