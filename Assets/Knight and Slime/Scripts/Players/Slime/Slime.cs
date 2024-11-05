using UnityEngine;

public class Slime : PlayerBase
{
    // put Slime unique code here like abilities, death actions, and any unqiue debuggin messages
    protected override void OnPlayerDeath()
    {
        Debug.Log("Slime died. Starting game over screen");
        base.OnPlayerDeath();
    }
}
