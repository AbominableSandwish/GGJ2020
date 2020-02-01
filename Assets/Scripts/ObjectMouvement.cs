using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMouvement : MonoBehaviour
{
    private float velocity = 0.3f;
    [SerializeField] private int LevelDistance;
    // Start is called before the first frame update
    void Start()
    {
        LevelDistance = Random.Range(0, 4);
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
