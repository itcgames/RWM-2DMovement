using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string name;
    protected StateMachine stateMachine;

    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}

public class Idle : BaseState
{
    private MovingStateMachine _sm;

    public Idle(MovingStateMachine stateMachine) : base("Idle", stateMachine)
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
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            stateMachine.ChangeState(_sm.movementRight);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            stateMachine.ChangeState(_sm.movementLeft);
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

public class MovementLeft : BaseState
{
    bool istrue = false;
    private MovingStateMachine _sm;
    public MovementLeft(MovingStateMachine stateMachine) : base("moving", stateMachine)
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
        if (!istrue)
        {
            handleLeftInput();
            istrue = true;
        }
        moveLeft();


        _sm.movement.setTimeSinceLastButtonPress(_sm.movement.getTimeSinceLastButtonPress() + Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _sm.movement.setVelocity(_sm.movement.getRigidBody().velocity);
            _sm.movement.setWalkLeft(false);
            istrue = false;
            stateMachine.ChangeState(_sm.idleState);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            stateMachine.ChangeState(_sm.movementRight);
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

public class MovementRight : BaseState
{
    bool istrue = false;
    private MovingStateMachine _sm;
    public MovementRight(MovingStateMachine stateMachine) : base("moving", stateMachine)
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
        if (!istrue)
        {
            handleRightInput();
            istrue = true;
        }
        moveRight();
        _sm.movement.setTimeSinceLastButtonPress(_sm.movement.getTimeSinceLastButtonPress() + Time.deltaTime);
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            _sm.movement.setVelocity(_sm.movement.getRigidBody().velocity);
            istrue = false;
            stateMachine.ChangeState(_sm.idleState);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            stateMachine.ChangeState(_sm.movementLeft);
        }
    }

    public void handleRightInput()
    {
        if (_sm.movement.getIsMovingLeft())
        {
            _sm.movement.setTimeLeft(_sm.movement._movementTime * 2.0f);
        }
        else
        {
            _sm.movement.setTimeLeft(_sm.movement._movementTime);
            if (_sm.movement.getVelocity().x > 0.0f)
            {
                _sm.movement.setTimeLeft(_sm.movement._MAX_WALKING_SPEED - _sm.movement.getVelocity().x / _sm.movement.acclearation); // t = v - u / a.
            }
        }
        _sm.movement.setWalkRight(true);
        _sm.movement.setWalkLeft(false);
        _sm.movement.setTimeSinceLastButtonPress(0.0f);
    }

    public void moveRight()
    {
        Vector2 temp = _sm.movement.getVelocity();
        if (_sm.movement.getTimeSinceLastButtonPress() < _sm.movement.getTimeLeft())
        {
            temp = _sm.movement.getVelocity();
            temp = _sm.movement.getRigidBody().velocity;
            temp.x = getVel(_sm.movement.getTimeSinceLastButtonPress(), temp.y).x;
            _sm.movement.setRigidBodyVelocity(temp);
            _sm.movement.setVelocity(temp);
            _sm.movement.setRigidBodyVelocity(new Vector2(Mathf.Clamp(temp.x, -_sm.movement._MAX_WALKING_SPEED, _sm.movement._MAX_WALKING_SPEED), temp.y)); // Clamp speed.
        }
        else
        {
            temp = _sm.movement.getRigidBody().velocity;
            temp.x = _sm.movement._MAX_WALKING_SPEED;
            _sm.movement.setRigidBodyVelocity(temp);
            _sm.movement.setVelocity(temp);
        }
    }

    Vector2 getVel(float time, float t_yVel)
    {
        return new Vector3(_sm.movement.acclearation * time, t_yVel, 0.0f); // v = u + at.
    }
}