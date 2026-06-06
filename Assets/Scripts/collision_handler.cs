using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision_handler : MonoBehaviour
{

    [Header("settings")]
    [SerializeField] private int  Damage = 1;
    [SerializeField] private float damageCooldown = .5f;

    private float lastDamageTime = 0f;
     void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ProcessHit(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ProcessHit(collision.gameObject);
        }
    }

    private void ProcessHit(GameObject enemyObject)

    {
        if(Time.time <lastDamageTime + damageCooldown)
            return; // Still in cooldown, ignore the hit
        lastDamageTime = Time.time; // Update the last damage time

        Debug.Log($"Hit by {enemyObject.name}, dealing {Damage} damage!");

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
