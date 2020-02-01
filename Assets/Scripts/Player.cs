using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const bool RIGHT = true;
    private const bool LEFT = false;
    private float MOVEMENT_SMOTHING = .05f;

    [SerializeField] private int maxHp = 10;
    [SerializeField] private float velocity = 20;

    private Vector2 move = Vector2.zero;
    private bool climbing = false;
    private bool orientation = RIGHT;
    private Vector3 dampVelocity = Vector3.zero;
    private Animator animator;
    private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
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

        Vector2 targetVelocity = new Vector2(fixedMove.x * 10f, rigidbody2D.velocity.y);
        rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref dampVelocity, MOVEMENT_SMOTHING);

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
}
