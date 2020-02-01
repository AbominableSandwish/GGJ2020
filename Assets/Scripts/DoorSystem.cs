using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorSystem : MonoBehaviour
{
    enum State
    {
        OPEN,
        CLOSE,
        WAIT
    }

    private Vector3 NextPositionTop;
    private Vector3 NextPositionBot;

    private State door = State.WAIT;

    [SerializeField] private bool isOpen = false;
    [SerializeField] private bool withoutOxygen = false;

    private Transform DoorTop;
    private Transform DoorBot;

    [SerializeField] private List<RoomSystem> Rooms;

    // Start is called before the first frame update
    void Start()
    {
        this.DoorTop = GetComponentsInChildren<Transform>()[1];
        this.DoorBot = GetComponentsInChildren<Transform>()[2];
    }

    void Update()
    {
        switch (door)
        {
            case State.OPEN:
                if (this.DoorTop.localPosition.y <= NextPositionTop.y && this.DoorBot.localPosition.y >= NextPositionBot.y)
                {
                    Debug.Log("Door Opening");
                    this.DoorTop.localPosition += Vector3.up * Time.deltaTime;
                    this.DoorTop.gameObject.GetComponent<SpriteRenderer>().size =
                        this.DoorTop.gameObject.GetComponent<SpriteRenderer>().size + new Vector2(0.0f, -0.01f);
                    this.DoorBot.localPosition += Vector3.down * Time.deltaTime;
                    this.DoorBot.gameObject.GetComponent<SpriteRenderer>().size =
                        this.DoorBot.gameObject.GetComponent<SpriteRenderer>().size + new Vector2(0.0f, -0.01f);
                }
                else
                {
                    Debug.Log("Door Opened");
                    this.DoorTop.localPosition = NextPositionTop;
                    this.DoorBot.localPosition = NextPositionBot;
                    door = State.WAIT;
                }

                break;
            case State.CLOSE:
                if (this.DoorTop.localPosition.y >= NextPositionTop.y && this.DoorBot.localPosition.y <= NextPositionBot.y)
                {
                    Debug.Log("Door Closing");
                    this.DoorTop.localPosition -= Vector3.up * Time.deltaTime;
                    this.DoorTop.gameObject.GetComponent<SpriteRenderer>().size =
                        this.DoorTop.gameObject.GetComponent<SpriteRenderer>().size + new Vector2(0.0f, 0.01f);
                    this.DoorBot.localPosition -= Vector3.down * Time.deltaTime;
                    this.DoorBot.gameObject.GetComponent<SpriteRenderer>().size =
                        this.DoorBot.gameObject.GetComponent<SpriteRenderer>().size + new Vector2(0.0f, +0.01f);
                }
                else
                {
                    Debug.Log("Door Closed");
                    this.DoorTop.localPosition = NextPositionTop;
                    this.DoorBot.localPosition = NextPositionBot;
                    door = State.WAIT;
                }

                break;
        }
    }

    // Update is called once per frame
    public void SetDoor()
    {
        if (door == State.WAIT)
        {
            isOpen = !isOpen;
            if (isOpen)
            {
                NextPositionTop = this.DoorTop.localPosition + new Vector3(0.0f, 0.25f);
                NextPositionBot = this.DoorBot.localPosition + new Vector3(0.0f, -0.25f);
                door = State.OPEN;
            }
            else
            {
                NextPositionTop = this.DoorTop.localPosition - new Vector3(0.0f, 0.25f);
                NextPositionBot = this.DoorBot.localPosition - new Vector3(0.0f, -0.25f);
                door = State.CLOSE;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SetDoor();
        }
    }

    public void SetPressurize(bool set)
    {
        withoutOxygen = set;
        if (withoutOxygen)
        {
            this.DoorTop.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            this.DoorBot.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            this.DoorTop.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            this.DoorBot.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void BurnRoom()
    {
        if (Random.Range(0, 100) <= 25)
        {
            foreach (var room in Rooms)
            {
                if (!room.onFire)
                {
                    room.LightRoom(true);
                }
            }
        }
    }
}
