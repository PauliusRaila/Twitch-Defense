using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerController : MonoBehaviour {

    public string playerName;

    public GameObject currentTarget;
    public Building currentBuilding;
    public bool isPlayerInside = false;
    public Weapon equipedWeapon;
    public Transform weaponSlot;
    private Animator anim;

    public enum PlayerClass { sharpshooter, soldier, demolition, scientist }

    [SerializeField]
    public PlayerClass currentClass = PlayerClass.sharpshooter; //sharpshooter as default.
    public int classLevel, characterLevel = 1;

    private float fireCountdown = 1f;


    [HideInInspector]
    public NavMeshAgent agent;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (equipedWeapon != null)
            Gizmos.DrawWireSphere(transform.position, equipedWeapon.shootRadius);
    }

    public void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);

            if (distanceToEnemy <= shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
                //currentTarget = target;
            }


            if (nearestEnemy != null && shortestDistance <= equipedWeapon.shootRadius)
            {
                currentTarget = nearestEnemy;
            }
            else {
                currentTarget = null;
            }
        }


    }


    void Awake() {
        

       

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        EquipWeapon("Pistol");

       
        //agent.Warp(playerManager.instance.spawnPoint.position);



        InvokeRepeating("UpdateTarget", 0f, 1f);
    }


    void Update() {

        if (currentTarget == null) { }
        else {
            Vector3 dir = currentTarget.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 20f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            equipedWeapon.Shoot(currentTarget.transform);
        }
    }

    public void enterBuilding(Transform roof) {
        agent.enabled = false;
        this.transform.position = roof.position;
        isPlayerInside = true;
        anim.SetBool("moving", false);
    }

    public void exitBuilding(Transform entrance)
    {
        this.transform.position = entrance.position;
        agent.enabled = true;
        isPlayerInside = false;
        anim.SetBool("moving", true);
    }

    public void EquipWeapon(string wepName) {
        GameObject weapon = (GameObject)Instantiate(Resources.Load("Weapons/" + wepName));
        
        weapon.transform.parent = weaponSlot;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = new Quaternion(0, 0, 0, 0);
        equipedWeapon = weapon.GetComponent<Weapon>();
        setClassPerks();
    }

    public void moveToBuilding() {
        agent.SetDestination(currentBuilding.entrance.position);
        FlowLines.instance.playerUIpositions.Add(GameObject.Find(playerName + "UI").transform);
        FlowLines.instance.playerPositions.Add(this.transform);
    }

    public void changeClass(string className) {

        if (currentClass.ToString() == className)
            return;
        else {
            currentClass = (PlayerClass)System.Enum.Parse(typeof(PlayerClass), className);
            if (GameObject.Find(name + "UI"))
                GameObject.Find(name + "UI").GetComponent<Image>().sprite = Resources.Load<Sprite>("ClassImages/" + currentClass.ToString());

            return;
        }

    }

    public void setClassPerks() {     
        
        switch (currentClass) {
            case PlayerClass.sharpshooter:
              

                float levelMultiplier = classLevel * 0.5f;
                float critRate = equipedWeapon.critRate / 100 * levelMultiplier;
                float shootRadius = equipedWeapon.shootRadius / 100 * levelMultiplier;

                equipedWeapon.critRate += critRate;
                equipedWeapon.shootRadius += shootRadius;


                break;

            case PlayerClass.demolition:

                break;

            case PlayerClass.scientist:
          
                break;

            case PlayerClass.soldier:

                break;

        }

    }

}
