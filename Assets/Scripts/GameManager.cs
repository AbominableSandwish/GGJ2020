using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Rooms;
    [SerializeField] private Text textLife;

    private RoomSystem[] rooms;
    // Start is called before the first frame update
    void Start()
    {
        rooms = Rooms.GetComponentsInChildren<RoomSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentLife = 0;
        foreach (var room in rooms)
        {
            currentLife += room.GetHealt();
        }

        textLife.text = "LIFE: " + currentLife +" / "+ 45 * rooms.Length;
    }

    public void LightRoom()
    {
       
        rooms[Random.Range(0, rooms.Length)].LightRoom(true);
    }
}
