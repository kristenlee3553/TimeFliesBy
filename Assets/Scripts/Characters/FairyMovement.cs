using UnityEngine;

public class FairyMovement : MonoBehaviour
{
    // Movement variables
    public float movementSpeed = 5f;
    private Vector2 fairyMovement;

    // Component variables
    [SerializeField] private Rigidbody2D rbFairy;

    /// <summary>
    /// When set to true, will not listen for fairy key input
    /// </summary>
    public bool noInputFairy = false;

    private SpriteRenderer sprite;
    private Animator animator;

    bool isFacingRight = false;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Called once per frame
    void Update()
    {
        if (!noInputFairy)
        {
            // Default is no movement
            fairyMovement = Vector2.zero;

            // Handles fairy inputs
            if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.FairyLeft]))
            {
                fairyMovement = Vector2.left;
            }
            if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.FairyRight]))
            {
                fairyMovement = Vector2.right;
            }
            if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.FairyUp]))
            {
                fairyMovement = Vector2.up;
            }
            if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.FairyDown]))
            {
                fairyMovement = Vector2.down;
            }

            if (PreserveManager.IsPreserving())
            {
                fairyMovement = Vector2.zero;
            }
            // Boundaries
            if (transform.position.x < GameManager.s_boundaryLeft)
            {
                Vector3 newPos = new(GameManager.s_boundaryLeft, transform.position.y, transform.position.z);
                transform.position = newPos;
            }
            else if (transform.position.x > GameManager.s_boundaryRight)
            {
                Vector3 newPos = new(GameManager.s_boundaryRight, transform.position.y, transform.position.z);
                transform.position = newPos;
            }
            if (transform.position.y > GameManager.s_boundaryUp)
            {
                Vector3 newPos = new(transform.position.x, GameManager.s_boundaryUp, transform.position.z);
                transform.position = newPos;
            }
            else if (transform.position.y < GameManager.s_boundaryDown)
            {
                Vector3 newPos = new(transform.position.x, GameManager.s_boundaryDown, transform.position.z);
                transform.position = newPos;
            }

            Flip();
        }
    }

    // Called 50 times per second
    private void FixedUpdate()
    {
        if (!noInputFairy)
        {
            // Update fairy movement
            rbFairy.velocity = movementSpeed * fairyMovement;
        }
    }

    private void Flip()
    {
        if (isFacingRight && fairyMovement.Equals(Vector2.left) || !isFacingRight && fairyMovement.Equals(Vector2.right))
        {
            isFacingRight = !isFacingRight;
            sprite.flipX = !sprite.flipX;
        }
    }

    public void resetFairy()
    {
        rbFairy.velocity = Vector2.zero;
        animator.SetBool("holding", false);
        
    }
}