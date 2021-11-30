using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMotion : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public GameObject playerBody;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator;

    public int currentHealth;

    public HealthBarScript healthBar;

    public int playerDamage;

    public float attackDelay;

    public playerMotor playerMotor;

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

        if (!playerInAttackRange)
        {
            ChasePlayer();
        }
            
        if (playerInAttackRange)
        {
            AttackPlayer();
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackDelay <= 0.02)
            {
                AttackEnemy(playerDamage);
            }
            else
            {
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            DestroyEnemies();
        }

        if (healthBar.IsDead())
        {
            DestroyEnemies();
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
        animator.SetBool("isAttacking", true);

        if (!alreadyAttacked)
        {
            healthBar.TakeDamage(2);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void DestroyEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
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

        if (currentHealth <= 0 && tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AttackEnemy(100);
    }
}
