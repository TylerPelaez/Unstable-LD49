using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GravityManager : MonoBehaviour
{
    public UnityEvent OnChangeGravity;

    private GravityObject.Directions direction;

    public GravityObject.Directions Direction {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
            // invoke when changing events
            OnChangeGravity.Invoke();
        }
    }

    public void TestMovement(string direction)
    {
        switch (direction)
        {
            case "up":
                Direction = GravityObject.Directions.UP;
                break;
            case "down":
                Direction = GravityObject.Directions.DOWN;
                break;
            case "left":
                Direction = GravityObject.Directions.LEFT;
                break;
            case "right":
                Direction = GravityObject.Directions.RIGHT;
                break;
            case "stop":
                Direction = GravityObject.Directions.STOP;
                break;
            default:
                Direction = GravityObject.Directions.STOP;
                break;
        }
    }
}
