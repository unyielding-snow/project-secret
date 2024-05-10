using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    State state;
    WalkState walkState;
    AirState airState;
    IdleState idleState;

    public Rigidbody2D body;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    public Animator animator;

    public float acceleration;
    public float maxSpeed;
    public float groundSpeed;
    public float jumpSpeed;
    public float groundDecay;
    public bool grounded { get; protected set; }
    public float xInput { get; protected set; }
    public float yInput { get; protected set; }

    private void Start()
    {
        //idleState.Setup(body, animator, this);
        //walkState.Setup(body, animator, this);
        //airState.Setup(body, animator, this);
        state = idleState;
    }

    void Update() 
    {
        CheckInput();
        HandleJumpInput();

        //if (state.isComplete)
        //{
            //SelectState();
        //}

        //state.Do();
    }

    void FixedUpdate()  // Handle Physics
    {
        CheckGround();
        ApplyFriction();
        HandleXMovement();
    }
    
    void SelectState()
    {
        if (grounded)
        {
            if(xInput == 0)  // moving horizontally
            {
                state = idleState;
            }
            else
            {
                state = walkState;
            }
        }
        else  // not grounded 
        {
            state = airState;
        }
    }

    void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    void HandleXMovement()
    {
        if (Mathf.Abs(xInput) > 0)
        {
            // Get rid of acceleration to improve preciseness
            //float increment = xInput * acceleration;
            //float newSpeed = Mathf.Clamp(body.velocity.x + increment, -maxSpeed, maxSpeed);  // Limiter clamp
            body.velocity = new Vector2(xInput * maxSpeed, body.velocity.y);

            //FaceDirection();
        }
    }

    void HandleJumpInput()
    {
        if(Input.GetButtonDown("Jump") && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    void FaceDirection()
    {
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void ApplyFriction()
    {
        if (grounded && xInput == 0 && body.velocity.y <= 0)
        {
            body.velocity *= groundDecay;
        }
    }

    public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
    {
        // Maps our starting value from one range to another
        // Used to un-loop animation

        float val = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));

        return clamp ? Mathf.Clamp(val, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : val;
    }

}

