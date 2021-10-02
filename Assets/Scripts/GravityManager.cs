using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GravityManager : MonoBehaviour
{
    public enum Directions
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3
    }

    public const float GRAVITY_STRENGTH = 1f;

    public UnityEvent<Directions> OnChangeGravity;

    private Directions direction;

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

    public void TestMovement(string direction)
    {
        if (!AllGravityObjectsDoneMoving())
        {
            return;
        }

        doneMovingGravityObjectCount = 0;
        
        switch (direction)
        {
            case "up":
                Direction = Directions.UP;
                break;
            case "left":
                Direction = Directions.LEFT;
                break;
            case "right":
                Direction = Directions.RIGHT;
                break;
            case "down":
                Direction = Directions.DOWN;
                break;
        }
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
