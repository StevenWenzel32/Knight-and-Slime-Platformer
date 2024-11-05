using UnityEngine;

public class Knight : PlayerBase
{
    // put knight unique code here like abilities, death actions, and any unqiue debuggin messages
    protected override void OnPlayerDeath()
    {
        Debug.Log("Knight died. Starting game over screen");
        // perform default death actions
        base.OnPlayerDeath();
    }
}
