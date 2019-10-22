using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public float turnRate = 10f;
    public GameObject flameThrower;
    public Transform[] patrolPoints;
    private int patrolPointIndex = 0;
    public AudioClip enemyFlamethrower;

    Transform target;
    NavMeshAgent agent;

    private void Start()
    {
        flameThrower.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        target = PlayerManager.instance.Player.transform;
    }

    private void Update()
    {
        IfArcade();

    }

    void FacePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnRate);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void ChasePlayer()
    {
        agent.SetDestination(target.position);
    }

    void AttackPlayer()
    {
        flameThrower.SetActive(true);
    }

    void StopAttackPlayer()
    {
        flameThrower.SetActive(false);
    }


    //Disabled do game inconsistensy
    void MoveToNextPatrolPoint()
    {
        if(patrolPoints.Length > 0)
        {
            agent.destination = patrolPoints[patrolPointIndex].position;
            patrolPointIndex ++;
            patrolPointIndex %= patrolPoints.Length;
        }
    }

    void IfArcade()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (GameManager.instance.normal) //add a boolean to check if dead, because on death inner parts of the enemy and the light still follow the enemy
        {
            if (distance <= lookRadius)
            {
                FacePlayer();
                ChasePlayer();

                if (distance <= agent.stoppingDistance)
                {
                    FacePlayer();
                    AttackPlayer();
                }
                else
                {
                    StopAttackPlayer();
                }
            }

        }
        else if (GameManager.instance.arcade)
        {
            ChasePlayer();
            if (distance <= agent.stoppingDistance)
            {
                FacePlayer();
                AttackPlayer();
            }
            else
            {
                StopAttackPlayer();
            }
        }
    }

}
