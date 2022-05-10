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

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator;

    public int currentHealth;

    public HealthBarScript healthBar;

    public playerMotor playerMotor;

    public int damageToPlayer;

    public int floppyDamage, teslaDamage;

    public WaveSpawner waveSpawner;

    public bool enemyHit = false;

    public bool hitWithTesla = false;

    public RaycastGun tesla;

    public WeaponSwaper weaponSelected;

    public GameObject[] powerupArray;
    
    public GameObject teslaPowerup;
    public GameObject maxAmmoPowerup;
    public GameObject healthBoostPowerup;

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

        if (!playerInAttackRange) ChasePlayer();
        if (playerInAttackRange)
        {
            AttackPlayer();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            DestroyEnemies();
        }

        if(healthBar.IsDead())
        {
            DestroyEnemies();
        }
        
        if (enemyHit && weaponSelected.selectedWeapon == 0)
        {
            AttackEnemy(floppyDamage);
            playerMotor.enemyHit = true;
            Debug.Log("Floppy Hit");
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
            healthBar.TakeDamage(damageToPlayer);

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
            int rng = Random.Range(0, 100);
            if(rng <= 20)
            {
                GameObject randDrop = powerupArray[Random.Range(0, powerupArray.Length)];
                Instantiate(randDrop, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            }
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
}
