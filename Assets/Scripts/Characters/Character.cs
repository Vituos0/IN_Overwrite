using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{   
    [Header("Property")]
    [SerializeField] private float HealthPoint;
    [SerializeField] private float DefensePoint;
    [SerializeField] private float AttackPoint;

    private float currentHealthPoint;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        currentHealthPoint = HealthPoint;   
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void TakeDamage(float damage)
    {
        currentHealthPoint -= Mathf.Max(0, damage - DefensePoint);
        //play hit animation
        if(currentHealthPoint <= 0)
        {
            Die();
        }
        
    }

    protected virtual void Attack(Character target)
    {
        //play attack animation
        target.TakeDamage(AttackPoint); 
    }

    protected virtual void Die()
    {   
        isDead = true;
        //play die animation
        //disable character
    }

    
}
