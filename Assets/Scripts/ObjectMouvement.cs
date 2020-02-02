using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectMouvement : MonoBehaviour
{
    [SerializeField] private float velocity = 0.3f;
    public float LevelDistance = -1;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)DateTime.Now.Ticks);
        if (LevelDistance == -1)
        {
            int rdmResult = (int) (Random.value * 75);
            if (rdmResult <= 10)
            {
                LevelDistance = 2;
                GetComponent<SpriteRenderer>().sortingOrder = -5;
            }

            if (rdmResult > 10 && LevelDistance <= 25)
            {
                LevelDistance = 1;
                GetComponent<SpriteRenderer>().sortingOrder = -4;
            }

            if (rdmResult > 25 && LevelDistance <= 50)
            {
                LevelDistance = 0.5f;
                GetComponent<SpriteRenderer>().sortingOrder = -3;
            }

            if (rdmResult > 50 && LevelDistance <= 75)
            {
                LevelDistance = 0.25f;
                GetComponent<SpriteRenderer>().sortingOrder = -2;
            }

            transform.localScale /= LevelDistance;
        }

        GetComponent<Rigidbody2D>().velocity.Set(-1*10, 0);
        GetComponent<Rigidbody2D>().AddForce(Vector2.left*35);
    }

    

    // Update is called once per frame
    void Update()
    {
       // transform.position += Vector3.left * velocity / LevelDistance * Time.deltaTime;
    }

    public void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
