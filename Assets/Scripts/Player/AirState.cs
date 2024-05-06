using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : State
{
    public AnimationClip anim;

    public override void Enter()
    {
        //animator.Play("Jump");
    }

    public override void Do() 
    {
        if (player.grounded)
        {
            isComplete = true;
        }
    }
}
