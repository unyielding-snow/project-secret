using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public override void Enter()
    {
        Debug.Log("Chase State");
        // only want to chase for 5 seconds, so use Time.time
    }

    public override void Do() { }

    public override void Exit() { }
}
