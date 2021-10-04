using System;
using System.Collections.Generic;
using UnityEngine;

using static GravityManager;

public class GravityObjectKinematic : MonoBehaviour
{
    private Collider2D _collider;
    
    [SerializeField] private Vector2 movementDirection;

    [SerializeField]
    private bool isLerping = false;
    [SerializeField]
    private Vector2 targetPosition = Vector2.positiveInfinity;

    private LifeCycleManager lifecycleManager;
    private GravityManager gravityManager;

    private Vector2 velocity;

    private List<Vector2> previousPositions = new List<Vector2>();
    private int lastPositionIndex = -1;

    private bool goingToEscape = false;
    private bool shouldPlayClip = false;
    
    private Vector3 previousState;
    private Vector3 currentState;

    private Animator animator;
    private SpriteRenderer sprite;
    private TrailRenderer trailRenderer;

    private SoundPlayer soundPlayer;
    
    private void Start()
    {
        previousState = transform.position;
        currentState = transform.position;
        _collider = GetComponent<Collider2D>();
        gravityManager = FindObjectOfType<GravityManager>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        soundPlayer = GetComponent<SoundPlayer>();
        trailRenderer = GetComponent<TrailRenderer>();

        if (gravityManager != null)
        {
            gravityManager.OnChangeGravity.AddListener(this.ChangeDirection);
            gravityManager.RegisterGravityObject();
        }

        lifecycleManager = FindObjectOfType<LifeCycleManager>();
        if (lifecycleManager != null)
        {
            lifecycleManager.OnRedo.AddListener(Redo);
            lifecycleManager.OnUndo.AddListener(Undo);
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
            previousState = currentState;
            
            velocity = velocity + ( movementDirection * Time.fixedDeltaTime * GRAVITY_STRENGTH) ;
            var newPosition = (Vector2)transform.position + velocity;
            // Avert your eyes if you don't want to go blind
            if (
                (
                    (movementDirection.x < 0f || movementDirection.y < 0f) && 
                    (newPosition.x < targetPosition.x || Mathf.Approximately(newPosition.x, targetPosition.x)) && 
                    (newPosition.y < targetPosition.y || Mathf.Approximately(newPosition.y, targetPosition.y))
                ) ||
                (
                    (movementDirection.x > 0f || movementDirection.y > 0f) && 
                    (newPosition.x > targetPosition.x || Mathf.Approximately(newPosition.x, targetPosition.x)) && 
                    (newPosition.y > targetPosition.y || Mathf.Approximately(newPosition.y, targetPosition.y))
                )
            )
            {
                ReachedTargetPosition();
            }
            else
            {
                currentState = newPosition;
            }
        }
    }

    private void Update()
    {
        if (isLerping)
        {
            float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            Vector3 lerpState = Vector3.Lerp(previousState, currentState, alpha);
            transform.position = lerpState;

        }
    }

    private void ReachedTargetPosition()
    {
        transform.position = targetPosition;
        velocity = Vector2.zero;
        isLerping = false;
        currentState = targetPosition;
        previousState = targetPosition;
        gravityManager.GravityObjectDoneMoving();
        if (goingToEscape && gameObject.CompareTag("Player"))
        {
            lifecycleManager.WinLevel();
        } else if (goingToEscape)
        {
            Destroy(gameObject);
        }

        if (shouldPlayClip)
        {
            soundPlayer.PlayRandomClip();
        }

        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }

        shouldPlayClip = false;
    }

    public void ChangeDirection(Directions _direction)
    {
        if(animator != null)
            animator.SetTrigger("squish");

        velocity = Vector2.zero;
        switch (_direction)
        {
            case Directions.UP:
                movementDirection = Vector2.up;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                sprite.flipX = false;
                break;

            case Directions.DOWN:
                movementDirection = Vector2.down;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                sprite.flipX = false;
                break;

            case Directions.LEFT:
                movementDirection = Vector2.left;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                sprite.flipX = false;
                break;

            case Directions.RIGHT:
                movementDirection = Vector2.right;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                sprite.flipX = true;
                break;
        }
        
        // Now determine target position
        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(transform.position, movementDirection, 2000f);
        float totalOffset = 0f;
        bool objectAdjacent = false;
        
        // target position check
        foreach (var hit in hits)
        {
            if (hit.collider == _collider)
            {
                continue;
            }

            if (hit.collider.gameObject.CompareTag("Object") || hit.collider.gameObject.CompareTag("Player"))
            {
                totalOffset += hit.collider.bounds.size.x;
                if (hit.collider.transform.position == transform.position + (Vector3) movementDirection)
                {
                    objectAdjacent = true;
                }
                continue;
            }

            if (hit.collider.isTrigger && hit.collider.gameObject.CompareTag("Win"))
            {
                targetPosition = hit.collider.gameObject.transform.position + (Vector3)(movementDirection * 10f);
                goingToEscape = true;
                break;
            }
            
            // Must have hit a wall
            targetPosition = (Vector2) hit.collider.gameObject.transform.position +
                             (-movementDirection * hit.collider.bounds.extents) +
                             (-movementDirection * totalOffset) + 
                             (-movementDirection * _collider.bounds.extents);
            
            break;
        }

        if (trailRenderer != null)
        {
            // Also check if anything is directly behind object
            hits = Physics2D.RaycastAll(transform.position, -movementDirection, 1f);
            foreach (var hit in hits)
            {
                if (hit.collider == _collider)
                {
                    continue;
                }
                if (hit.collider.gameObject.CompareTag("Object"))
                {
                    if (hit.collider.transform.position == transform.position - (Vector3) movementDirection)
                    {
                        trailRenderer.enabled = false;
                    }
                }
            }
        }

        
        
        if (!objectAdjacent && targetPosition != (Vector2)transform.position)
        {
            shouldPlayClip = true;
        }

        targetPosition = new Vector2(RoundToNearestHalf(targetPosition.x), RoundToNearestHalf(targetPosition.y));
        
        if (lastPositionIndex < previousPositions.Count - 1)
        {
            previousPositions.RemoveRange(lastPositionIndex + 1, previousPositions.Count - (lastPositionIndex + 1));
        }
        previousPositions.Add(transform.position);
        lastPositionIndex++;
        
        previousState = transform.position;
        currentState = transform.position;
        
        isLerping = true;
    }

    private static float RoundToNearestHalf(float a)
    {
        return a = Mathf.Round(a * 2f) * 0.5f;
    }
    
    private void Undo()
    {
        if (lastPositionIndex < 0 || isLerping) // islerping shouldn't happen but...
        {
            return;
        }

        if (lastPositionIndex == previousPositions.Count - 1)
        {
            previousPositions.Add(transform.position);
        }
        
        transform.position = previousPositions[lastPositionIndex];
        lastPositionIndex--;
    }

    private void Redo()
    {
        if (lastPositionIndex >= previousPositions.Count - 1 || isLerping)
        {
            return;
        }
        
        transform.position = previousPositions[lastPositionIndex + 2];
        lastPositionIndex ++;
    }
}

