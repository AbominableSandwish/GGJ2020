using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public bool isDetected;
    private bool IsFollowing = false;

    private Transform Target;
    [SerializeField] private Vector3 positionOffSet;

    // Update is called once per frame
    void Update()
    {
        if (IsFollowing)
        {
            //transform.parent = Target.transform;
            //positionOffSet
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerDetector")
        {
            if (this.isDetected != true)
            {
                this.isDetected = true;
                transform.localScale += new Vector3(0.1f, 0.05f, 0.0f);
                transform.localPosition += new Vector3(0.0f, 0.05f, 0.0f);
                transform.GetComponent<SpriteRenderer>().color += new Color(0.15f, 0.15f, 0.15f);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerDetector")
        {
            if (this.isDetected != false)
            {
                this.isDetected = false;
                transform.localScale -= new Vector3(0.1f, 0.05f, 0.0f);
                transform.localPosition -= new Vector3(0.0f, 0.05f, 0.0f);
                transform.GetComponent<SpriteRenderer>().color -= new Color(0.15f, 0.15f, 0.15f);
            }
        }
    }

    public void SetFollowing(Transform target)
    {
        if (target != null)
        {
            IsFollowing = true;
            this.Target = target;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            transform.parent = Target.transform;
            transform.localPosition = Vector3.zero;
            transform.localPosition += positionOffSet*1.5f;

        }
        else
        {
            
            IsFollowing = false;
            this.Target = null;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            transform.localPosition -= positionOffSet * 1.5f;
            transform.parent = GameObject.Find("Spatialship").transform;
            transform.localPosition += new Vector3(0.0f, 0.1f, 0.0f);

        }
    }
}
