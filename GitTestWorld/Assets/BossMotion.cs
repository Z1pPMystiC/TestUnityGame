using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossMotion : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsPlayer;

    public Animator animator;

    public int currentHealth;

    public HealthBarScript healthBar;

    public int playerDamage;

    public float attackDelay;

    public playerMotor playerMotor;

    public int damageToPlayer;

    public float damageDelay;

    public int projectileDamage;

    public Slider bossHealth;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float attackRange;
    public bool playerInAttackRange;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        player = GameObject.Find("PlayerObject").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        attackDelay = playerMotor.attackDelayCurrent;

        if (!playerInAttackRange && animator.GetBool("isAttacking") == false)
        {
            ChasePlayer();
        }
            
        if (playerInAttackRange)
        {
            AttackPlayer();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            DestroyBoss();
        }

        if (healthBar.IsDead())
        {
            DestroyBoss();
        }

        bossHealth.value = currentHealth;
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
        animator.SetBool("isAttacking", true);

        if (!alreadyAttacked)
        {
            Invoke("TakeDamage", damageDelay);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
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
            playerMotor.SetBossDead(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AttackEnemy(projectileDamage);
    }

    public void TakeDamage() {
        healthBar.TakeDamage(damageToPlayer);
    }

}
