using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GravityManager;

public class Arrow : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        var gravityManager = FindObjectOfType<GravityManager>();
        if (gravityManager != null)
        {
            gravityManager.OnChangeGravity.AddListener(changeDirection);
        }

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeDirection(Directions direction)
    {
        animator.SetTrigger("unhide");
        switch (direction)
        {
            case Directions.UP:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;

            case Directions.DOWN:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;

            case Directions.LEFT:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;

            case Directions.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case Directions.DEFAULT:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            default:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }
}
