using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State
{
    public AnimationClip anim;
    public override void Enter()
    {
        //animator.Play("Walk");
    }

    public override void Do()
    {
        // aniamtor speed cycle check
        if(!player.grounded || Mathf.Abs(body.velocity.x) < 0.1f )
        {
            isComplete = true; 
        }
    }
}
