using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlanet : MonoBehaviour
{
    [SerializeField] private Vector2 Size;

    private const int MAX_PLANET = 5;
    private int countPlanet = 0;

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
            int rdm = Random.Range(0, 101);
            if (rdm >= 100)
            {
                Vector3 new_position = new Vector3(transform.position.x + Random.Range(0, Size.x),
                                           transform.position.y + Random.Range(0, Size.y)) - (Vector3) Size / 2;
                Instantiate(Planets[Random.Range(0, Planets.Count)], new_position, Quaternion.identity, transform);
                countPlanet++;
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
