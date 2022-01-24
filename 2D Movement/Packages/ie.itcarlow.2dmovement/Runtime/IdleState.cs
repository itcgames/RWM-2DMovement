using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private MovingStateMachine _sm;

    public IdleState(MovingStateMachine stateMachine) : base("Idle", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _sm.movement.setRigidBodyVelocity(_sm.movement.getVelocity());
            stateMachine.ChangeState(_sm.movementRight);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _sm.movement.setRigidBodyVelocity(_sm.movement.getVelocity());
            stateMachine.ChangeState(_sm.movementLeft);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _sm.movement.getIsGrounded())
        {
            _sm.movement.setIsJumping(true);
            _sm.movement.setIsGrounded(false);
            stateMachine.ChangeState(_sm.jumping);
        }
        else
        {
            stopLeftAndRightMovement();
        }
    }

    public void stopLeftAndRightMovement()
    {
        Vector2 temp = _sm.movement.getVelocity();
        if (_sm.movement.getVelocity().x != 0.0f)
        {
            _sm.movement.setDeclaration(_sm.movement.acclearation / _sm.movement._movementTime * Time.deltaTime); // Declaration = v/t.
            if (_sm.movement.getVelocity().x < 0.0f)
            {
                temp.x += _sm.movement.getDeclaration(); // reduce vel by declaration.
                _sm.movement.setVelocity(temp);

                if (_sm.movement.getVelocity().x >= -_sm.movement._LOWEST_WALKING_SPEED)
                {
                    temp = _sm.movement.getVelocity();
                    temp.x = 0.0f;
                    _sm.movement.setVelocity(temp);
                }
            }
            else if (_sm.movement.getVelocity().x > 0.0f)
            {
                temp = _sm.movement.getVelocity();
                temp.x -= _sm.movement.getDeclaration(); // reduce vel by declaration.
                _sm.movement.setVelocity(temp);

                if (_sm.movement.getVelocity().x <= _sm.movement._LOWEST_WALKING_SPEED)
                {
                    temp = _sm.movement.getVelocity();
                    temp.x = 0.0f;
                    _sm.movement.setVelocity(temp);
                }
            }
            temp.y = _sm.movement.getRigidBody().velocity.y;
            _sm.movement.setRigidBodyVelocity(temp);
            _sm.movement.setVelocity(temp);
        }
    }
}
