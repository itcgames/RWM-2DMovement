using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStateMachine : StateMachine
{
    [HideInInspector]
    public Runtime2DMovement movement;
    [HideInInspector]
    public Idle idleState;
    public MovementLeft movementLeft;
    public MovementRight movementRight;

    private void Awake()
    {
        idleState = new Idle(this);
        movementLeft = new MovementLeft(this);
        movementRight = new MovementRight(this);
        movement = this.GetComponent<Runtime2DMovement>();
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }
}
