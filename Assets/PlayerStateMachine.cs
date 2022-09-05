using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    LOCOMOTION,
    SPRINT,
    ROLL
}
public class PlayerStateMachine : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _sprintInput;
    [SerializeField] InputActionReference _rollInput;
    

    [Header("Movement")]
    [SerializeField] Transform _root;
    [SerializeField] float _speed;
    [SerializeField] float _sptintSpeed;
    [SerializeField] float _movingThreshold;
    [SerializeField] Animator _animator;

    //float _movementSpeed;
    Vector2 _playerMovement;
    bool _isSprinting;
    bool _isRolling;
    private void Start()
    {
        //Move 
        _moveInput.action.started += StartMove;
        _moveInput.action.performed += UpdateMove;
        _moveInput.action.canceled += EndMove;

        //Sprint
        _sprintInput.action.started += Sprint;
        //Roll
        _rollInput.action.started += Roll;

    }
    private void FixedUpdate()
    {
       // _movementSpeed = _speed;
        Vector2 direction = new Vector2(_playerMovement.x, _playerMovement.y);
        _root.transform.Translate(direction * Time.fixedDeltaTime * _speed, Space.World);
        //Debug.Log($"Magnitude : {direction.magnitude}");

        //animation
        _animator.SetFloat("MoveSpeedX" , direction.x);
        _animator.SetFloat("MoveSpeedY" , direction.y);

        _animator.SetBool("Idle", true);
        _animator.SetBool("IsRolling", false);
        _animator.SetBool("IsRunning", false);

        if (_isSprinting)
        {
//            _speed = _sptintSpeed;
//            _animator.SetBool("Idle", false);
            _animator.SetBool("IsRunning", true);
//            _animator.SetBool("IsRolling", false);

        }
        else if(_isRolling)
        {
//            _animator.SetBool("Idle", false);
//            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsRolling", true);
        }

        //_movementSpeed = _speed;

    }
    private void LateUpdate()
    {
        _isSprinting = false;
        _isRolling = false; 
    }

    #region Move Action
    void StartMove(InputAction.CallbackContext obj)
    {
        _playerMovement = obj.ReadValue<Vector2>();
    }
    void UpdateMove(InputAction.CallbackContext obj)
    {
        _playerMovement = obj.ReadValue<Vector2>();
    }
    void EndMove(InputAction.CallbackContext obj)
    {
        _playerMovement = new Vector2(0, 0);
    }
    void Sprint(InputAction.CallbackContext obj)
    {
        _isSprinting = obj.ReadValueAsButton();
        Debug.Log("Sprint");
        _animator.SetBool("IsRunning", true);

    }
    void Roll(InputAction.CallbackContext obj)
    {
        _isRolling = obj.ReadValueAsButton();
        Debug.Log("Rolling");
        _animator.SetBool("IsRolling", true);

    }

    #endregion
}
