using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.Timeline;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerCombat1 : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float coolDown;
    private bool onCD;

    public HealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;

    public int attackDamage;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        onCD = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime) 
        { 

            if(Input.GetKeyDown(KeyCode.L))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    IEnumerator CoolDownDmg()
    {
        onCD = true;
        yield return new WaitForSeconds(coolDown);
        onCD = false; 
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
    

    void Die()
    {
        animator.SetBool("IsDead", true);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D player1 in hitEnemies)
        {
            player1.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
