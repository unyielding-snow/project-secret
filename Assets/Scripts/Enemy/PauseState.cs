using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : State
{
    private float elaspedTime = 0;
    public override void Enter()
    {
        Debug.Log("Pause State");
        animator.SetBool("Walk", false);
    }

    public override void Do() 
    {
        elaspedTime += Time.unscaledDeltaTime;

        //if (time >= 3)  // Elasped time == 2 seconds
        //{
        //    isComplete = true;
        //}
    }

    public override void Exit() 
    { 

    }
}
