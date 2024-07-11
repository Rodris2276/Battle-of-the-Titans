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
    private Animator anim;
    public GameManagerScript gameManager;
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    [SerializeField] GameObject Player1;

    public HealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;

    private bool isDead;

    public float attackRange = 0.5f;
    public float attackRate = 2f;
    public int attackDamage;
    float nextAttackTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if(currentHealth <= 0 && !isDead)
        {
            isDead=true;
            gameManager.WinGame();
            Die();
        }
        healthBar.SetHealth(currentHealth);
    }

    public void DealDamage(GameObject Player1)
    {
        var atm = Player1.GetComponent<PlayerCombat>();
        if(atm != null)
        {
            atm.TakeDamage(attackDamage);
        }
    }
    
    void Die()
    {
        animator.SetBool("IsDead", true);
        GetComponent<PlayerCombat1>().enabled = false;
        GetComponent<PlayerMovement1>().enabled = false;
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D Player1 in hitEnemies)
        {
            Player1.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
