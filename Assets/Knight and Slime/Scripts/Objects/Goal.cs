using UnityEngine;

public class Goal : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // for animation once made
    //    anim.SetBool("wave", true);
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        // Check for collisions with player 
        if (collision.gameObject.CompareTag("Player"))
        {
            // start the end of level processes
            LevelManager.instance.LevelCompleted();
        }
    }
}
