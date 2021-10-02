using UnityEngine;

using static GravityManager;

public class GravityObjectKinematic : MonoBehaviour
{
    protected Rigidbody2D rb2d;

    public const float TERMINAL_VELOCITY_MAGNITUDE = 10f; 
    
    [SerializeField] private Vector2 movementDirection;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private bool doneMoving;

    private Vector2 previousPosition;
    private GravityManager gravityManager;
    
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        doneMoving = true;
        previousPosition = transform.position;
        gravityManager = FindObjectOfType<GravityManager>();
        if (gravityManager != null)
        {
            gravityManager.OnChangeGravity.AddListener(ChangeDirection);
            gravityManager.RegisterGravityObject();
        }
    }

    private void OnDestroy()
    {
        gravityManager.OnChangeGravity.RemoveListener(ChangeDirection);
        gravityManager.UnregisterGravityObject(doneMoving);
    }

    private void FixedUpdate()
    {
        if (previousPosition == (Vector2)transform.position)
        {
            if (!doneMoving)
            {
                gravityManager.GravityObjectDoneMoving();
                
            }
            doneMoving = true;
            velocity = Vector2.zero;
            return;
        }
        previousPosition = transform.position;
        
        var acceleration = movementDirection * GRAVITY_STRENGTH * Time.deltaTime;
        velocity += acceleration;
        if (velocity.magnitude > TERMINAL_VELOCITY_MAGNITUDE)
        {
            velocity = velocity.normalized * TERMINAL_VELOCITY_MAGNITUDE;
        }
        
        var newPosition = (Vector2)transform.position + velocity;
        rb2d.MovePosition(newPosition);
    }

    public void ChangeDirection(Directions _direction)
    {
        doneMoving = false;
        velocity = Vector2.zero;
        previousPosition = Vector2.positiveInfinity; // Just make it not be equal to any real position to reset it
        switch (_direction)
        {
            case Directions.UP:
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                movementDirection = Vector2.up;
                break;

            case Directions.DOWN:
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                movementDirection = Vector2.down;
                break;

            case Directions.LEFT:
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                movementDirection = Vector2.left;
                break;

            case Directions.RIGHT:
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                movementDirection = Vector2.right;
                break;
        }
    }
}

