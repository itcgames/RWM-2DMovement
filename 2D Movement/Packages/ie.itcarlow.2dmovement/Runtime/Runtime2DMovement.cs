using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runtime2DMovement : MonoBehaviour
{
    private bool _moveRight; // right direction.
    private bool _moveLeft;
    private bool _isJumping;
    private Rigidbody2D rb;
    private bool _isGrounded; // is the player on the ground.
    private float jumpTimeCounter;

    public int _Walkingspeed;
    public string _walkableSurfaceTagName;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;
    public float impluseJumpVel;
    public float TimeToReachMaxHeight;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if (!rb)
        {
            gameObject.AddComponent<Rigidbody2D>();
            rb = GetComponent<Rigidbody2D>();
            rb.angularDrag = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        _moveRight = false;
        _moveLeft = false;
        _isGrounded = false;
        _Walkingspeed = 2;

        impluseJumpVel = 4f;
        TimeToReachMaxHeight = 0.5f;
    }

    void Update()
    {
        getInput();
    }

    // Gets input for the user.
    void getInput()
    {
        getRightInput();
        getLightInput();
        getUpInput();
        move();
    }

    void move()
    {
        if (_moveRight || _moveLeft)
        {
            if (_moveRight)
            {
                moveRight();
            }
            else if (_moveLeft)
            {
                moveLeft();
            }
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
            _moveRight = true;
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
            _moveLeft = true;
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
        Vector2 temp = rb.velocity;
        temp.x = _Walkingspeed;
        rb.velocity = temp;
    }

    public void moveLeft()
    {
        Vector2 temp = rb.velocity;
        temp.x = -_Walkingspeed;
        rb.velocity = temp;
    }

    public void stopLeftAndRightMovement()
    {
        Vector2 temp = rb.velocity;
        temp.x = 0;
        rb.velocity = temp;
    }

    void intialJump()
    {
        Vector3 temp = rb.velocity;
        temp = Vector2.up * impluseJumpVel; // Impluse megaman into the air by a set amount.
        temp.x = rb.velocity.x;
        rb.velocity = temp;
        jumpTimeCounter = TimeToReachMaxHeight; // reset jumptimecounter.
        _isJumping = true;
    }

    void continuousJump()
    {
        if (jumpTimeCounter > 0)
        {
            Vector3 temp = rb.velocity;
            temp = Vector2.up * impluseJumpVel;
            temp.x = rb.velocity.x;
            rb.velocity = temp;
            jumpTimeCounter -= Time.deltaTime;
        }
        else // Else he is falling.
        {
            _isJumping = false;
        }
    }
}
