using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Controller Settings")]
    [SerializeField] private float _walkSpeed = 8f;
    [SerializeField] private float _runSpeed = 12f;
    [SerializeField] private float _gravityModifier = 0.5f; //0.95f;
    [SerializeField] private float _jumpPower = 0.25f;
    [SerializeField] private InputAction _movementInput;

    private CharacterController _characterController;

    private float _currentSpeed = 5f;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector2 _heightMovement;
    private bool _jump;

    private GameObject _player;
    
    // x horizontal, y vertical

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        _movementInput.Enable();
    }

    private void OnDisable()
    {
        _movementInput.Disable();
    }

    private void Update()
    {
        
            
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        _horizontalInput = _movementInput.ReadValue<Vector2>().x;
        _verticalInput = _movementInput.ReadValue<Vector2>().y;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && _characterController.isGrounded)
            _jump = true;

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            _currentSpeed = _runSpeed;
        }
        else
        {
            _currentSpeed = _walkSpeed;
        }
        
    }

    private void FixedUpdate()
    {
        if (_player.transform.position.z > 0 || _player.transform.position.z < 0)
        {
            Vector3 transformPosition = _player.transform.position;
            transformPosition.z = 0;
        }
        Move();
    }

    private void Move()
    {
        if (_jump)
        {
            if (Keyboard.current.leftShiftKey.isPressed)
            {
                _heightMovement.y = _jumpPower * 1.5f;
            }
            else
            {
                _heightMovement.y = _jumpPower;
            }

            _jump = false;
        }

        _heightMovement.y -= _gravityModifier * Time.deltaTime;
        Vector2 localVerticalVector = transform.up * _verticalInput;
        Vector2 localHorizontalVector = transform.right * _horizontalInput;
        Vector2 _movementVector = localHorizontalVector + localHorizontalVector;
        _movementVector.Normalize();
        _movementVector *= _currentSpeed * Time.deltaTime;
        _characterController.Move(_movementVector + _heightMovement);

        if (_characterController.isGrounded)
        {
            _heightMovement.y = 0f;
        }
    }
    
    

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("   hahahahahahahahahah    ");
        CheckEnemyCollision(other);
    }

    private void CheckEnemyCollision(Collision other)
    {
        if (other.transform.tag == "RubberEnemy")
        {
            StartCoroutine("DieAndRestart");
        }
    }

    IEnumerator DieAndRestart()
    {
        Destroy(_player);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
