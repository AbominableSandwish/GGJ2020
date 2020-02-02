using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSystem : MonoBehaviour
{
    [SerializeField] private List<DoorSystem> doors;
    // Start is called before the first frame update
    private bool withoutOxygen = false;
    public bool onFire = false;
    private float timeToBurn = 1.5f;
    private float counterTime = 0.0f;

    private int HealtRoom = 45;

    private GameObject have;

    float LevelFire = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onFire)
        {
            counterTime += Time.deltaTime;
            if (counterTime >= timeToBurn)
            {
                counterTime = 0.0f;
                LevelFire += 0.2f;
                if (LevelFire > 1.5f)
                {
                    if(HealtRoom != 0)
                        HealtRoom -= 3;
                    if (HealtRoom <= 15)
                    {
                        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0, 1);
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().color = Color.red;
                    }

                    foreach (var door in doors)
                    {
                        door.BurnRoom(have);
                    }
                }
            }
        }
    }

    public void Depressurize()
    {
        withoutOxygen = !withoutOxygen;
        if (withoutOxygen)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        foreach (var door in doors)
        {
            door.SetPressurize(withoutOxygen);
        }
    }

    public void LightRoom(bool set, GameObject fire)
    {
        onFire = set;
        if (onFire)
        {
            have = fire;
            Instantiate(fire, transform.position, Quaternion.identity, transform);
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            counterTime = 0.0f;
        }
    }

    public void FireExtinction()
    {
        LightRoom(false, null);
        have = null;
        LevelFire = 0.0f;
    }

    public int GetHealt()
    {
        return HealtRoom;
    }
}
