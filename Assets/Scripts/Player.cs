using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Object
    {
        SPACESUIT,
        EXTINGUISHER,
        KIT_REPAIR,
        EMPTY,
    }

    private GameManager gameManager;

    [SerializeField] private Object inHand = Object.EMPTY;

    private const bool RIGHT = true;
    private const bool LEFT = false;
    private const int NOTHING = 0;
    private const int GROUND = 1;
    private const int LADDER = 2;
    private const float MOVEMENT_SMOTHING = 0.05f;
    private const float MAX_HORIZONTAL_LADDER_OFFSET = 0.1f;

    [SerializeField] private int maxHp = 10;
    [SerializeField] public float velocity = 20;
    [SerializeField] public float climbVelocity = 5;

    [SerializeField] private LayerMask groundLayer;

    private int hp;
    private bool hasBubbleSuit = false;
    private bool isUsingExtinguisher = false;
    private Vector2 move = Vector2.zero;
    private bool isClimbing = false;
    private GameObject ladder = null;
    private bool orientation = RIGHT;
    private Vector3 dampVelocity = Vector3.zero;
    private float gravityScaleBackup;

    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private ParticleSystem extinguisherParticles;

    public RoomSystem CurrentRoom;

    public GameObject ObjectCollectable;

    private bool InAction = false;
    private float TimetoAction = 0.0f;
    private bool IsSleeping = false;

    private bool isRight = true;

    public void SetRoom(RoomSystem room)
    {
        CurrentRoom = room;
    }



    // Start is called before the first frame update
    void Start()
    {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        extinguisherParticles = transform.Find("Extinguisher Particles").gameObject.GetComponent<ParticleSystem>();

        gravityScaleBackup = rigidbody2D.gravityScale;

        Physics2D.IgnoreCollision(GameObject.Find("Spatialship").GetComponent<Collider2D>(), GetComponent<Collider2D>());

        hp = maxHp;
    }

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Planet" && other.gameObject.tag != "Room" && other.gameObject.tag != "Door")
        {
            if (other.gameObject.tag == "Ladder")
            {
                ladder = other.gameObject;
            }



            if (inHand == Object.EMPTY)
            {
                if (ObjectCollectable.tag == null)
                {
                    ObjectCollectable.transform.localScale += new Vector3(0.1f, 0.1f, 0.0f);
                }

                if (other.gameObject.tag == "Extinguisher")
                {

                    ObjectCollectable = other.gameObject;
                }

                if (other.gameObject.tag == "BottleOxygen")
                {
                    ObjectCollectable = other.gameObject;
                }

                if (other.gameObject.tag == "RepairKit")
                {
                    ObjectCollectable = other.gameObject;
                }

                if (other.gameObject.tag == "Bed")
                {
                    ObjectCollectable = other.gameObject;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            other.gameObject.GetComponent<Ladder>().ActivateTopPlatform(true);
            ladder = null;
        }
            
        if (inHand == Object.EMPTY)
        {
            if (other.gameObject.tag == "Extinguisher" || other.gameObject.tag == "BottleOxygen" || other.gameObject.tag == "RepairKit")
            {
                ObjectCollectable.transform.localScale -= new Vector3(0.1f, 0.1f, 0.0f);
                ObjectCollectable = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead())
        {
            move = Vector2.zero;
            return;
        }

        animator.SetBool("Dead", false);

        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * velocity;
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            isRight = true;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            isRight = false;
        }

        animator.SetBool("IsRight", isRight);

        animator.SetFloat("Running Velocity", Mathf.Abs(rigidbody2D.velocity.x));
        animator.SetFloat("Climbing Velocity", Mathf.Abs(rigidbody2D.velocity.y));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (inHand == Object.EMPTY)
            {
                if (ObjectCollectable != null)
                {
                    if (ObjectCollectable.tag == "Extinguisher")
                    {
                        inHand = Object.EXTINGUISHER;
                    }

                    if (ObjectCollectable.tag == "BottleOxygen")
                    {
                        inHand = Object.SPACESUIT;
                        EquipBubbleSuit(!hasBubbleSuit);
                    }

                    if (ObjectCollectable.tag == "RepairKit")
                    {
                        inHand = Object.KIT_REPAIR;
                    }

                    ObjectCollectable.GetComponent<CollectableObject>().SetFollowing(transform);
                }
            }
            else
            {
                if (!InAction)
                {
                    if (inHand == Object.EXTINGUISHER || inHand == Object.KIT_REPAIR)
                    {
                        inHand = Object.EMPTY;
                        ObjectCollectable.GetComponent<CollectableObject>().SetFollowing(null);
                    }
                    if (inHand == Object.SPACESUIT)
                    {
                        inHand = Object.EMPTY;
                        ObjectCollectable.GetComponent<CollectableObject>().SetFollowing(null);
                        EquipBubbleSuit(!hasBubbleSuit);
                    }
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.E) && !isClimbing)
        {
            if (ObjectCollectable.gameObject.tag == "Bed")
            {
                IsSleeping = true;
                GetComponent<Animator>().SetBool("IsSleeping", true);
                ObjectCollectable.gameObject.GetComponent<BedSystem>().Sleep(true);
            }

            if (inHand == Object.EXTINGUISHER)
            {
                InAction = true;
                UseExtinguisher(true);
                TimetoAction = 3.0f;
            }
            if (inHand == Object.KIT_REPAIR)
            {
                animator.SetBool("IsReparing", true);
                InAction = true;
                TimetoAction = 3.0f;
            }
            if (inHand == Object.SPACESUIT)
            {
                if (CurrentRoom.GetOxygen())
                {
                    animator.SetBool("IsReparing", true);
                    InAction = true;
                    TimetoAction = 3.0f;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (ObjectCollectable.gameObject.tag == "Bed")
            {
                GetComponent<Animator>().SetBool("IsSleeping", false);
                ObjectCollectable.gameObject.GetComponent<BedSystem>().Sleep(false);
                IsSleeping = false;
            }

            if (inHand == Object.EXTINGUISHER)
            {
                UseExtinguisher(false);
                InAction = false;
            }
            if (inHand == Object.KIT_REPAIR)
            {
                animator.SetBool("IsReparing", false);
                InAction = false;
            }
            if (inHand == Object.SPACESUIT)
            {
                animator.SetBool("IsReparing", false);
                InAction = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (IsDead())
        {
            return;
        }

        if (!IsSleeping)
        {
                if (InAction)
                {
                    if (CurrentRoom.GetOnFire() || CurrentRoom.GetOxygen())
                    {
                        TimetoAction -= Time.deltaTime;
                        gameManager.GetComponent<GameManager>().AddScore(GameManager.Gain.FIRE_EXTINGUISHED);
                        if (TimetoAction <= 0.0f)
                        {
                            if (inHand == Object.EXTINGUISHER)
                            {
                                CurrentRoom.FireExtinction();
                            }

                            if (inHand == Object.SPACESUIT)
                            {
                                CurrentRoom.RoomPluged();
                            }

                            InAction = false;
                        }
                    }
                }

                Vector2 fixedMove = move * Time.fixedDeltaTime;
                int standingOn = IsStandingOn();

                if (ladder != null)
                {
                    if (fixedMove.y < 0)
                    {
                        if (standingOn == GROUND)
                        {
                            StopClimbing();
                        }
                        else if (standingOn == LADDER)
                        {
                            ladder.GetComponent<Ladder>().ActivateTopPlatform(false);
                            StartClimbing();
                        }
                    }
                    else if (fixedMove.y > 0 && standingOn != LADDER)
                    {
                        ladder.GetComponent<Ladder>().ActivateTopPlatform(true);
                        StartClimbing();
                    }
                }
                else
                {
                    StopClimbing();
                }

                float targetVelocityX = InAction ? 0 : fixedMove.x * 10f;
                float targetVelocityY = rigidbody2D.velocity.y;

                if (isClimbing)
                {
                    float playerX = transform.position.x;
                    float ladderX = ladder.transform.position.x;
                    float xDiff = Mathf.Abs(playerX - ladderX);

                    if (xDiff > MAX_HORIZONTAL_LADDER_OFFSET)
                    {
                        if (playerX > ladderX)
                        {
                            targetVelocityX = -3;
                        }
                        else
                        {
                            targetVelocityX = 3;
                        }
                    }
                    else
                    {
                        targetVelocityX = 0;
                    }
                    //speed
                    targetVelocityY = fixedMove.y * 4.5f;
                }

                rigidbody2D.velocity = Vector3.SmoothDamp(
                    rigidbody2D.velocity,
                    new Vector2(targetVelocityX, targetVelocityY),
                    ref dampVelocity,
                    MOVEMENT_SMOTHING
                );

                if (
                    !isClimbing &&
                    (move.x > 0 && orientation != RIGHT) ||
                    (move.x < 0 && orientation != LEFT)
                )
                {
                    Flip();
                }
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

    private int IsStandingOn()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.85f, groundLayer);

        if (hit.collider != null)
        {
            return hit.collider.tag == "Ladder Top Platform" ? LADDER : GROUND;
        }

        return NOTHING;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, groundLayer);

        if (hit.collider != null && hit.distance > 0.7f)
        {
            return true;
        }

        return false;
    }

    private void StartClimbing()
    {
        if (!isClimbing)
        {
            isClimbing = true;
            gravityScaleBackup = rigidbody2D.gravityScale;
            rigidbody2D.gravityScale = 0;
            animator.SetBool("Is Climbing", true);
        }
    }

    private void StopClimbing()
    {
        if (isClimbing)
        {
            isClimbing = false;
            rigidbody2D.gravityScale = gravityScaleBackup;
            animator.SetBool("Is Climbing", false);

            Vector2 newVelocity = rigidbody2D.velocity;
            newVelocity.y = 0;
            rigidbody2D.velocity = newVelocity;
        }
    }

    public Object GetObject()
    {
        return inHand;
    }

    private void EquipBubbleSuit(bool equip)
    {
        if (equip != hasBubbleSuit)
        {
            hasBubbleSuit = equip;
            transform.Find("Bubble Sprite").gameObject.SetActive(equip);
        }
    }

    private void UseExtinguisher(bool use)
    {
        if (use != isUsingExtinguisher)
        {

            if (use)
            {
                extinguisherParticles.Play();
            }
            else
            {
                extinguisherParticles.Stop();
            }

            animator.SetBool("Is Using Extinguisher", use);
            isUsingExtinguisher = use;
        }
    }

    public bool IsDead()
    {
        return hp <= 0;
    }

    public void InflictDamage(int damage)
    {
        if (!IsDead())
        {
            hp  -= damage;
            
            if (IsDead())
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        hp = 0;
        animator.SetBool("Dead", true);
        animator.SetTrigger("Death");
    }

    public void Revive(int hp)
    {
        this.hp = hp;

        if (this.hp > maxHp) {
            this.hp = maxHp;
        }

        animator.SetBool("Dead", false);
    }

    public int GetHP()
    {
        return hp;
    }
}
