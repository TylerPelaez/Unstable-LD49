using UnityEngine;

using static GravityManager;

public class GravityObjectKinematic : MonoBehaviour
{
    private Collider2D _collider;
    
    [SerializeField] private Vector2 movementDirection;

    [SerializeField]
    private bool isLerping = false;
    [SerializeField]
    private Vector2 lerpStartPosition;
    [SerializeField]
    private Vector2 targetPosition = Vector2.positiveInfinity;
    [SerializeField]
    private float lerpStartTime;
    [SerializeField]
    private float totalLerpTime;

    private GravityManager gravityManager;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        gravityManager = FindObjectOfType<GravityManager>();
        if (gravityManager != null)
        {
            gravityManager.OnChangeGravity.AddListener(this.ChangeDirection);
            gravityManager.RegisterGravityObject();
        }
    }

    private void OnDestroy()
    {
        gravityManager.OnChangeGravity.RemoveListener(this.ChangeDirection);
        gravityManager.UnregisterGravityObject(!isLerping);
    }

    private void FixedUpdate()
    {
        if (isLerping)
        {
            float timeSinceLerpStart = Time.time - lerpStartTime;
            float percentComplete = timeSinceLerpStart / totalLerpTime;
            transform.position = Vector2.Lerp(lerpStartPosition, targetPosition, percentComplete);
            if (percentComplete >= 1.0f)
            {
                isLerping = false;
                gravityManager.GravityObjectDoneMoving();
            }
        }
    }

    public void ChangeDirection(Directions _direction)
    {
        switch (_direction)
        {
            case Directions.UP:
                movementDirection = Vector2.up;
                break;

            case Directions.DOWN:
                movementDirection = Vector2.down;
                break;

            case Directions.LEFT:
                movementDirection = Vector2.left;
                break;

            case Directions.RIGHT:
                movementDirection = Vector2.right;
                break;
        }
        
        // Now determine target position
        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(transform.position, movementDirection, 2000f);
        float totalOffset = 0f;
        foreach (var hit in hits)
        {
            if (hit.collider == _collider)
            {
                continue;
            }

            if (hit.collider.gameObject.CompareTag("Object") || hit.collider.gameObject.CompareTag("Player"))
            {
                totalOffset += hit.collider.bounds.size.x;
                continue;
            }

            if (gameObject.CompareTag("Player") && hit.collider.isTrigger && hit.collider.gameObject.CompareTag("Win"))
            {
                targetPosition = hit.collider.gameObject.transform.position;
                break;
            }
            
            // Must have hit a wall
            targetPosition = (Vector2) hit.collider.gameObject.transform.position +
                             (-movementDirection * hit.collider.bounds.extents) +
                             (-movementDirection * totalOffset) + 
                             (-movementDirection * _collider.bounds.extents);
            break;
        }

        var position = transform.position;
        lerpStartPosition = position;
        var lerpDistance = Vector2.Distance(targetPosition, position);
        lerpStartTime = Time.time;
        totalLerpTime = lerpDistance / GRAVITY_STRENGTH;
        isLerping = true;
    }
}

