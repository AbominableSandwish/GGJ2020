using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorSystem : MonoBehaviour
{
    enum State
    {
        OPEN,
        CLOSE
    }

    private Collider2D doorCollider;

    private Vector3 OpenPositionTop;
    private Vector3 OpenPositionBot;

    private Vector3 ClosePositionTop;
    private Vector3 ClosePositionBot;

    private State door = State.CLOSE;

    [SerializeField] private bool isOpen = false;
    private bool isWaiting = true;
    private bool isAuto = true;

    private bool allOpen = false;
    private bool allClose = false;

    private Transform DoorTop;
    private Transform DoorBot;

    [SerializeField] private List<RoomSystem> Rooms;

    public GameObject PlayerDetect = null;

    // Start is called before the first frame update
    void Start()
    {
        doorCollider = GetComponentsInChildren<BoxCollider2D>()[1].gameObject.GetComponent<Collider2D>();
        this.doorCollider.gameObject.SetActive(false);

        this.DoorTop = GetComponentsInChildren<Transform>()[1];
        this.DoorBot = GetComponentsInChildren<Transform>()[2];

        ClosePositionTop = this.DoorTop.localPosition;
        ClosePositionBot = this.DoorBot.localPosition;

        OpenPositionTop = this.DoorTop.localPosition + new Vector3(0.0f, 0.25f);
        OpenPositionBot = this.DoorBot.localPosition + new Vector3(0.0f, -0.25f);
    }

    void FixedUpdate()
    {
        if (!isWaiting)
        {
            switch (door)
            {
                case State.OPEN:
                    if (this.DoorTop.localPosition.y <= OpenPositionTop.y &&
                        this.DoorBot.localPosition.y >= OpenPositionBot.y)
                    {
                        this.DoorTop.localPosition += Vector3.up / 0.7f * Time.deltaTime;
                        this.DoorTop.gameObject.GetComponent<SpriteRenderer>().size =
                            this.DoorTop.gameObject.GetComponent<SpriteRenderer>().size + new Vector2(0.0f, -0.03f);
                        this.DoorBot.localPosition += Vector3.down * Time.deltaTime;
                        this.DoorBot.gameObject.GetComponent<SpriteRenderer>().size =
                            this.DoorBot.gameObject.GetComponent<SpriteRenderer>().size + new Vector2(0.0f, -0.030f);
                    }
                    else
                    {
                        this.DoorTop.localPosition = OpenPositionTop;
                        this.DoorBot.localPosition = OpenPositionBot;
                        isWaiting = true;
                    }

                    break;
                case State.CLOSE:
                    if (this.DoorTop.localPosition.y >= ClosePositionTop.y &&
                        this.DoorBot.localPosition.y <= ClosePositionBot.y)
                    {
                        this.DoorTop.localPosition -= Vector3.up * Time.deltaTime;
                        this.DoorTop.gameObject.GetComponent<SpriteRenderer>().size =
                            this.DoorTop.gameObject.GetComponent<SpriteRenderer>().size + new Vector2(0.0f, 0.03f);
                        this.DoorBot.localPosition -= Vector3.down * Time.deltaTime;
                        this.DoorBot.gameObject.GetComponent<SpriteRenderer>().size =
                            this.DoorBot.gameObject.GetComponent<SpriteRenderer>().size + new Vector2(0.0f, +0.03f);
                    }
                    else
                    {
                        this.DoorTop.localPosition = ClosePositionTop;
                        this.DoorBot.localPosition = ClosePositionBot;
                        this.DoorTop.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(0.08f, 0.35f);
                        this.DoorBot.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(0.08f, 0.35f);
                        isWaiting = true;
                    }

                    break;
            }
        }
    }

    // Update is called once per frame
    public void SetDoor(bool set)
    {
        if (isOpen != set)
        {
            isOpen = set;
            if (isOpen)
            {
                {
                    this.doorCollider.gameObject.SetActive(false);
                    door = State.OPEN;
                }
            }
            else
            {
                this.doorCollider.gameObject.SetActive(true);
                door = State.CLOSE;
            }
            isWaiting = false;
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerDetect = other.gameObject;
            if (isAuto)
            {
                SetDoor(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {


        if (other.gameObject.tag == "Player")
        {
            PlayerDetect = null; if (isAuto)
            {
                SetDoor(false);
            }
        }

    }

    public void SetPressurize(bool set)
    {
        isAuto = !set;
    }

    public void SetIsAuto(bool isAuto)
    {
        this.allClose = false;
        this.allOpen = false;
        this.isAuto = isAuto;
        SetColordoor();
        if (isAuto)
        {
            if (PlayerDetect != null)
            {
                SetDoor(true);
            }
        }
    }

    public void SetAllOpen(bool allOpen)
    {
        this.allOpen = allOpen;
        SetDoor(allOpen);
        SetColordoor();
    }

    public void SetAllClose(bool allClose)
    {
        this.allClose = allClose;
        SetDoor(allClose);
        SetColordoor();
    }

    void SetColordoor()
    {
        if (allOpen)
        {
            this.DoorTop.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            this.DoorBot.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (allClose)
        {
            this.DoorTop.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            this.DoorBot.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (isAuto)
        {
            this.DoorTop.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.725f, 1.0f);
            this.DoorBot.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.725f, 1.0f);
        }


    }

    public void BurnRoom(GameObject fire)
    {
        if (Random.Range(0, 100) <= 25)
        {
            foreach (var room in Rooms)
            {
                if (!room.onFire)
                {
                    room.LightRoom(true, fire);
                }
            }
        }
    }
}
