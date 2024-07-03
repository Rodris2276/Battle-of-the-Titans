using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int maxHealth = 100;
    public RectTransform healthTransform;
    private float cachedY;
    private float minXValue;
    private float maxXValue;
    public Text healthText;
    public UnityEngine.UI.Image visualHealth;
    public float coolDown;
    private bool onCD;

    int currentHealth;
    public int attackDamage = 20;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
    void Start()
    {
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = healthTransform.position.x - healthTransform.rect.width;
        currentHealth = maxHealth;
        onCD = false;
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

     private int CurrentHealth
    {
        get{ return currentHealth;}
        set { 
            currentHealth = value;
            HandleHealth();
        
        }
    }

    private float Mapvalues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    private void HandleHealth()
    {
        healthText.text = "Health: " + currentHealth;
        float currentXValue = Mapvalues(currentHealth, 0, maxHealth, minXValue, maxXValue);
        healthTransform.position = new Vector3(currentXValue, cachedY);

        if (currentHealth > maxHealth/2)// Then I have more than 50% 
        {
            visualHealth.color = new Color32((byte) Mapvalues(currentHealth, maxHealth/2, maxHealth, 255, 0) , 255, 0, 255);
        }
        else //Less then 50% 
        {
            visualHealth.color = new Color32(255, (byte) Mapvalues(currentHealth, 0, maxHealth/2, 0, 255), 0, 255);
        }
    }

    IEnumerator CoolDownDmg()
    {
        onCD = true;
        yield return new WaitForSeconds(coolDown);
        onCD = false; 
    }

    void OnTriggerStay(Collider other)
    {
        if(other.name =="Damage"){
            if (!onCD && currentHealth > 0)
            {
                StartCoroutine(CoolDownDmg());
                CurrentHealth -=1;
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
    }

    void Die()
    {
        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D player2 in hitEnemies)
        {
            player2.GetComponent<PlayerCombat1>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
