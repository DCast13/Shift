using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackHandler : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    private EnemyBehavior enemyBehavior;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        enemyBehavior = GetComponentInParent<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemyBehavior != null && enemyBehavior.IsAttacking())
        {
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(damage);
                Debug.Log("Player hit");
            }
        }
    }
}
