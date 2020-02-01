using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectMouvement : MonoBehaviour
{
    private float velocity = 0.3f;
    public int LevelDistance = 1;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)DateTime.Now.Ticks);
        int rdmResult = (int) (Random.value * 75);
        if(rdmResult <= 10)
            LevelDistance = 1;
        if (rdmResult > 10 && LevelDistance <= 25)
            LevelDistance = 2;
        if (rdmResult > 25 && LevelDistance <= 50)
            LevelDistance = 3;
        if (rdmResult > 50 && LevelDistance <= 75)
            LevelDistance = 4;
        transform.localScale /= LevelDistance;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * velocity / LevelDistance * Time.deltaTime;
    }

    public void OnBecameInvisible()
    {
        GetComponentInParent<SpawnerPlanet>().PlanetRemove();
        Destroy(gameObject);
    }
}
