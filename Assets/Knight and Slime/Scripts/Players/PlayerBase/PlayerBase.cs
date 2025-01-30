using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    // ref the animator to add in animations later
    protected Animator anim;
    // track player status
    public bool alive = true;
    // get the menu manager
    private LevelManager levelManager;

    protected virtual void Awake()
    {
        // looks for the first object in the hiearchy 
        levelManager = FindObjectOfType<LevelManager>();
    }

    // function to kill players
    // works when they go off map or touch an object that kills them
    public virtual void KillPlayer(){
        // start kill process
        if (alive){
            // set to false
            alive = false;

            // play death animation **
            if (anim != null)
            {
                // currently no animation for death
                // animator.SetTrigger("Die");
            }
            //death actions
            OnPlayerDeath();
        }
    }

    // if doing nothing else destory the player object
    protected virtual void OnPlayerDeath(){
        // show game over screen
        levelManager.GameOver();
        // Pause/unpause the game by stoping time
        Time.timeScale = 0;
    }

    public virtual void ResetPlayer(){
        alive = true;
        // reset the position and any other player stats needed -- could be specific for slime to end certain liquid absordeded
        transform.position = Vector3.zero;
    }
}
