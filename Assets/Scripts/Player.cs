﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const bool RIGHT = true;
    private const bool LEFT = false;
    private const float MOVEMENT_SMOTHING = 0.05f;
    private const float MAX_HORIZONTAL_LADDER_OFFSET = 0.1f;

    [SerializeField] private int maxHp = 10;
    [SerializeField] private float velocity = 20;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 move = Vector2.zero;
    private bool isClimbing = false;
    private GameObject ladder = null;
    private bool orientation = RIGHT;
    private Vector3 dampVelocity = Vector3.zero;
    private float gravityScaleBackup;

    private Animator animator;
    private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        gravityScaleBackup = rigidbody2D.gravityScale;
    }

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(IsGrounded() ? "Grounded" : "Not grounded");
        
        if (other.gameObject.tag == "Ladder") {
            ladder = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log(IsGrounded() ? "Grounded" : "Not grounded");
        
        if (other.gameObject.tag == "Ladder") {
            ladder = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * velocity;
        animator.SetFloat("Running Velocity", Mathf.Abs(move.x));
    }

    void FixedUpdate()
    {
        Vector2 fixedMove = move * Time.fixedDeltaTime;

        if (
            ladder == null ||
            (move.y <= 0 && IsGrounded())
        ) {
            stopClimbing();
        } else if (move.y != 0) {
            startClimbing();
        }

        float targetVelocityX = fixedMove.x * 10f;
        float targetVelocityY = rigidbody2D.velocity.y;

        if (isClimbing) {
            float playerX = transform.position.x;
            float ladderX = ladder.transform.position.x;
            float xDiff = Mathf.Abs(playerX - ladderX);

            if (xDiff > MAX_HORIZONTAL_LADDER_OFFSET) {
                if (playerX > ladderX) {
                    targetVelocityX = -3;
                } else {
                    targetVelocityX = 3;
                }
            } else {
                targetVelocityX = 0;
            }

            targetVelocityY = fixedMove.y * 10f;
        }

        rigidbody2D.velocity = Vector3.SmoothDamp(
            rigidbody2D.velocity,
            new Vector2(targetVelocityX, targetVelocityY),
            ref dampVelocity,
            MOVEMENT_SMOTHING
        );

        if (
            (move.x > 0 && orientation != RIGHT) ||
            (move.x < 0 && orientation != LEFT)
        ) {
            Flip();
        }
    }

    private void Flip()
    {
        orientation = !orientation;

        // Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.78f, groundLayer);

        if (hit.collider != null) {
            return true;
        }
        
        return false;
    }

    private void startClimbing()
    {
        if (!isClimbing) {
            isClimbing = true;
            gravityScaleBackup = rigidbody2D.gravityScale;
            rigidbody2D.gravityScale = 0;
        }
    }

    private void stopClimbing()
    {
        if (isClimbing) {
            isClimbing = false;
            rigidbody2D.gravityScale = gravityScaleBackup;
            
            Vector2 newVelocity = rigidbody2D.velocity;
            newVelocity.y = 0;
            rigidbody2D.velocity = newVelocity;
        }
    }
}
