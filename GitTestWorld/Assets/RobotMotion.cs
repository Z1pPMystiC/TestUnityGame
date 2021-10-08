using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMotion : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator;

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
        if (playerInAttackRange) AttackPlayer();
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
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


}
