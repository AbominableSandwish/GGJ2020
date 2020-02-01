using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Rooms;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightRoom()
    {
        RoomSystem[] rooms = Rooms.GetComponentsInChildren<RoomSystem>();
        rooms[Random.Range(0, rooms.Length)].LightRoom(true);
    }
}
