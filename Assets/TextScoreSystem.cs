using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScoreSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "Score: " + GameObject.Find("GameManager").GetComponent<GameManager>().GetScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
