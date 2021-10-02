//#define STANDALONE_CONTROL 
#define USING_JOYSTICK_VARIABLE
/* a��es executadas internada pela propria classe MonoBehavior;
 * Ex: Move sem a necessidade de outra classe externa a chama-la */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if USING_CROSS_PLATAFORM
using UnityStandardAssets.CrossPlatformInput;
#endif

public class MovementByInput : MonoBehaviour, IMovementInput
{
#if USING_JOYSTICK_VARIABLE
    private Joystick variableJoystick;
    public Joystick VariableJoystick {
        get 
        { if (variableJoystick == null)
                variableJoystick = GameObject.FindObjectOfType<Joystick>();
            return variableJoystick;
        }
        set => variableJoystick = value; }
#endif

    [SerializeField] protected CanFlipSprite FlipSprite;

    [SerializeField] private readonly bool STANDALONE_CONTROL = true;

    [Tooltip("Habilitar movimento")]
    [SerializeField] private bool _enable = true;
    
    [Tooltip("Velocidade movimento")]
    [SerializeField] private float _speed = 5.0f;

    [Tooltip("Velocidade movimento")]
    [SerializeField]
    private bool _isMoving = false;

    public float Speed { get => _speed; set => _speed = value; }
    public bool EnableMove { get => _enable; set => _enable = value; }
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    protected Rigidbody2D rb2d;

    private float offsetHorizontalToMove = 0.1f;

    private void Awake()
    {
        FlipSprite.Sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (STANDALONE_CONTROL)
            Move();
    }

    public virtual void Move()
    {
        if (rb2d == null)
            rb2d = this.GetComponent<Rigidbody2D>();

        if (!EnableMove) {
            this.rb2d.velocity = Vector2.zero;
            return;
        }

#if USING_CROSS_PLATAFORM
        this.rb2d.velocity = new Vector2(
            CrossPlatformInputManager.GetAxis("Horizontal") * Speed * Time.fixedDeltaTime,
            CrossPlatformInputManager.GetAxis("Vertical") * Speed) * Time.fixedDeltaTime;
#endif

        this.rb2d.MovePosition(rb2d.position + new Vector2(VariableJoystick.Horizontal, VariableJoystick.Vertical) * Speed * Time.fixedDeltaTime);
        
        if (this.VariableJoystick.Horizontal > offsetHorizontalToMove)
            FlipSprite.MovingRight = false;
        else if (this.VariableJoystick.Horizontal < offsetHorizontalToMove * -1.0f)
            FlipSprite.MovingRight = true;

        if (this.VariableJoystick.Horizontal > offsetHorizontalToMove * -1.0f && this.VariableJoystick.Horizontal < offsetHorizontalToMove)
            IsMoving = false;
        else
            IsMoving = true;


    }
}
