using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runtime2DMovement : MonoBehaviour
{
    private bool _moveRight; // right direction.
    private bool _moveLeft;
    private Rigidbody2D rb;
    private bool _isGrounded; // is the player on the ground.

    public int _speed;
    public string _walkableSurfaceTagName;
    public KeyCode leftKey;
    public KeyCode rightKey;

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
        _speed = 2;
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
        move();
    }

    void move()
    {
        if (_moveRight || _moveLeft)
        {
            if (_moveRight)
            {
                Vector2 temp = rb.velocity;
                temp.x = _speed;
                rb.velocity = temp;
            }
            else if (_moveLeft)
            {
                Vector2 temp = rb.velocity;
                temp.x = -_speed;
                rb.velocity = temp;
            }
        }
        else
        {
            Vector2 temp = rb.velocity;
            temp.x = 0;
            rb.velocity = temp;
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

    private void OnCollisionEnter2D(Collision2D walkableSurface)
    {
        if (walkableSurface.gameObject.tag == _walkableSurfaceTagName && !_isGrounded)
        {
            _isGrounded = true;
        }
    }


    private void OnCollisionExit2D(Collision2D walkableSurface)
    {
        if (walkableSurface.gameObject.tag == _walkableSurfaceTagName && _isGrounded)
        {
            _isGrounded = false;
        }
    }
}
