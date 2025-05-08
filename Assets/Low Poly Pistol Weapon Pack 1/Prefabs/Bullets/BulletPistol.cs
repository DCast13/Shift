using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletPistol : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public int damage = 10;

    // Optional: destroy after 5s if it never hits anything
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBehavior enemyBehavior = other.GetComponent<EnemyBehavior>();
            if (enemyBehavior != null)
            {
                enemyBehavior.TakeDamage(damage);
                Debug.Log("Enemy Health" + enemyBehavior.health);
                Debug.Log("Enemy has been shot by a GUN");
            }
        }

        Destroy(gameObject);
    }
}
