using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public LayerMask enemyLayer;
    private PolygonCollider2D attackHitbox; 

    // If you are hit while attacking, it shouldn't cancel your attack.
    // We will just play a on hit coroutine.

    public override void Enter()
    {
        Debug.Log("In Attack State");
        startTime = Time.time;

        // Actviate Hitbox 
        animator.SetBool("isAttack", true);
        attackHitbox = GetComponentInChildren<PolygonCollider2D>();

        // Collect all enemies hit
        Collider2D[] hitEnemies ; 

    }

    public override void Do()
    {
        if (time >= 0.3f)
        {
            Exit();
        }
    }


    public override void Exit()
    {
        animator.SetBool("isAttack", false);
        isComplete = true;
    }
}
