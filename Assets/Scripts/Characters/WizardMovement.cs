using UnityEngine;

/// <summary>
/// Slope stuff: https://www.youtube.com/watch?v=QPiZSTEuZnw
/// TO DO: ANIMATION CONTROLLER
/// </summary>
public class WizardMovement : MonoBehaviour
{
    // Movement variables
    private float wizardMovement;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpingPower;
    private bool isFacingRight = true;

    /// <summary>
    /// True if wizard is on ground layer
    /// </summary>
    private bool isGrounded;

    /// <summary>
    /// True if wizard is currently jumping
    /// </summary>
    private bool isJumping;

    /// <summary>
    /// True if wizard is able to jump
    /// </summary>
    private bool canJump;

    /// <summary>
    /// Max speed wizard can fall before dying
    /// </summary>
    private readonly float maxYVelocity = -7.036598f;

    /// <summary>
    /// True if fairy is freezing wizard
    /// </summary>
    bool isFrozen;

    // Component variables
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // ----------------------- Slope Stuff -----------------------

    /// <summary>
    /// How far to check if on a slope
    /// </summary>
    [SerializeField] private float slopeCheckDistance;

    /// <summary>
    /// Max slope angle wizard can climb
    /// </summary>
    [SerializeField] private float maxSlopeAngle;

    /// <summary>
    /// True if wizard can walk on the slope
    /// </summary>
    private bool canWalkOnSlope;

    /// <summary>
    /// True if wizard is on a slope
    /// </summary>
    private bool isOnSlope;

    /// <summary>
    /// The angle from the bottom of the wizard to the ground
    /// </summary>
    private float slopeDownAngle;

    /// <summary>
    /// The angle from the bottom of the the wizard to the front of the wizard
    /// </summary>
    private float slopeSideAngle;

    /// <summary>
    /// Don't know what this is for
    /// </summary>
    private float lastSlopeAngle;

    /// <summary>
    /// New velocity for wizard
    /// </summary>
    private Vector2 newVelocity;

    /// <summary>
    /// For jumping
    /// </summary>
    private Vector2 newForce;

    // ---------------- Components ---------------------
    private BoxCollider2D collider;
    private Rigidbody2D rbWizard;
    private Vector2 colliderSize;

    /// <summary>
    /// Not sure
    /// </summary>
    private Vector2 slopeNormalPerp;

    // Script that manages death
    private Death deathScript;
    private bool dead = false;

    private IInteractable interactObject;

    private void Start()
    {
        // Initiate Variables
        collider = GetComponent<BoxCollider2D>();
        rbWizard = GetComponent<Rigidbody2D>();
        deathScript = GetComponent<Death>();

        colliderSize = collider.size;
    }

    // Called once per frame
    void Update()
    {
        // Update if wizard is being frozen
        isFrozen = PreserveManager.IsPreservingWizard();

        // --------------------- Movement Input ---------------------------
        
        if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.WizardLeft]))
        {
            wizardMovement = -1.0f;
        }

        else if (Input.GetKeyUp(GameManager.s_keyBinds[GameManager.KeyBind.WizardLeft]))
        {
            wizardMovement = 0.0f;
        }

        if (Input.GetKey(GameManager.s_keyBinds[GameManager.KeyBind.WizardRight]))
        {
            wizardMovement = 1.0f;
        }

        else if (Input.GetKeyUp(GameManager.s_keyBinds[GameManager.KeyBind.WizardRight]))
        {
            wizardMovement = 0.0f;
        }

        // Handles jumping
        if (Input.GetKeyDown(GameManager.s_keyBinds[GameManager.KeyBind.WizardJump]) && !isFrozen)
        {
            Jump();
        }

        if (isFrozen)
        {
            wizardMovement = 0.0f;
            rbWizard.velocity = Vector2.zero;
        }

        Flip();

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

        // Interactable objects
        if (Input.GetKeyUp(GameManager.s_keyBinds[GameManager.KeyBind.Interact]) && interactObject != null)
        {
            interactObject.Interact();
        }

    }

    // Called 50 times per second
    private void FixedUpdate()
    {
        CheckGrounded();

        if (!dead)
        {
            SlopeCheck();
            ApplyMovement();
        }
        else
        {
            deathScript.Die();
        }
    }

    // Checks if player is on the ground
    private void CheckGrounded()
    {
        // Check if on ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // If not moving -> can jump
        if (rbWizard.velocity.y == 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
        }

        if (isGrounded && rbWizard.velocity.y < maxYVelocity)
        {
            dead = true;
        }
        else {
            dead = false;
        }
    }

    /// <summary>
    /// Checks if there is a slope in front or wizard is on the slope
    /// </summary>
    private void SlopeCheck()
    {
        // Get position at the feet of the wizard
        Vector2 checkPos = collider.bounds.center - (Vector3)(new Vector2(0.0f, colliderSize.y / 10.02f));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    /// <summary>
    /// Check if there is a slope to the front of the wizard
    /// </summary>
    /// <param name="checkPos"></param>
    private void SlopeCheckHorizontal(Vector2 checkPos)
    {

        Vector2 front = isFacingRight ? transform.right : -transform.right;

        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, front, slopeCheckDistance, groundLayer);        

        if (slopeHitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    /// <summary>
    /// Check if wizard is on a slope
    /// </summary>
    /// <param name="checkPos"></param>
    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }
    }

    /// <summary>
    ///  Actually move the wizard
    /// </summary>
    private void ApplyMovement()
    {
        if (!isFrozen)
        {

            if (isGrounded && !isOnSlope && !isJumping) //if not on slope
            {
                newVelocity.Set(movementSpeed * wizardMovement, 0.0f);
                rbWizard.velocity = newVelocity;
            }
            else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) //If on slope
            {
                newVelocity.Set(movementSpeed * slopeNormalPerp.x * -wizardMovement, movementSpeed * slopeNormalPerp.y * -wizardMovement);
                rbWizard.velocity = newVelocity;
            }
            else if (!isGrounded) //If in air
            {
                newVelocity.Set(movementSpeed * wizardMovement, rbWizard.velocity.y);
                rbWizard.velocity = newVelocity;       
            }
        }

    }

    /// <summary>
    /// Make the wizard jump
    /// </summary>
    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            isJumping = true;
            newVelocity.Set(0.0f, 0.0f);
            rbWizard.velocity = newVelocity;
            newForce.Set(0.0f, jumpingPower);
            rbWizard.AddForce(newForce, ForceMode2D.Impulse);
        }
    }


    // Handles flipping sprite
    private void Flip()
    {
        if (isFacingRight && wizardMovement < 0f || !isFacingRight && wizardMovement > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Interactable Objects

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IInteractable interactableObject))
        {
            interactObject = interactableObject;
            interactableObject.ShowInteractable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IInteractable interactableObject))
        {
            interactObject = null;
            interactableObject.RemoveInteractable();
        }
    }
}