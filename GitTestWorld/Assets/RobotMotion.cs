using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RobotMotion : MonoBehaviour
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

    public Image leftCrosshair;
    public Image rightCrosshair;
    public Image upCrosshair;
    public Image downCrosshair;

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

        if (!playerInAttackRange) ChasePlayer();
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

        if(healthBar.IsDead())
        {
            DestroyEnemies();
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    private void AttackPlayer() {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        
        animator.SetBool("isRunning", false);
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
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
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
        var tempColorLeft = leftCrosshair.color;
        var tempColorRight = rightCrosshair.color;
        var tempColorUp = upCrosshair.color;
        var tempColorDown = downCrosshair.color;
        tempColorLeft.a = 1f;
        tempColorRight.a = 1f;
        tempColorUp.a = 1f;
        tempColorDown.a = 1f;
        leftCrosshair.color = tempColorLeft;
        rightCrosshair.color = tempColorRight;
        upCrosshair.color = tempColorUp;
        downCrosshair.color = tempColorDown;
        Invoke("ClearHitmarker", 0.5f);
    }

    public void ClearHitmarker()
    {
        var tempColorLeft = leftCrosshair.color;
        var tempColorRight = rightCrosshair.color;
        var tempColorUp = upCrosshair.color;
        var tempColorDown = downCrosshair.color;
        tempColorLeft.a = 0f;
        tempColorRight.a = 0f;
        tempColorUp.a = 0f;
        tempColorDown.a = 0f;
        leftCrosshair.color = tempColorLeft;
        rightCrosshair.color = tempColorRight;
        upCrosshair.color = tempColorUp;
        downCrosshair.color = tempColorDown;
    }
}
