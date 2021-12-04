﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runtime2DMovement : MonoBehaviour
{
    // BOOLS USED FOR MOVEMENT, WILL BE CHANGED FOR FSM
    private bool _moveRight;
    private bool _moveLeft;
    private bool _isJumping;
    private bool _isGrounded;
    private bool _stopMovement = false;
    // BOOLS USED FOR MOVEMENT, WILL BE CHANGED FOR FSM

    private Rigidbody2D rb;
    private float jumpTimeCounter;
    private Vector2 _velocity = new Vector2(0.0f, 0.0f);
    private float _timeLeft;
    private float _declaration = 0.0f;
    private float _elaspedTimeSinceButtonPress;

    // VARIALBES THE USER CAN EDIT TO CREATE DIFFERENT JUMP ARCS/MOVEMENT
    public string _walkableSurfaceTagName;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public float impluseJumpVel = 4.0f;
    public float TimeToReachMaxHeight = 0.5f;
    public float _movementTime = 0.100f;
    public float _MAX_WALKING_SPEED = 5.0f;
    public float acclearation = 17.0f;
    public float _LOWEST_WALKING_SPEED = 0.3f;
    // VARIALBES THE USER CAN EDIT TO CREATE DIFFERENT JUMP ARCS/MOVEMENT

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        // CHECK IF THE GAMEOBJECT HAS A RIGIDBODY IF NOT THEN CREATE ONE
        if (!rb)
        {
            gameObject.AddComponent<Rigidbody2D>();
            rb = GetComponent<Rigidbody2D>();
            rb.angularDrag = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 6;
        }

        // CHECK IF THE GAME OBJECT HAS A BOX COLLDER ATTACHED, IF NOT THEN CREATE ONE.
        if (!this.GetComponent<BoxCollider2D>())
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        getInput();
        move();
    }

    // Gets input for the user.
    void getInput()
    {
        getUpInput();
        getRightInput();
        getLightInput();
    }

    void move()
    {
        if ((_moveRight || _moveLeft) && !_stopMovement)
        {
            if (_moveRight)
            {
                moveRight();
            }
            else if (_moveLeft)
            {
                moveLeft();
            }
            _elaspedTimeSinceButtonPress += Time.deltaTime;
        }
        else
        {
            stopLeftAndRightMovement();
        }
    }

    void getRightInput()
    {
        if (Input.GetKeyDown(rightKey)) // RIGHT
        {
            handleRightInput();
        }
        if (Input.GetKeyUp(rightKey))
        {
            _moveRight = false;
        }
    }

    void getLightInput()
    {
        if (Input.GetKeyDown(leftKey)) // RIGHT
        {
            handleLeftInput();
        }
        if (Input.GetKeyUp(leftKey))
        {
            _moveLeft = false;
        }
    }

    void getUpInput()
    {
        if (Input.GetKeyDown(jumpKey) && _isGrounded) // UP
        {
            intialJump();
        }
        if (Input.GetKey(jumpKey) && _isJumping)
        {
            continuousJump();
        }
        if (Input.GetKeyUp(jumpKey))
        {
            _isJumping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D walkableSurface)
    {
        if (walkableSurface.gameObject.tag == _walkableSurfaceTagName && !_isGrounded)
        {
            _isGrounded = true;
            _isJumping = false;
        }
    }


    private void OnCollisionExit2D(Collision2D walkableSurface)
    {
        if (walkableSurface.gameObject.tag == _walkableSurfaceTagName && _isGrounded)
        {
            _isGrounded = false;
        }
    }

    public void moveRight()
    {
        if (_elaspedTimeSinceButtonPress < _timeLeft)
        {
            _velocity = rb.velocity;
            _velocity.x = getVel(_elaspedTimeSinceButtonPress).x;
            rb.velocity = _velocity;
            rb.velocity = new Vector2(Mathf.Clamp(_velocity.x, -_MAX_WALKING_SPEED, _MAX_WALKING_SPEED), _velocity.y); // Clamp speed.
        }
        else
        {
            Vector3 temp = rb.velocity;
            temp.x = _MAX_WALKING_SPEED;
            rb.velocity = temp;
        }
    }

    public void moveLeft()
    {
        if (_elaspedTimeSinceButtonPress < _timeLeft) // If the elaspedtime on when the button is pressed is less than 
        {
            _velocity = rb.velocity;
            _velocity.x = -getVel(_elaspedTimeSinceButtonPress).x;
            rb.velocity = _velocity;
            rb.velocity = new Vector2(Mathf.Clamp(_velocity.x, -_MAX_WALKING_SPEED, _MAX_WALKING_SPEED), _velocity.y); // Clamp speed.
        }
        else
        {
            Vector3 temp = rb.velocity;
            temp.x = -_MAX_WALKING_SPEED;
            rb.velocity = temp;
        }
    }

    public void stopLeftAndRightMovement()
    {
        if (_velocity.x != 0.0f)
        {
            _declaration = acclearation / _movementTime * Time.deltaTime; // Declaration = v/t.
            if (_velocity.x < 0.0f)
            {
                _velocity.x += _declaration; // reduce vel by declaration.
                if (_velocity.x >= -_LOWEST_WALKING_SPEED)
                {
                    _velocity.x = 0.0f;
                }
            }
            else if (_velocity.x > 0.0f)
            {
                _velocity.x -= _declaration; // reduce vel by declaration.
                if (_velocity.x <= _LOWEST_WALKING_SPEED)
                {
                    _velocity.x = 0.0f;
                }
            }
            _velocity.y = rb.velocity.y;
            rb.velocity = _velocity;
        }
    }

    public void intialJump()
    {
        handleJumpInput();
    }

    public void continuousJump()
    {
        if (jumpTimeCounter > 0)
        {
            Vector3 temp = rb.velocity;
            temp = Vector2.up * impluseJumpVel * 1.3f;
            temp.x = rb.velocity.x;
            rb.velocity = temp;
            jumpTimeCounter -= Time.deltaTime;
        }
        else // Else he is falling.
        {
            _isJumping = false;
        }
    }

    public bool getIsGrounded()
    {
        return _isGrounded;
    }

    public bool getIsMovingRight()
    {
        return _moveRight;
    }

    public bool getIsMovingLeft()
    {
        return _moveLeft;
    }

    public bool getIsJumping()
    {
        return _isJumping;
    }

    public void handleRightInput()
    {
        if (_moveLeft)
        {
            _timeLeft = _movementTime * 2.0f;
        }
        else
        {
            _timeLeft = _movementTime;
            if (_velocity.x > 0.0f)
            {
                _timeLeft = _MAX_WALKING_SPEED - _velocity.x / acclearation; // t = v - u / a.
            }
        }
        _moveRight = true;
        _moveLeft = false;
        _elaspedTimeSinceButtonPress = 0.0f;
    }

    public void handleLeftInput()
    {
        if (_moveRight)
        {
            _timeLeft = _movementTime * 2.0f;
        }
        else
        {
            _timeLeft = _movementTime;
            if (_velocity.x < 0.0f)
            {
                _timeLeft = _MAX_WALKING_SPEED - _velocity.x / acclearation; // t = v - u / a.
            }
        }
        _moveLeft = true;
        _moveRight = false;
        _elaspedTimeSinceButtonPress = 0.0f;
    }

    public void handleJumpInput()
    {
        Vector3 temp = rb.velocity;
        temp = Vector2.up * impluseJumpVel; // Impluse megaman into the air by a set amount.
        temp.x = rb.velocity.x;
        rb.velocity = temp;
        jumpTimeCounter = TimeToReachMaxHeight; // reset jumptimecounter.
        _isJumping = true;
    }

    Vector2 getVel(float time)
    {
        return new Vector3(acclearation * time, _velocity.y, 0.0f); // v = u + at.
    }

    public void setStopMovement(bool t_stopMovement)
    {
        _stopMovement = t_stopMovement;
    }
}
