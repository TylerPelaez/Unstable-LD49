using UnityEngine;

using static GravityManager;

public class GravityObjectDynamic : MonoBehaviour
{
    [Tooltip("Speed movement")]
    [SerializeField] private float _speed = 5.0f;

    public float Speed { get => _speed; set => _speed = value; }

    protected Rigidbody2D rb2d;

    public Vector2 diretion2move;
    
    [Tooltip("Directorion of movement")]
    public Directions direction = Directions.DOWN;

    [SerializeField] private BoxCollider2D kinematicCollider;

    private void Awake()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        var gravityManager = FindObjectOfType<GravityManager>();
        if(gravityManager != null)
            gravityManager.OnChangeGravity.AddListener(Move);
        
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(),kinematicCollider);
    }

    public void Move(Directions direction)
    {
        switch (direction)
        {

            case Directions.UP:
                Move(Vector2.up);
                break;


            case Directions.LEFT:
                Move(Vector2.left);
                break;

            case Directions.RIGHT:
                Move(-Vector2.left);
                break;

            default:
            case Directions.DOWN:
                Move(-Vector2.up);
                break;
        }
    }

    void Move(Vector2 direction)
    {
        this.rb2d.velocity = direction * Speed;
    }
}