using Assets.Scripts.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector3 startingPosition;

    [Header("Object Body")]
    public Rigidbody2D body;
    public Animator animator;

    [Header("Stats")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range = 5f;

    [Header("Hitboxes")]
    [SerializeField] private BoxCollider2D attackRange;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    public bool willMove = false;  // If the enemy will patrol or not

    [Header("State Machine")]
    State state;
    public PatrolState patrolState;
    public PauseState pauseState;
    public ChaseState chaseState;
    public EnemyAttackState attackState;
    protected bool chase = false;

    [SerializeField] private int defaultState = 0;

    void Start()
    {
        startingPosition = transform.position;
        patrolState.Setup(body, animator);
        pauseState.Setup(body, animator);

        if(defaultState == 0)
        {
            state = pauseState;
        }
        else if(defaultState == 1)
        {
            state = patrolState;
        }

        state.Enter();

    }

    void Update()
    {
        if(state.isComplete)
        {
            SelectState();
        }
        state.Do();
    }

    private void SelectState()
    {
        if (PlayerInView())
        {
            state = chaseState;
        }
        else if (PlayerInAttackRange())
        {
            Debug.Log("Enemy Attack!");
            state = attackState;
        }
        else
        {
            state = patrolState;
        }
        state.Enter();
    }

    private bool PlayerInAttackRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(attackRange.bounds.center + -transform.right * range * transform.localScale.x, 
                                             attackRange.bounds.size, 
                                             0, 
                                             Vector2.left,
                                             playerLayer);

        //return hit.collider != null;
        return false;
    }

    private bool PlayerInView()
    {
        if (false)  // See player
        {
            chase = true;
        }

        return false;
    } 

    private void OnDrawGizmos()  // Debug Visualisation
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackRange.bounds.center + -transform.right * range * transform.localScale.x, 
                            attackRange.bounds.size);
    }

}