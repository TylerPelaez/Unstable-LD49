using System;
using UnityEngine;
using UnityEngine.Events;

public class GravityManager : MonoBehaviour
{
    public enum Directions
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
        DEFAULT = 4,
    }

    public const float GRAVITY_STRENGTH = 1f;

    public UnityEvent<Directions> OnChangeGravity;

    private Directions direction = Directions.DEFAULT;

    [SerializeField] private int registeredGravityObjectCount;
    [SerializeField] private int doneMovingGravityObjectCount = 0;
    
    public Directions Direction {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
            // invoke when changing events
            OnChangeGravity.Invoke(direction);
        }
    }

    private void Start()
    {
        var lifecycleManager = FindObjectOfType<LifeCycleManager>();
        if (lifecycleManager != null)
        {
            lifecycleManager.OnRedo.AddListener(ResetDirection);
            lifecycleManager.OnUndo.AddListener(ResetDirection);
        }
    }

    private void ResetDirection()
    {
        direction = Directions.DEFAULT;
    }
    
    public void ChangeDirection(Directions newDirection)
    {
        if (!AllGravityObjectsDoneMoving() || newDirection == direction)
        {
            return;
        }

        doneMovingGravityObjectCount = 0;
        Direction = newDirection;
}

    public bool AllGravityObjectsDoneMoving()
    {
        return doneMovingGravityObjectCount == registeredGravityObjectCount;
    }
    
    public void RegisterGravityObject()
    {
        registeredGravityObjectCount++;
        doneMovingGravityObjectCount++;
    }
    
    public void UnregisterGravityObject(bool doneMoving)
    {
        registeredGravityObjectCount--;
        if (doneMoving)
        {
            doneMovingGravityObjectCount--;
        }
    }

    public void GravityObjectDoneMoving()
    {
        doneMovingGravityObjectCount++;
    }
}
