using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorPlanet : MonoBehaviour
{
    private GameManager GameManager;

    private SpawnerSpaceObject PlanetManager;
    void Start()
    {
       this.GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.PlanetManager = GameObject.Find("SpawnerPlanet").GetComponent<SpawnerSpaceObject>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            this.GameManager.AddScore(GameManager.Gain.ASTEROID_DODGED);
        }
        if (other.gameObject.tag == "Planet")
        {
            this.GameManager.AddScore(GameManager.Gain.PLANET);
            this.PlanetManager.LaunchSpaceObject(1);
        }
    }
}
