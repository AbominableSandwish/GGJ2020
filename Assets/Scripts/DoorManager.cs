using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoorManager : MonoBehaviour
{
    private GameManager gameManager;
    private DoorSystem[] doors;
    private Button[] doorbtns;

    [Header("Open/Close all doors")]
    [SerializeField] private bool isOpen = false;

    private float timeToChangeButton = 0.2f;
    private bool isAction = false;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        doors = GameObject.Find("Doors").GetComponentsInChildren<DoorSystem>();
        doorbtns= GameObject.Find("PanelDoorButtons").GetComponentsInChildren<Button>();
        doorbtns[0].gameObject.SetActive(false);
        doorbtns[1].gameObject.SetActive(false);
    }

    enum DoorsState
    {
        OPEN,
        CLOSE,
        AUTO,
    }

    private DoorsState doorsState = DoorsState.AUTO;

    public void buttonSetDoors()
    {
        switch (doorsState)
        {
            case DoorsState.AUTO:
                SetIsAuto(false);
                doorsState = DoorsState.OPEN;
                SetAllDoors(true);
                doorbtns[2].gameObject.SetActive(false);
                doorbtns[0].gameObject.SetActive(true);
                break;
            case DoorsState.OPEN:
                doorsState = DoorsState.CLOSE;
                SetAllDoors(false);
                doorbtns[0].gameObject.SetActive(false);
                doorbtns[1].gameObject.SetActive(true);
                break;
            case DoorsState.CLOSE:
                doorsState = DoorsState.AUTO;
                SetAllDoors(false);
                SetIsAuto(true);
                doorbtns[1].gameObject.SetActive(false);
                doorbtns[2].gameObject.SetActive(true);
                break;

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            buttonSetDoors();
        }
    }

    public void SetIsAuto(bool isAuto)
    {
        foreach (DoorSystem door in doors)
        {
            door.SetIsAuto(isAuto);
        }
    }


    public void SetAllDoors(bool isOpen)
    {
        foreach(DoorSystem door in doors)
        {
            if (isOpen == true)
            {
                door.SetAllClose(false);
                door.SetAllOpen(true);
                
            }

            if (isOpen == false)
            {
                door.SetAllClose(true);
                door.SetAllOpen(false);
            }
        }
    }

    public void SetDoor(int nbr,bool isOpen)
    {
        doors[nbr-1].SetDoor(isOpen);  
    }
}
