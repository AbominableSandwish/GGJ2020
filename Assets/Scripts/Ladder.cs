using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private GameObject topPlatform;
    
    // Start is called before the first frame update
    void Start()
    {
        topPlatform = transform.Find("Top Platform").gameObject;
    }

    public void ActivateTopPlatform(bool active)
    {
        topPlatform.SetActive(active);
    }
}
