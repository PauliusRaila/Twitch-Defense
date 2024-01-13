using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyNPC : MonoBehaviour {

    public float startHealth = 50;
    private float health;
    

   // public float lookRadius = 5f;
   // public float wanderRange = 20f;
    public float extraSpeed = 2f;
    public float infectRadius = 0.5f;
    public int chanceToSpawn;

    NavMeshAgent agent;
    NavMeshObstacle navObstacle;
    GameObject healtCanvas;
    public Transform nearestTarget = null;

    private NavMeshHit navHit;
    public Image healthBar;

    //Behaviour states
    public enum EnemyState { isChasing, isAttacking, isIdle }
    public EnemyState currentState;

    Animator anim;

    // Use this for initialization
    void OnEnable () {
        InitializeReferences();        
    }

    private void Start()
    {
        UpdateTarget();
    }

    private void Update() {
        UpdateAnimations();
    }

    private void FixedUpdate()
    {      
        if (nearestTarget != null && currentState != EnemyState.isAttacking)
        {
            if (checkDistanceToTarget() <= infectRadius)
            {              
                currentState = EnemyState.isAttacking;
                agent.enabled = false;
                navObstacle.enabled = true;
            }
        }
        else if (nearestTarget == null) {
            UpdateTarget();
        }

        healtCanvas.transform.LookAt(Camera.main.transform);
    }


   // private void OnDrawGizmosSelected()
   //  {
   //     Gizmos.color = Color.red;
   //     Gizmos.DrawWireSphere(transform.position, lookRadius);
   //
   //     Gizmos.color = Color.green;
   //     Gizmos.DrawWireSphere(transform.position, infectRadius);
   // }

    private float checkDistanceToTarget() {
        float distanceToEnemy = Vector3.Distance(nearestTarget.transform.position, transform.position);
        return distanceToEnemy;
    }

    private void InitializeReferences()
    {
        navObstacle = GetComponent<NavMeshObstacle>();
        agent = GetComponent<NavMeshAgent>();
        
        anim = GetComponent<Animator>();
        healtCanvas = transform.Find("healthCanvas").gameObject;
        health = startHealth;
        currentState = EnemyState.isIdle;
        //checkRate = Random.RandomRange(0.2f, 0.5f);
    }

    private void DisableThis()
    {
        this.enabled = false;
    }


    public void UpdateTarget()
    {
        GameObject[] enemyTargets = GameObject.FindGameObjectsWithTag("EnemyTarget");
        float shortestDistance = Mathf.Infinity;
        

        foreach (GameObject target in enemyTargets)
        {
            float distanceToEnemy = Vector3.Distance(target.transform.position, transform.position);

            if (distanceToEnemy <= shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestTarget = target.transform;

                currentState = EnemyState.isChasing;

                MoveToTarget(nearestTarget);
            }
        }
    }

    private void UpdateAnimations()
    {
        switch (currentState) {
            case EnemyState.isAttacking:
                anim.SetBool("idle", false);
                anim.SetBool("attacking", true);            
                break;
            case EnemyState.isChasing:               
                anim.SetBool("idle", false);
                anim.SetBool("attacking", false);
                break;
            case EnemyState.isIdle:
                anim.SetBool("attacking", false);
                anim.SetBool("idle", true);
                break;              
        }
    }

    private void MoveToTarget(Transform nearestTarget)
    {
        float distance = Vector3.Distance(nearestTarget.transform.position, transform.position);

        if (agent.enabled == false) {
            navObstacle.enabled = false;
            agent.enabled = true;
        }
        
        agent.SetDestination(nearestTarget.position);                 
    }

    public void inflictDamage() {

     
            if (nearestTarget.GetComponent<shelterBuilding>() != null)
                nearestTarget.GetComponent<shelterBuilding>().decreaseHealth(10);
            else
                nearestTarget.GetComponent<rescueBuilding>().decreaseHealth(10);                           
    }

    public void decreaseHealth(float dmg) {
        health -= dmg;
        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            WaveManager.instance.totalEnemiesAlive--;
            WaveManager.instance.checkWaveStatus();
            Destroy(gameObject);
        }           
    }
}
