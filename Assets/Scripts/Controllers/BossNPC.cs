using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossNPC : MonoBehaviour {

    public float startHealth = 50;
    private float health;
    

    public float lookRadius = 5f;
    public float wanderRange = 20f;
    public float extraSpeed = 2f;
    public float infectRadius = 0.5f;
    public int chanceToSpawn;

    public Transform mainTarget;
    NavMeshAgent agent;
    GameObject healtCanvas;

    private NavMeshHit navHit;
    private Vector3 wanderTarget;
    public Image healthBar;

    //Behaviour states
    public enum EnemyState { isWandering, isChasing, isAttacking, isIdle}
    public EnemyState currentState;

    public int bossSpawnPositions = 1;

    Animator anim;

    // Use this for initialization
    void OnEnable () {
        InitializeReferences();
        
    }

    private void Start()
    {
        InvokeRepeating("LookForTarget", 1f, 2f);
        InvokeRepeating("checkIfShouldWander", 1f, 2f);
    }


    // Update is called once per frame
    void Update () {
        
    }

    private void FixedUpdate()
    {
        UpdateAnimations();

        if (currentState == EnemyState.isWandering)
            if (Vector3.Distance(wanderTarget, transform.position) <= 0.5f)
            {
                currentState = EnemyState.isIdle;

            }

        healtCanvas.transform.LookAt(Camera.main.transform);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, infectRadius);
    }

    private void InitializeReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        healtCanvas = transform.Find("healthCanvas").gameObject;
        health = startHealth;
        currentState = EnemyState.isIdle;
        //checkRate = Random.RandomRange(0.2f, 0.5f);
    }

    void checkIfShouldWander() {
        Debug.Log("checkIfShouldWander");
        if (currentState == EnemyState.isIdle && mainTarget == null)
        {
                agent.SetDestination(RandomWanderTarget(transform.position, wanderRange, out wanderTarget));
                currentState = EnemyState.isWandering;                                
        }
        

    }

    Vector3 RandomWanderTarget(Vector3 centre, float range, out Vector3 result) {
        bool isPointValid = false;
        while (!isPointValid) {
            Vector3 randomPoint = centre + Random.insideUnitSphere * wanderRange;

            if (NavMesh.SamplePosition(randomPoint, out navHit, 1.0f, 1))
            {
                result = navHit.position;
                return result;
            }
         
        }

        result = centre;
        return result;

    }

    private void DisableThis()
    {
        this.enabled = false;
    }


    private void LookForTarget()
    {              
        if (mainTarget == null)
        {
            GameObject[] enemyTargets = GameObject.FindGameObjectsWithTag("EnemyTarget");

            foreach (GameObject target in enemyTargets)
            {
                GameObject objectMain = target.transform.parent.gameObject;
                //use it...
            }



            foreach (GameObject target in enemyTargets)
            {

                float distance = Vector3.Distance(target.transform.position, transform.position);
                if (distance <= lookRadius)
                {
                    currentState = EnemyState.isChasing;
                    //if(target.transform.Find("entrance") != null)
                   //     mainTarget = target.transform.Find("entrance").transform;
                   // else
                        mainTarget = target.transform;
                }
            }
        }
        else{
            MoveToTarget();
            
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
            case EnemyState.isWandering:
                anim.SetBool("idle", false);
                anim.SetBool("attacking", false);
                break;
                
        }

    }

    private void MoveToTarget()
    {
        float distance = Vector3.Distance(mainTarget.transform.position, transform.position);
        if (distance <= lookRadius)
        {
            agent.SetDestination(mainTarget.position);
            if (distance <= infectRadius)
            {
                if (currentState != EnemyState.isAttacking) {
                    currentState = EnemyState.isAttacking;
                    //agent.Stop();
                }
                    
                //Infect(mainTarget);
            }
                
            
           
            }
            //else
           // {
           //     mainTarget = null;
           //     isChasing = false;
           // }
        
    }

    private void Infect(Transform target) {
        //target.tag = "Enemy";
        mainTarget = null;
        
    }

    public void inflictDamage() {
        if (mainTarget == null)
        {
            currentState = EnemyState.isIdle;
            return;
        }
        else if(Vector3.Distance(mainTarget.transform.position, transform.position) <= infectRadius) {
            if (mainTarget.GetComponent<shelterBuilding>() != null)
                mainTarget.GetComponent<shelterBuilding>().decreaseHealth(10);
            else
                mainTarget.GetComponent<rescueBuilding>().decreaseHealth(10);
        }
                 
        
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
