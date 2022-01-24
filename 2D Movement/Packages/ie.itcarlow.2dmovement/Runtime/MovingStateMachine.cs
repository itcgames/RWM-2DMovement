using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStateMachine : StateMachine
{
    [HideInInspector]
    public Runtime2DMovement movement;
    [HideInInspector]
    public IdleState idleState;
    public WalkingLeftState movementLeft;
    public WalkingRightState movementRight;
    public JumpingState jumping;

    private void Awake()
    {
        idleState = new IdleState(this);
        movementLeft = new WalkingLeftState(this);
        movementRight = new WalkingRightState(this);
        jumping = new JumpingState(this);
        movement = this.GetComponent<Runtime2DMovement>();
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }
}
