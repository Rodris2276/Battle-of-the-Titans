using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.Timeline;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerCombat : MonoBehaviour
{
    Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    [SerializeField] GameObject Player2;

    public HealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;

    public float attackRange = 0.5f;
    public float attackRate = 2f;
    public int attackDamage;
    float nextAttackTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime) 
        { 

            if(Input.GetKeyDown(KeyCode.F))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if(currentHealth <= 0)
        {
            Die();  
        }
        healthBar.SetHealth(currentHealth);
    }

    public void DealDamage(GameObject Player2)
    {
        var atm = Player2.GetComponent<PlayerCombat1>();
        if(atm != null)
        {
            atm.TakeDamage(attackDamage);
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        GetComponent<PlayerCombat>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
    }

    void Attack()
    {
        animator.SetTrigger("Attack") ;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D Player2 in hitEnemies)
        {
            Player2.GetComponent<PlayerCombat1>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
