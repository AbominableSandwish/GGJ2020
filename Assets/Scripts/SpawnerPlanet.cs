using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerPlanet : MonoBehaviour
{
    [SerializeField] private Vector2 Size;

    private const int MAX_PLANET = 5;
    private int countPlanet = 0;
    private float timeToSpawn = 2.5f;
    private float counterTime=0.0f ;

    [SerializeField] List<GameObject> Planets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countPlanet != MAX_PLANET)
        {
            counterTime += Time.deltaTime;
            if (counterTime >= timeToSpawn)
            {
                int rdm = (int) DateTime.Now.Ticks;
                Random.InitState(rdm);

                if (Random.value * 100 <= 20)
                {
                    Vector3 new_position = new Vector3(transform.position.x + Random.Range(0, Size.x),
                                               transform.position.y + Random.value * Size.y) - (Vector3) Size / 2;
                    Instantiate(Planets[(int) (Random.value * Planets.Count)], new_position, Quaternion.identity,
                        transform);
                    countPlanet++;

                }
                counterTime = 0.0f;

            }
        }
    }

    public void PlanetRemove()
    {
        countPlanet--;
    }

    void OnDrawGizmosSelected()
    {
        if (Size != Vector2.zero)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position - (Vector3) Size / 2,
                (transform.position - (Vector3) Size / 2) + Vector3.right * Size.x);
            Gizmos.DrawLine(((transform.position - (Vector3) Size / 2) + Vector3.right * Size.x),
                (transform.position + (Vector3) Size / 2));
            Gizmos.DrawLine((transform.position + (Vector3) Size / 2), (transform.position + (Vector3)Size / 2) - Vector3.right * Size.x);
            Gizmos.DrawLine((transform.position + (Vector3) Size / 2) - Vector3.right * Size.x,
                transform.position - (Vector3) Size / 2);
        }
    }
}
