using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public LayerMask groundLayer, playerLayer;
    public float health;

    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public float patrolTimer;
    public float patrolCooldown;

    public float attackCooldown;
    public bool attacked;

    public float sightRange, attackRange;
    public bool inSightRange, inAttackRange;

    public Animator animator;
    public float movementThreshold = 0.1f;

    private void Awake()
    {
        player = GameObject.Find("FirstPersonController").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        inSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        bool isMoving = enemy.velocity.magnitude > movementThreshold;
        animator.SetBool("isRunning", isMoving);

        if (!inSightRange && !inAttackRange)
        {
            Patrolling();
        }
        if (inSightRange && !inAttackRange)
        {
            Chasing();
        }
        if (inSightRange && inAttackRange)
        {
            Attacking();
        }
    }

    private void Patrolling()
    {
        patrolTimer += Time.deltaTime;

        if (!walkPointSet || (patrolTimer >= patrolCooldown))
        {
            SearchWalkPoint();
            patrolTimer = 0f;
        }

        if (walkPointSet)
        {
            enemy.SetDestination(walkPoint);
        }

        // Vector3 distanceToWalkPoint = transform.position - walkPoint;
    }

    private void Chasing()
    {
        enemy.SetDestination(player.position);
        patrolTimer = 0f;
    }

    private void Attacking()
    {
        enemy.SetDestination(transform.position);
        patrolTimer = 0f;

        // transform.LookAt(player);

        if (!attacked)
        {
            attacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y + 50f, transform.position.z + randomZ);

        RaycastHit hit;

        if (Physics.Raycast(potentialPoint, Vector3.down, out hit, 75f, groundLayer))
        {
            walkPoint = hit.point;
            walkPointSet = true;
            Debug.Log("Walk point found: " + walkPoint);
        }
        else
        {
            Debug.Log("Could not find new walk point");
        }
    }

    private void ResetAttack()
    {
        attacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), .5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
