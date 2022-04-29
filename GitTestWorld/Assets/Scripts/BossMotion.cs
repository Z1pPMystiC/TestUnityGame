using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class BossMotion : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsPlayer;

    public Animator animator;

    public int currentHealth;
    public int maxHealth;

    public HealthBarScript healthBar;

    public playerMotor playerMotor;

    public int damageToPlayer;

    public float damageDelay;

    public int floppyDamage, teslaDamage;

    public Slider bossHealth;
    public TextMeshProUGUI bossNameText;
    public TextMeshProUGUI bossHealthText;

    public string bossName;

    public bool playerInBossArena = false;

    public bool enemyHit = false;

    public RaycastGun tesla;

    public WeaponSwaper weaponSelected;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float attackRange;
    public bool playerInAttackRange;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        bossHealth.value = 0;
        bossNameText.SetText("");
        bossHealthText.SetText("");
        bossHealth.maxValue = maxHealth;
    }

    private void Awake()
    {
        player = GameObject.Find("PlayerObject").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange && animator.GetBool("isAttacking") == false)
        {
            ChasePlayer();
        }
            
        if (playerInAttackRange)
        {
            AttackPlayer();
        }

        if (playerInBossArena)
        {
            bossHealth.value = currentHealth;
            bossNameText.SetText(bossName);
            bossHealthText.SetText(currentHealth + " / " + maxHealth + " HP");
        }

        if (enemyHit && weaponSelected.selectedWeapon == 0)
        {
            AttackEnemy(floppyDamage);
            playerMotor.enemyHit = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false) ;

        if (!alreadyAttacked)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);

            Invoke("TakeDamage", damageDelay);

            alreadyAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void DestroyBoss()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Boss"))
        {
            Destroy(enemy);
        }

    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }

    public void AttackEnemy(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && tag == "Boss")
        {
            currentHealth = 0;
            playerMotor.SetBossDead(true);
            Destroy(gameObject);
            tesla.myList.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        enemyHit = true;
        Invoke("ClearHit", 0.05f);
    }

    private void ClearHit()
    {
        enemyHit = false;
    }

    public void SetPlayerInBossArena(bool boolean) {
        playerInBossArena = boolean;
    }

    public void TakeDamage()
    {
        healthBar.TakeDamage(damageToPlayer);
    }

}
