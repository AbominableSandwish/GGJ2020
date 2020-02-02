using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedSystem : MonoBehaviour
{
    public void Sleep(bool set)
    {
        GetComponent<Animator>().SetBool("IsSleeping", set);
    }
}
