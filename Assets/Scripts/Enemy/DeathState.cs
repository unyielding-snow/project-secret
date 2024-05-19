using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    public override void Enter()
    {
        Debug.Log("Death State");
        // Play Death Animation Stuff
    }

    public override void Do() 
    {
        // Maybe wait a while to play death animation

        Exit();
    }

    public override void Exit() 
    {
        // Global Stats
        GlobalData.AddDeath(enemy.GetEnemyType());

        Destroy(gameObject);
    }
}
