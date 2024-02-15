using UnityEngine;

public class FairyMovement : MonoBehaviour
{
    // Movement variables
    public float movementSpeed = 5f;
    private Vector2 fairyMovement;

    // Component variables
    [SerializeField] private Rigidbody2D rbFairy;

    // Called once per frame
    void Update()
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
    }

    // Called 50 times per second
    private void FixedUpdate()
    {
        // Update fairy movement
        rbFairy.velocity = movementSpeed * fairyMovement;
    }
}