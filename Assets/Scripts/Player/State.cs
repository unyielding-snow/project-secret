using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public bool isComplete {get; protected set;}

    protected RigidBody2D body;

    // protected Animator animator;

    protected bool grounded;
    protected float xInput;
    protected float yInput;

    public virtual void Enter() { }

    public virtual void Do() { }

    public virtual void FixedDo() { }

    public virtual void Exit() { }


}
