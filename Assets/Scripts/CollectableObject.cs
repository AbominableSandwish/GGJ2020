using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    private bool IsFollowing = false;

    private Transform Target;

    // Update is called once per frame
    void Update()
    {
        if (IsFollowing)
        {
            transform.position = Target.position;
        }
    }

    public void SetFollowing(Transform target)
    {
        if (target != null)
        {
            IsFollowing = true;
            this.Target = target;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else
        {
            
            IsFollowing = false;
            this.Target = null;
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
