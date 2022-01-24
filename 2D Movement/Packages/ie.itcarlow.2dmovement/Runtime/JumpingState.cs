using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BaseState
{
    private MovingStateMachine _sm;
    public JumpingState(MovingStateMachine stateMachine) : base("moving", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        handleJumpInput();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _sm.movement.setRigidBodyVelocity(_sm.movement.getVelocity());
            stateMachine.ChangeState(_sm.movementLeft);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _sm.movement.setRigidBodyVelocity(_sm.movement.getVelocity());
            stateMachine.ChangeState(_sm.movementRight);
        }
        if (Input.GetKey(KeyCode.Space) && _sm.movement.getIsJumping())
        {
            continuousJump();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _sm.movement.setIsJumping(false);
        }
        else if (_sm.movement.getIsGrounded())
        {
            stateMachine.ChangeState(_sm.idleState);
        }
    }

    public void handleJumpInput()
    {
        Vector3 temp = _sm.movement.getRigidBody().velocity;
        temp = Vector2.up * _sm.movement.impluseJumpVel; // Impluse megaman into the air by a set amount.
        temp.x = _sm.movement.getRigidBody().velocity.x;
        _sm.movement.setRigidBodyVelocity(temp);
        _sm.movement.setJumpTimeCounter(_sm.movement.TimeToReachMaxHeight); // reset jumptimecounter.
        _sm.movement.setIsJumping(true);
        _sm.movement.setIsGrounded(false);
    }

    public void continuousJump()
    {
        if (_sm.movement.getJumpTimeCounter() > 0)
        {
            Vector3 temp = _sm.movement.getRigidBody().velocity;
            temp = Vector2.up * _sm.movement.impluseJumpVel * 1.3f;
            temp.x = _sm.movement.getRigidBody().velocity.x;
            _sm.movement.setRigidBodyVelocity(temp);
            _sm.movement.setJumpTimeCounter(_sm.movement.getJumpTimeCounter() - Time.deltaTime);
        }
        else // Else he is falling.
        {
            _sm.movement.setIsJumping(false);
        }
    }
}