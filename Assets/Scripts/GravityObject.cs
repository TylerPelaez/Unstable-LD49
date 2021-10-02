using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public enum Directions {STOP = 0, UP = 1, DOWN = 2, LEFT = 3, RIGHT = 4}

    [Tooltip("Speed movement")]
    [SerializeField] private float _speed = 5.0f;

    public float Speed { get => _speed; set => _speed = value; }

    protected Rigidbody2D rb2d;

    public Vector2 diretion2move;
    
    [Tooltip("Directorion of movement")]
    public Directions direction = Directions.STOP;

    [SerializeField] private BoxCollider2D kinematicCollider;

    private void Awake()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        var gravityManager = FindObjectOfType<GravityManager>();
        if(gravityManager != null)
            gravityManager.OnChangeGravity.AddListener(() => this.Move(gravityManager.Direction));
        
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(),kinematicCollider);
    }

    public void Move(Directions direction)
    {
        switch (direction)
        {
            case Directions.STOP:
                Move(Vector2.zero);
                break;

            case Directions.UP:
                Move(Vector2.up);
                break;

            case Directions.DOWN:
                Move(-Vector2.up);
                break;

            case Directions.LEFT:
                Move(Vector2.left);
                break;

            case Directions.RIGHT:
                Move(-Vector2.left);
                break;

            default:
                Move(Vector2.zero);
                break;
        }
    }

    void Move(Vector2 direction)
    {
        this.rb2d.velocity = direction * Speed;
        Debug.Log("Move " + direction);
    }
}
