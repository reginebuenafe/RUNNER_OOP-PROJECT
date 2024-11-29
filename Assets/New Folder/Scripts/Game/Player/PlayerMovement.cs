using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _screenBorder;

    private Rigidbody2D rb;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    private Camera _camera;
    private Animator _animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       SetPlayerVelocity();
       RotateInDirectionOfInput();
       SetAnimation();
    }

    private void SetAnimation(){
        bool isMoving = _movementInput != Vector2.zero;

        _animator.SetBool("IsMoving", isMoving);
    }
    private void SetPlayerVelocity(){
        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.1f);

        
        rb.linearVelocity = _smoothedMovementInput * _speed;
        PreventPlayerGoingOffScreen();
    }

    private void PreventPlayerGoingOffScreen(){
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        if((screenPosition.x < _screenBorder && rb.linearVelocity.x < 0)||
        (screenPosition.x > _camera.pixelWidth - _screenBorder && rb.linearVelocity.x > 0)){
            rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
        }

        if((screenPosition.y < _screenBorder && rb.linearVelocity.y < 0)||
        (screenPosition.y > _camera.pixelHeight - _screenBorder && rb.linearVelocity.y > 0)){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,0);
        }
    }

    private void RotateInDirectionOfInput(){
        if(_movementInput != Vector2.zero){
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothedMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation,targetRotation, _rotationSpeed * Time.deltaTime);

            rb.MoveRotation(rotation);

        }
    }
    private void OnMove(InputValue inputValue){
        _movementInput = inputValue.Get<Vector2>();

    }
}
