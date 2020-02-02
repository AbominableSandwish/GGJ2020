using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Rooms;

    [Header("Danger After Impact")]
    [SerializeField] private GameObject Fire;
    [SerializeField] private GameObject Hole;

    private Text HealthText;
    private Text ScoreText;

    private RoomSystem[] rooms;

    // EVENT //
    float LastEventTime = 60.0f;

    enum Event
    {
        Asteroid,
        Comet,
        Mist,
        Attack
    }


    enum Danger
    {
        Fire,
        Oxygen,
    }

    //////////////
    // SCORING //
    private float Score = 0.0f;

    private int PlanetFound = 0;
    private int AsteroidDodged = 0;
    private int FireExtinguished = 0;
    private int HolePluged = 0;
    private int RoomRepared = 0;

    private int roomsDestroyed = 0;

    public enum Gain
    {
        ASTEROID_DODGED = 35,
        PLANET = 25,
        FIRE_EXTINGUISHED = 4,
        HOLE_PLOGED = 3,
        REPARE = 6,
    }


    // Start is called before the first frame update
    void Start()
    {
        rooms = Rooms.GetComponentsInChildren<RoomSystem>();

        HealthText = GameObject.Find("HealthText").GetComponent<Text>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentLife = 0;
        foreach (var room in rooms)
        {
            currentLife += room.GetHealth();
        }

        HealthText.text = "HEALTH: " + currentLife +" / "+ 45 * rooms.Length;
    }

    public void AddScore(Gain action)
    {
       
        
        switch (action)
        {
            case Gain.ASTEROID_DODGED:
                AsteroidDodged++;
                break;
            case Gain.FIRE_EXTINGUISHED:
                FireExtinguished++;
                break;
            case Gain.HOLE_PLOGED:
                HolePluged++;
                break;
            case Gain.PLANET:
                PlanetFound++;
                break;
            case Gain.REPARE:
                RoomRepared++;
                break; 
        }

        this.Score += (((int)action)/6.0f);
        if (Score < 10)
        {
            ScoreText.text = "SCORE: 000" + ((int)Score).ToString();
        }
        if (Score >= 10 && Score < 100)
        {
            ScoreText.text = "SCORE: 00" + ((int)Score).ToString() ;
        }

        if (Score >= 100 && Score < 1000)
        {
            ScoreText.text = "SCORE: 0" + ((int)Score).ToString();
        }
        if (Score >= 1000)
        {
            ScoreText.text = "SCORE: " + ((int)Score).ToString();
        }
    } 

    public void LightRoom()
    {
        List<RoomSystem> FreeRooms = CheckFreeRoom();
        RoomSystem room = FreeRooms[Random.Range(0, FreeRooms.Count)];
        room.LightRoom(true, Fire);
    }

    List<RoomSystem> CheckFreeRoom()
    {
        List<RoomSystem> FreeRooms = new List<RoomSystem>();

        foreach (var room in rooms)
        {
            if (!room.onFire && !room.GetOxygen())
            {
                FreeRooms.Add(room);
            }
        }
        return FreeRooms;
    }

    public void DepressurizeRoom()
    {
        List<RoomSystem> FreeRooms = CheckFreeRoom();
        RoomSystem room = FreeRooms[Random.Range(0, FreeRooms.Count)];
        room.Depressurize(true, Hole);
    }

    public void Impact()
    {
        if (Random.Range(0, 10) <= 5)
        {
            LightRoom();
        }
        else
        {
            DepressurizeRoom();
        }

    }
}
