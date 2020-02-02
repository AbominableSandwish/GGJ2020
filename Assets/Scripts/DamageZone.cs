using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageQuantity = 1;
    public float damageInterval = 1f;

    private BoxCollider2D boxCollider2D;

    private Player player = null;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();    
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            StartDamages(collider.gameObject.GetComponent<Player>());
        }
    }
    
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            StopDamages();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= damageInterval)
            {
                player.InflictDamage(damageQuantity);
                elapsedTime -= damageInterval;
            }
        }
    }

    private void StartDamages(Player player)
    {
        this.player = player;
        elapsedTime = 0;
    }

    private void StopDamages()
    {
        player = null;


    }
}
