using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingLeftState : BaseState
{
    private MovingStateMachine _sm;
    public WalkingLeftState(MovingStateMachine stateMachine) : base("moving", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        handleLeftInput();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        moveLeft();
        _sm.movement.setTimeSinceLastButtonPress(_sm.movement.getTimeSinceLastButtonPress() + Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _sm.movement.setVelocity(_sm.movement.getRigidBody().velocity);
            stateMachine.ChangeState(_sm.idleState);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            stateMachine.ChangeState(_sm.movementRight);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _sm.movement.getIsGrounded())
        {
            stateMachine.ChangeState(_sm.jumping);
        }
    }

    public void handleLeftInput()
    {
        if (_sm.movement.getIsMovingRight())
        {
            _sm.movement.setTimeLeft(_sm.movement._movementTime * 2.0f);
        }
        else
        {
            _sm.movement.setTimeLeft(_sm.movement._movementTime);
            if (_sm.movement.getVelocity().x < 0.0f)
            {
                _sm.movement.setTimeLeft(_sm.movement._MAX_WALKING_SPEED - _sm.movement.getVelocity().x / _sm.movement.acclearation); // t = v - u / a.
            }
        }
        _sm.movement.setWalkLeft(true);
        _sm.movement.setWalkRight(false);
        _sm.movement.setTimeSinceLastButtonPress(0.0f);
    }

    public void moveLeft()
    {
        Vector2 temp = _sm.movement.getVelocity();
        if (_sm.movement.getTimeSinceLastButtonPress() < _sm.movement.getTimeLeft())
        {
            temp = _sm.movement.getVelocity();
            temp = _sm.movement.getRigidBody().velocity;
            temp.x = -getVel(_sm.movement.getTimeSinceLastButtonPress(), temp.y).x;
            _sm.movement.setRigidBodyVelocity(temp);
            _sm.movement.setVelocity(temp);
            _sm.movement.setRigidBodyVelocity(new Vector2(Mathf.Clamp(temp.x, -_sm.movement._MAX_WALKING_SPEED, _sm.movement._MAX_WALKING_SPEED), temp.y)); // Clamp speed.
        }
        else
        {
            temp = _sm.movement.getRigidBody().velocity;
            temp.x = -_sm.movement._MAX_WALKING_SPEED;
            _sm.movement.setRigidBodyVelocity(temp);
            _sm.movement.setVelocity(temp);
        }
    }

    Vector2 getVel(float time, float t_yVel)
    {
        return new Vector3(_sm.movement.acclearation * time, t_yVel, 0.0f); // v = u + at.
    }
}
