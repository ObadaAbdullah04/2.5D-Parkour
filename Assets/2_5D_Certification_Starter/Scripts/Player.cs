using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _anim;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _gravity = 1f;
    [SerializeField] private float _jumpHeight = 1f;
    private Vector3 _direction = Vector3.zero;
    private float _yVelocity;
    Vector3 facing;
    bool _isJumping = false;
    bool _canClimb = false;
    bool _isLedge = false;
    private Ledge _activeLedge;
    

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!_isLedge)
        {
            Movement();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && _canClimb)
            {
                _anim.SetBool("GrabLedge", false);
                _anim.SetBool("isClimbing", true);
                _canClimb = false;
                _isLedge = false;
            }
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        _direction.z = horizontal * _speed;

        _anim.SetFloat("Speed", Mathf.Abs(_direction.z));

        if (horizontal != 0)
        {
            facing = transform.localEulerAngles;
            facing.y = _direction.z > 0 ? 0 : 180;
            transform.localEulerAngles = facing;
        }
        if (_controller.isGrounded)
        {
            _isJumping = false;
            _anim.SetBool("isJumping", _isJumping);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _isJumping = true;
                _anim.SetBool("isJumping", _isJumping);
            }
        }
        else
        {
            _yVelocity -= _gravity * Time.deltaTime;
        }

        _direction.y = _yVelocity;
        _controller.Move(_direction * Time.deltaTime);
    }
    public void GrabLedge(Vector3 handPos, Ledge currentLedge)
    {
        _isLedge = true;
        _anim.SetBool("GrabLedge", true);
        _anim.SetFloat("Speed", 0.0f);
        _anim.SetBool("isJumping", false);
        _canClimb = true;
        _controller.enabled = false;

        facing.y = 0;
        transform.position = handPos;
        _activeLedge = currentLedge;
    }
    public void ClimbUpComplete()
    {
        _anim.SetBool("isClimbing", false);
        transform.position = _activeLedge.GetStandPos();
        _controller.enabled = true;
    }
}
