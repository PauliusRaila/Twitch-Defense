using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shelterBuilding : MonoBehaviour {

    public float startHealth = 200;
    private float buildingHealth;
    public Image healthBar;

    void Start () {
        buildingHealth = startHealth;
        WaveManager.instance.shelterCount += 1;
    }

    public void decreaseHealth(float dmg)
    {
        buildingHealth -= dmg;

        healthBar.fillAmount = buildingHealth / startHealth;

        if (buildingHealth <= 0)
        {          

            WaveManager.instance.shelterCount -= 1;
            if (WaveManager.instance.shelterCount == 0) {
                GameObject.Find("rescueBuilding").tag = "EnemyTarget";

            }
              
;
            Destroy(gameObject);
        }

    }
}
