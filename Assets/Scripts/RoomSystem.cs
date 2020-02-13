using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSystem : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private List<DoorSystem> doors;
    // Start is called before the first frame update
    private bool withoutOxygen = false;
    public bool onFire = false;
    private float timeToBurn = 1.5f;
    private float counterTime = 0.0f;

    private int HealthRoom = 30;

    private GameObject danger;
    private List<GameObject> dangers; 

    float LevelFire = 0.0f;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dangers = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (withoutOxygen)
        {
            if (HealthRoom >= 0)
                HealthRoom -= 2;
        }


        if (onFire)
        {
            counterTime += Time.deltaTime;
            if (counterTime >= timeToBurn)
            {
                counterTime = 0.0f;
                LevelFire += 0.2f;
                if (LevelFire > 0.8f)
                {
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    dangers.Add(Instantiate(danger, transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.0f, 1f)), Quaternion.identity, transform));
                }

                if (LevelFire > 1.6f)
                {
                    if(HealthRoom >= -15)
                        HealthRoom -= 5;
                    if (HealthRoom <= 20)
                    {
                        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0, 1);
                        GameObject Fire = Instantiate(danger,
                            transform.position + new Vector3(Random.Range(-1f, 1f), 0),
                            Quaternion.identity, transform);
                        Fire.GetComponent<Animator>().SetTrigger("Danger");
                        dangers.Add(Fire);
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().color = Color.red;
                        GameObject Fire = Instantiate(danger,
                            transform.position + new Vector3(Random.Range(-1f, 1f), 0),
                            Quaternion.identity, transform);
                        Fire.GetComponent<Animator>().SetTrigger("Danger");
                        dangers.Add(Fire);
                    }

                    foreach (var door in doors)
                    {
                        door.BurnRoom(danger);
                    }
                }
            }
        }
    }

    public void Depressurize(bool set, GameObject hole)
    {
        withoutOxygen = set;
        if (withoutOxygen)
        {
            danger = hole;
            dangers.Add(Instantiate(hole, transform.position, Quaternion.identity, transform));
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
        else
        {
             foreach (var danger in dangers)
            {
                Destroy(danger);
            }
            dangers.Clear();
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void LightRoom(bool set, GameObject fire)
    {
        onFire = set;
        if (onFire)
        {
            danger = fire;
            dangers.Add(Instantiate(fire, transform.position, Quaternion.identity, transform));
        }
        else
        {
            foreach (var danger in dangers)
            {
                Destroy(danger);
            }
            dangers.Clear();
            if (HealthRoom <= 20)
            {
                GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
                counterTime = 0.0f;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                counterTime = 0.0f;
            }
        
        }
    }

    public void FireExtinction()
    {
        if (danger != null)
        {
            LightRoom(false, null);
            danger = null;
            LevelFire = 0.0f;
            
        }
    }

    public void RoomPluged()
    {
        Depressurize(false, null);
        danger = null;
    }

    public bool GetOnFire()
    {
        return onFire;
    }
    public bool GetOxygen()
    {
        return withoutOxygen;
    }

    public int GetHealth()
    {
        return HealthRoom;
    }
}
