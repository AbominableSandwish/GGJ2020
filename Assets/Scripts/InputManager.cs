using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            if ((Input.mouseScrollDelta.y < 0 || GameObject.Find("Spatialship").transform.localScale.y >= 0.4f) &&
                (Input.mouseScrollDelta.y > 0 || GameObject.Find("Spatialship").transform.localScale.y <= 1.0f))
            {
                GameObject.Find("Spatialship").transform.localScale -= Vector3.one * (Input.mouseScrollDelta.y / 10);
                GameObject.Find("FirePropulseur").transform.localScale -= Vector3.one * (Input.mouseScrollDelta.y / 10);
            }
        }
    }
}
