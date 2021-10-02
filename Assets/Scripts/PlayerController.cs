using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private BoxCollider2D collider;

    // Prevents us from moving when we hit gravity objects
    [SerializeField] private BoxCollider2D kinematicCollider;
    
    private float dirX = 0f;
    
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float coyoteTime = .1f;
    
    [SerializeField]
    private LayerMask jumpableGround;

    private float lastGroundedTime;
    [SerializeField]
    private bool isGrounded = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        lastGroundedTime = Time.time;
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(collider, kinematicCollider);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        Vector2 targetVelocity = new Vector2(dirX * moveSpeed, rigidBody.velocity.y);

        if (targetVelocity.magnitude > maxSpeed)
        {
            targetVelocity = targetVelocity.normalized * maxSpeed;
        }

        if (IsGrounded())
        {
            lastGroundedTime = Time.time;
        }

        isGrounded = Time.time - lastGroundedTime <= coyoteTime;
        
        rigidBody.velocity = targetVelocity;

        if (Input.GetButtonDown("Debug Reset"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        { 
            rigidBody.AddForce(new Vector2(0, jumpForce));
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
