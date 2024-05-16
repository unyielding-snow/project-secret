using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatrolState : State
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;

    [Header("Enemy")]
    [SerializeField] private Transform enemyTransform;
    private Vector3 initScale;
    public bool movingLeft;
    public float patrolSpeed = 1f;

    private Rigidbody2D rb;

    public override void Enter()
    {
        Debug.Log("Enter Patrol State");
        initScale = enemyTransform.localScale;
        animator.SetBool("Walk", true);
    }

    public override void Do() 
    {
        if(movingLeft)
        {
            //Debug.Log("Patrol State : Do Left");
            if (enemyTransform.position.x >= leftBorder.position.x)
                MoveDirection(-1);
            else
            {
                //anim.SetBool("moving", false);
                movingLeft = !movingLeft; 
            }
        }
        else
        {
            if (enemyTransform.position.x <= rightBorder.position.x)
                MoveDirection(1);
            else
            {
                //anim.SetBool("moving", false);
                movingLeft = !movingLeft;
            }
        }
    }

    public override void Exit() 
    {
        Debug.Log("Exiting Patrol State");
        // TODO: Maybe show alert icon?
    }

    public void MoveDirection(int _direction)
    {
        //anim.SetBool("moving",true);

        // Face Player Direction
        // TODO: Watch how hollow knight does this animation
        enemyTransform.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        // Patrol Left and Right, will be different for other enemies
        // Move in direction
        enemyTransform.position = new Vector3(enemyTransform.position.x + Time.deltaTime * patrolSpeed * _direction, enemyTransform.position.y, enemyTransform.position.x);
    }

}
