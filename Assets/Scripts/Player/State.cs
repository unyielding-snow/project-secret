using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class State : MonoBehaviour
{
    public bool isComplete {get; protected set;}
    protected Rigidbody2D body;
    protected Animator animator;
    protected PlayerController player;

    public virtual void Enter() { }

    public virtual void Do() { }

    public virtual void FixedDo() { }

    public virtual void Exit() { }

    public void Setup(Rigidbody2D _body, Animator _animator, PlayerController _player)
    {
        body = _body;
        animator = _animator;
        player = _player;
    }
}
