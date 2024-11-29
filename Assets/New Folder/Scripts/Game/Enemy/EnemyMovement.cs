using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private float _screenBorder;

    [SerializeField]
    private float _obstacleCheckCircleRadius;

    [SerializeField]
    private float _obstacleCheckDistance;

    [SerializeField]
    private LayerMask _obstacleLayerMask;

    

    private Rigidbody2D _rb;
    private PlayerAwarenessController _playerAwarenessController;

    private Vector2 _targetDirection;
    private float _changeDirectionCooldown;
    private Camera _camera;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _obstacleAvoidanceTargetDirection;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        _camera = Camera.main;
        _obstacleCollisions = new RaycastHit2D[10];
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection(){
        HandleRandomDirectionChange();
        HandlePLayerTargeting();
        HandleObstacles();
        HandleEnemyOffScreen();
    }

    private void HandleRandomDirectionChange(){
        _changeDirectionCooldown -= Time.deltaTime;
        if(_changeDirectionCooldown <=0){
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange,transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(1f,5f);
        }
    }

    private void HandlePLayerTargeting(){
        if(_playerAwarenessController.AwareOfPlayer){
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
    }

    private void HandleEnemyOffScreen(){
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        if((screenPosition.x < _screenBorder && _targetDirection.x < 0)||
        (screenPosition.x > _camera.pixelWidth - _screenBorder && _targetDirection.x > 0)){
            _targetDirection = new Vector2(-_targetDirection.x,_targetDirection.y);
        }

        if((screenPosition.y < _screenBorder && _targetDirection.y < 0)||
        (screenPosition.y > _camera.pixelHeight - _screenBorder && _targetDirection.y > 0)){
            _targetDirection = new Vector2(-_targetDirection.x,-_targetDirection.y);
        }
    }

    private void HandleObstacles(){
        _obstacleAvoidanceCooldown -= Time.deltaTime;

        var  contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_obstacleLayerMask);

        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            _obstacleCheckCircleRadius,
            transform.up,
            contactFilter,
            _obstacleCollisions,
            _obstacleCheckDistance
        );

        for (int index = 0; index < numberOfCollisions; index++){
            var obstcaleCollsion = _obstacleCollisions[index];

            if(obstcaleCollsion.collider.gameObject == gameObject){
                continue;
            }

            if(_obstacleAvoidanceCooldown <= 0){
                _obstacleAvoidanceTargetDirection = obstcaleCollsion.normal;
                _obstacleAvoidanceCooldown = 0.5f;
            }
            var targetRotation = Quaternion.LookRotation(transform.forward, _obstacleAvoidanceTargetDirection);
            var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            
            _targetDirection = rotation * Vector2.up;
            break;
        }
    }

    private void RotateTowardsTarget(){

        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rb.SetRotation(rotation);
    }

    private void SetVelocity(){
        
            _rb.linearVelocity = transform.up * _speed;
        
    }
}
