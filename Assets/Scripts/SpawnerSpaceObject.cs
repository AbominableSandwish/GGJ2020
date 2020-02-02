using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerSpaceObject : MonoBehaviour
{
    [SerializeField] private Vector2 Size;

    private int SpaceObjectToLaunch = 15;
    private float timeToSpawn = 1.5f;
    private float counterTime=0.0f ;

    [SerializeField] List<GameObject> SpaceObject;

    // Update is called once per frame
    void Update()
    {
        if (SpaceObjectToLaunch != 0)
        {
            counterTime += Time.deltaTime;
            if (counterTime >= timeToSpawn)
            {
                int rdm = (int) DateTime.Now.Ticks;
                Random.InitState(rdm);

                if (Random.value * 100 <= 35)
                {
                    Vector3 new_position = new Vector3(transform.position.x + Random.Range(0, Size.x),
                                               transform.position.y + Random.value * Size.y) - (Vector3) Size / 2;
                    Instantiate(SpaceObject[(int) (Random.value * SpaceObject.Count)], new_position, Quaternion.identity,
                        transform);
                    SpaceObjectToLaunch++;

                }
                counterTime = 0.0f;

            }
        }
    }

    public void LaunchSpaceObject(int nbr)
    {
        SpaceObjectToLaunch += nbr;
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
