using UnityEngine;

public class Knight : PlayerBase
{
    private bool hasKey = false;

    // put knight unique code here like abilities, death actions, and any unqiue debuggin messages
    protected override void OnPlayerDeath()
    {
        Debug.Log("Knight died. Starting game over screen");
        // perform default death actions
        base.OnPlayerDeath();
    }

    // getters and setters
    public bool GetHasKey(){
        return hasKey;
    }

    public void SetHasKey(bool key){
        hasKey = key;
    }
}
