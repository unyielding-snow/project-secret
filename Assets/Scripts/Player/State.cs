using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    // Intialized from Character
    protected Rigidbody2D body;
    protected Animator animator;
    protected PlayerController player;
    protected EnemyController enemy;
    protected float speed;

    // State Variables
    public bool isComplete {get; protected set;}

    protected float startTime;
    public float time => Time.time - startTime;  // => can't be set, so can be public

    public virtual void Enter() { }

    public virtual void Do() { }

    public virtual void FixedDo() { }

    public virtual void Exit() { }

    public void Setup(Rigidbody2D _body, Animator _animator)
    {
        body = _body;
        animator = _animator;
    }

    public void SetupPlayer(Rigidbody2D _body, Animator _animator, PlayerController _player)
    {
        body = _body;
        animator = _animator;
        player = _player;
    }

    public void SetupEnemy(Rigidbody2D _body, Animator _animator, EnemyController _enemy)
    {
        body = _body;
        animator = _animator;
        enemy = _enemy;
    }
}
