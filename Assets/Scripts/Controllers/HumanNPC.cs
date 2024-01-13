using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HumanNPC : MonoBehaviour {   

    public float lookRadius = 5f;
    public float wanderRange = 20f;
    public float extraSpeed = 2f;
    private float normalSpeed = 3f;

    public Transform mainTarget;
    NavMeshAgent agent;

    private float checkRate = 0.1f;
    private float nextCheck;
    
    private NavMeshHit navHit;
    private Vector3 wanderTarget;
    

    //Behaviour states[changle later that green stuff that i forgot about
    public bool isWandering = false;
    public bool isMovingToTarget = false;
    public bool isFleeing = false;

    Animator anim;

    // Use this for initialization
    void OnEnable () {
        InitializeReferences();

	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextCheck) {
            checkIfShouldWander();
        }

        if (isWandering)
            if (Vector3.Distance(wanderTarget, transform.position) <= 0.5f) {              
                isWandering = false;
                anim.SetBool("idle", true);
            }               

        if (transform.tag == "Human")
        {
            Flee();
        }
        else if (isFleeing)
            isFleeing = false;

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void InitializeReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        checkRate = Random.RandomRange(0.5f, 2f);
    }

    void checkIfShouldWander() {
        if (!isWandering && !isFleeing)
        {
            if (RandomWanderTarget(transform.position, wanderRange, out wanderTarget))
            {
                agent.SetDestination(wanderTarget);
                anim.SetBool("idle", false);
                isWandering = true;              
            }
        }
       
    }

    bool RandomWanderTarget(Vector3 centre, float range, out Vector3 result) {
        Vector3 randomPoint = centre + Random.insideUnitSphere * wanderRange;
        if (NavMesh.SamplePosition(randomPoint, out navHit, 1.0f,1)) {
            result = navHit.position;
            return true;
        }
        else
        {
            result = centre;
            return false;
        }
    }

    private void DisableThis()
    {
        this.enabled = false;
    }

    private void Flee()
    {
       

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject target in enemies)
        {

            float distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance <= lookRadius)
            {
                Vector3 dirToEnemy = transform.position - target.transform.position;
                Vector3 newPos = transform.position + dirToEnemy;


                agent.SetDestination(newPos);
                agent.speed = normalSpeed + extraSpeed;
                isWandering = false;
                isFleeing = true;
            }
            else
            {
                agent.speed = normalSpeed;
                isFleeing = false;
            }
        }
    }
}
