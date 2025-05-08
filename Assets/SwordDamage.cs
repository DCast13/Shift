using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordDamage : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public PlayerCombat playerCombat;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && playerCombat != null && playerCombat.isAttacking)
        {
            EnemyBehavior enemyBehavior = other.GetComponent<EnemyBehavior>();
            if (enemyBehavior != null)
            {
                enemyBehavior.TakeDamage(damage);
            }
        }
    }
}
